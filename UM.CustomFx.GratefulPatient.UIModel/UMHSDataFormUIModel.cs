using System;
using Blackbaud.AppFx.Server;
using System.Data.SqlClient;
using UM.CustomFx.GratefulPatient.Models;

namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class UMHSDataFormUIModel
	{

		private void UMHSDataFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
            switch(this.Mode)
            {
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Edit:
                    this.FORMHEADER.Value = "Edit MIMED Info";
                    break;
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add:
                    this.FORMHEADER.Value = "Add MIMED Info";
                    break;
            }
            if (ISDMINELIGIBLE.Value == true)
            {
                this.DMINELIGIBLEREASONCODEID.Enabled = true;
                this.DMINELIGIBLEREASONCODEID.Required = this.ISDMINELIGIBLE.Value;
            }
            else
            {
                this.DMINELIGIBLEREASONCODEID.Value = Guid.Empty;
                this.DMINELIGIBLEREASONCODEID.Enabled = false;
                this.DMINELIGIBLEREASONCODEID.Required = this.ISDMINELIGIBLE.Value;

            }
        }
        private void _Saving(object sender, Blackbaud.AppFx.UIModeling.Core.SavingEventArgs e)
        {           
            if  (this.RecordId != null)
            { 
                SqlConnection conn = new SqlConnection(ConnectionStringHelper.GetConnectionString());
                conn.Open();
                SqlCommand cmd = new SqlCommand("select ID from USR_UMHS_DATA U WHERE U.MRN = (@MRN) and U.CONSTITUENTID <> (@ID)", conn);
                cmd.Parameters.AddWithValue("@ID", this.RecordId);
                cmd.Parameters.AddWithValue("@MRN", this.MRN.Value.ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataSet ds = new System.Data.DataSet();
                da.Fill(ds);
                Object returnObjVal = cmd.ExecuteNonQuery();
                if (ds.Tables[0].Rows.Count > 0)
                {      
                         
                    conn.Close();
                    SaveFailed();
                }
            }           
        }
        private void _isdmineligible_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            if (ISDMINELIGIBLE.Value == true)
            { 
                this.DMINELIGIBLEREASONCODEID.Enabled = true;
                this.DMINELIGIBLEREASONCODEID.Required = this.ISDMINELIGIBLE.Value;
            }
            else
            {
                this.DMINELIGIBLEREASONCODEID.Value = Guid.Empty;
                this.DMINELIGIBLEREASONCODEID.Enabled = false;
                this.DMINELIGIBLEREASONCODEID.Required = this.ISDMINELIGIBLE.Value;
                
            }
        }
        private new void SaveFailed()
        {
            throw new NotImplementedException("The MRN already exists in the system. Enter another MRN.");
        }

        #region "Event handlers"

        partial void OnCreated()
		{
			this.Loaded += UMHSDataFormUIModel_Loaded;
            this.BIRTHDATE.ValueChanged += BIRTHDATE_ValueChanged;
            this.Saving += _Saving;
            this.ISDMINELIGIBLE.ValueChanged += _isdmineligible_ValueChanged;
        }

        private void BIRTHDATE_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            //fuzzy date makes this harder than it should be, consider using Date for UI Field
            AGE.Value = Helpers.Common.CalculateAge((Blackbaud.AppFx.FuzzyDate)e.NewValue).ToString();
        }
        #endregion
    }

}