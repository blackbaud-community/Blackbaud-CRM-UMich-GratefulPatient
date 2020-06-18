using Blackbaud.AppFx.UIModeling.Core;

namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class UMHSAddressViewDataFormUIModel
	{

		private void UMHSAddressViewDataFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
            this.HYPHEN.Value = " - ";
            this.UpdateConfidentialImage();
            this.UpdatePrimaryImage();
            this.UpdateDoNotEmailDescription();           
            this.UpdateDateRangeInformation();
        }

        private void UpdateConfidentialImage()
        {
            if (this.ISCONFIDENTIAL.Value)
            {
                this.CONFIDENTIALDESCRIPTION.Value = "Confidential";
            }
            else
            {
                this.CONFIDENTIALIMAGE.Visible = false;
                this.CONFIDENTIALDESCRIPTION.Visible = false;
                this.ISCONFIDENTIAL.Visible = false;
            }
        }
        private void UpdatePrimaryImage()
        {
            if (this.PRIMARY.Value)
                this.PRIMARYIMAGE.ValueDisplayStyle = ValueDisplayStyle.GoodImageOnly;
            else
                this.PRIMARY.Visible = false;
        }
        private void UpdateDoNotEmailDescription()
        {
            if (this.DONOTMAIL.Value)
            {
                this.DONOTMAILIMAGE.ValueDisplayStyle = ValueDisplayStyle.WarningImageOnly;
                this.DONOTMAILDESCRIPTION.Value = "Do not mail";//Content.Email_DoNotEmail;
                this.DONOTMAILDESCRIPTION.ValueDisplayStyle = ValueDisplayStyle.WarningTextOnly;
            }
            else
                this.DONOTMAIL.Visible = false;
        }
        private void UpdateDateRangeInformation()
        {
            if (this.HISTORICALSTARTDATE.HasValue())
                this.DATEADDED.Visible = false;
            else
                this.HISTORICALSTARTDATE.Visible = false;
            if (!this.HISTORICALENDDATE.HasValue())
            {
                this.HISTORICALENDDATEDESCRIPTION.Value = "Present";
                this.HISTORICALENDDATE.Visible = false;
            }
            else
                this.HISTORICALENDDATEDESCRIPTION.Visible = false;
        }
        #region "Event handlers"

        partial void OnCreated()
		{
			this.Loaded += UMHSAddressViewDataFormUIModel_Loaded;
		}

#endregion

	}

}