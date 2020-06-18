namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class UMHSEmailAddressFormUIModel
	{

		private void UMHSEmailAddressFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
            switch (this.Mode)
            {
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Edit:
                    this.FORMHEADER.Value = "Edit MIMED EmailAddress";
                    break;
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add:
                    this.FORMHEADER.Value = "Add MIMED EmailAddress";
                    _infosourcecodeid.ResetDataSource();
                    break;
            }

            if (this.ISPRIMARY.Value)
            {
                this.ISPRIMARY.Enabled = false;
            }
        }

#region "Event handlers"

		partial void OnCreated()
		{
			this.Loaded += UMHSEmailAddressFormUIModel_Loaded;
        }

#endregion

	}

}