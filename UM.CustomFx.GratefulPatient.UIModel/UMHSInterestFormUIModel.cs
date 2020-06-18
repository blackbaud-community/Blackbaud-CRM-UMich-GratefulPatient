using System;

namespace UM.CustomFx.GratefulPatient.UIModel
{
    public partial class UMHSInterestFormDEPARTMENTSUIModel
    {
        partial void OnCreated()
        {
            this.DEPARTMENTID.ValueChanged += DEPARTMENTID_ValueChanged;
        }

        private void DEPARTMENTID_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            this.SUBDEPARTMENTID.Value = Guid.Empty;
            this.SUBDEPARTMENTID.ResetDataSource();
        }
    }


	public partial class UMHSInterestFormUIModel
	{                
        private void UMHSInterestFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
            switch (this.Mode)
            {
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Edit:
                    this.FORMHEADER.Value = "Edit MIMED Interests";
                    break;
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add:
                    this.FORMHEADER.Value = "Add MIMED Interests";

                    break;
            }
        }

#region "Event handlers"

		partial void OnCreated()
		{
			this.Loaded += UMHSInterestFormUIModel_Loaded;
		}

#endregion

	}

}