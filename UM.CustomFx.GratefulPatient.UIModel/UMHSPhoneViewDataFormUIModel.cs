
using Blackbaud.AppFx.UIModeling.Core;

namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class UMHSPhoneViewDataFormUIModel
	{

		private void UMHSPhoneViewDataFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
            this.HYPHEN.Value = " - ";
            this.UpdateConfidentialImage();
            this.UpdatePrimaryImage();
            this.UpdateDateRangeInformation();
            this.UpdateDoNotCallDescription();
           // this.UpdateCallBeforeAfter();
           // this.UpdateInformationSource();
           // this.UpdateContactInfo();
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
                this.PRIMARYIMAGE.ValueDisplayStyle = ValueDisplayStyle.Hidden;
        }

        private void UpdateDateRangeInformation()
        {
            if (this.STARTDATE.HasValue())
                this.DATEADDED.Visible = false;
            else
                this.STARTDATE.Visible = false;
            if (!this.ENDDATE.HasValue())
            {
                this.ENDDATEDESCRIPTION.Value = "Present";
                this.ENDDATE.Visible = false;
            }
            else
                this.ENDDATEDESCRIPTION.Visible = false;
            //if (this.SEASONALSTARTDATE.HasValue())
            //{
            //    this.SEASONALRANGEDESCRIPTION.Caption = "Seasonal";
            //    this.SEASONALRANGEDESCRIPTION.Value = this.SEASONALSTARTDATE.Value.ToString() + " - " + this.SEASONALENDDATE.Value.ToString();
            //}
            //else
            //    this.SEASONALRANGEDESCRIPTION.Visible = false;
        }
        private void UpdateDoNotCallDescription()
        {
            if (this.DONOTCALL.Value)
            {
                this.DONOTCALLIMAGE.ValueDisplayStyle = ValueDisplayStyle.WarningImageOnly;
                this.DONOTCALLDESCRIPTION.Value = "Do not call";
                this.DONOTCALLDESCRIPTION.ValueDisplayStyle = ValueDisplayStyle.WarningTextOnly;
            }
            else
                this.DONOTCALL.Visible = false;
        }
       

        


        #region "Event handlers"

        partial void OnCreated()
		{
			this.Loaded += UMHSPhoneViewDataFormUIModel_Loaded;
		}

#endregion

	}

}