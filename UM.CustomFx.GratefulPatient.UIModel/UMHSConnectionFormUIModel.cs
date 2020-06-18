using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Blackbaud.AppFx.Server;
using UM.CustomFx.GratefulPatient.Models;

namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class UMHSConnectionFormUIModel
	{

		private void UMHSConnectionFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
            switch (this.Mode)
            {
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Edit:
                    this.FORMHEADER.Value = "Edit Patient Connection";
                    break;
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add:
                    this.FORMHEADER.Value = "Add Patient Connection";
                    _infosourcecodeid.ResetDataSource();
                    break;
            }

            

        }

        private void _Saving(object sender, Blackbaud.AppFx.UIModeling.Core.SavingEventArgs e)
        {
            if (this.Mode == Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add ) //Check this while adding only 
            { 
                if (RELATIONSHIPCODEID.Value == Guid.Parse("437C723A-AA22-49B9-977E-96E389D21A6A")) //Self
                {    
                        DataListLoadRequest request = new DataListLoadRequest
                        {
                            DataListID = new Guid("5a96ea16-83bc-4dcc-a2a5-a66f4065de0a"),
                            ContextRecordID = this.ContextRecordId,
                            SecurityContext = this.GetRequestSecurityContext()
                        };
                    AppFxWebService svc = new AppFxWebService(this.GetRequestContext());
                    var reply = svc.DataListLoad(request);

                        foreach (DataListResultRow row in reply.Rows)
                    {
                        if (row.Values[5] != null && row.Values[5].ToLower().Equals("self") )
                        {
                            SaveFailed();
                            break;
                        }                                           
                    }
                }
            }
        }

        private new void SaveFailed()
        {
            throw new NotImplementedException("There can be only one SELF entry!");
        }

        #region "Event handlers"

        partial void OnCreated()
		{
			this.Loaded += UMHSConnectionFormUIModel_Loaded;
            
            this.Saving += _Saving;

		}

#endregion

	}

}