using System;
using System.Data;
using System.Data.SqlClient;

namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class MIMEDTeamAddFormUIModel
	{

		private void MIMEDTeamAddFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
		    switch (this.Mode)
		    {
		        case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Edit:
		            this.FORMHEADER.Value = "Edit Team Member";
		            break;
		        case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add:
		            this.FORMHEADER.Value = "Add Team Member";
		            break;
		    }

        }
	    private void TEAMMEMBERID_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
	    {
	        var con = new SqlConnection(this.GetRequestContext().AppDBConnectionString());
	        con.Open();

            _siteid.Value = Guid.Empty;

            using (SqlCommand command = con.CreateCommand())
	        {
	            _siteid.ResetDataSource();
	            command.CommandType = CommandType.StoredProcedure;
	            command.CommandText = "USR_USP_MIMEDDEFAULTSITEFORMEMBER";
	            command.CommandTimeout = 7200;
	            command.Parameters.AddWithValue("@TEAMMEMBERID", this.TEAMMEMBERID.Value);

	            SqlDataReader reader = command.ExecuteReader();
	            try
	            {
	                if (reader.HasRows)
	                {
	                    reader.Read();
	                    _siteid.Value = reader.GetGuid(0);
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


        #region "Event handlers"

        partial void OnCreated()
		{
			this.Loaded += MIMEDTeamAddFormUIModel_Loaded;
            this.TEAMMEMBERID.ValueChanged += TEAMMEMBERID_ValueChanged;
		}

 
        #endregion

    }

}