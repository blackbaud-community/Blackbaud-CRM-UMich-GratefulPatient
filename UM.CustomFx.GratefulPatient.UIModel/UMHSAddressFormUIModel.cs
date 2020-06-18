namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class UMHSAddressFormUIModel
	{

		private void UMHSAddressFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
            switch (this.Mode)
            {
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Edit:
                    this.FORMHEADER.Value = "Edit MIMED Address";
                    break;
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add:
                    this.FORMHEADER.Value = "Add MIMED Address";
                    _infosourcecodeid.ResetDataSource();
                    break;
            }
            this.DONOTMAILREASONCODEID.Enabled = this.DONOTMAIL.Value;
            if (this.ISPRIMARY.Value)
            {
                this.ISPRIMARY.Enabled = false;                
            }
        }
        private void _donotmail_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            this.DONOTMAILREASONCODEID.Enabled = true;           
            this.DONOTMAILREASONCODEID.Enabled = this.DONOTMAIL.Value;
        }

        #region "Event handlers"

        partial void OnCreated()
		{
			this.Loaded += UMHSAddressFormUIModel_Loaded;
            this.DONOTMAIL.ValueChanged += _donotmail_ValueChanged;
        }

#endregion

	}

}