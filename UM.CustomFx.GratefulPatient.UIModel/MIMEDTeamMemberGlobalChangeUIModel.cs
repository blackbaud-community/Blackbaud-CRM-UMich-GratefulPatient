using System;
using System.Data;
using System.Data.SqlClient;
using Blackbaud.AppFx.UIModeling.Core;

namespace UM.CustomFx.GratefulPatient.UIModel
{
    public static class MIMEDTeamMemberGlobalChange
    {
        public static void AutoPopulateTeamMemberSite(string connectionString, Guid teamMemberId, SimpleDataListField<Guid> teamMemberSiteId)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            teamMemberSiteId.Value = Guid.Empty;

            using (SqlCommand command = con.CreateCommand())
            {
                teamMemberSiteId.ResetDataSource();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USR_USP_MIMEDDEFAULTSITEFORMEMBER";
                command.CommandTimeout = 7200;
                command.Parameters.AddWithValue("@TEAMMEMBERID", teamMemberId);

                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        teamMemberSiteId.Value = reader.GetGuid(0);
                    }
                }
                catch { }
                finally
                {
                    reader.Close();
                }
            }
            con.Close();
        }
    }

    public partial class MIMEDTeamMemberGlobalChangeUIModel
	{
	    private void MIMEDTeamMemberGlobalChangeUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
	    {
	        if (this.FORMHEADER.Caption == "Add MIMED Team Member")
	        {
	            _teamrolecodeid.Required = true;
	            _startdate.Required = true;
	        }
	    }

	    private void TEAMMEMBERID_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
	    {
	        MIMEDTeamMemberGlobalChange.AutoPopulateTeamMemberSite(this.GetRequestContext().AppDBConnectionString(), this.TEAMMEMBERID.Value, _teammembersiteid);
	    }

        #region "Event handlers"

        partial void OnCreated()
		{
			this.TEAMMEMBERID.ValueChanged += TEAMMEMBERID_ValueChanged;
		    this.Loaded += MIMEDTeamMemberGlobalChangeUIModel_Loaded;

		}
        #endregion

	}
}