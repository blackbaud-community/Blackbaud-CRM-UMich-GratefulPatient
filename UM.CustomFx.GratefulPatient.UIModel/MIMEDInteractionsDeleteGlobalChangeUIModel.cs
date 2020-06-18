using Blackbaud.AppFx.UIModeling.Core;
using System;
using UM.CustomFx.GratefulPatient.Models;

namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class MIMEDInteractionsDeleteGlobalChangeUIModel
	{

		private void MIMEDInteractionsDeleteGlobalChangeUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{

            CRMDataRepo crmData = new CRMDataRepo();

            //< FormFieldOverrides >
            //< FormFieldOverride FieldID = "RECORDTYPEID" ReadOnly = "true" DefaultValueText = "82CA852C-C3C9-430F-BC4C-ACA70AF90C16" />
            //</ FormFieldOverrides >

            Guid recordTypeID = crmData.GetRecordTypeID("MIMED Data");  


            FieldOverride item = new FieldOverride();

            item.ReadOnly = true;
            item.FieldId = "RECORDTYPEID";
            item.DefaultValueText = recordTypeID.ToString();
            this.IDSETREGISTERID.SearchFieldOverrides.Add(item);
        }

        #region "Event handlers"

        partial void OnCreated()
		{
			this.Loaded += MIMEDInteractionsDeleteGlobalChangeUIModel_Loaded;
		}

#endregion

	}

}