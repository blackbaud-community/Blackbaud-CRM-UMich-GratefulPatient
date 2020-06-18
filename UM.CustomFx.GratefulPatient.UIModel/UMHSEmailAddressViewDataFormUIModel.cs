using Blackbaud.AppFx.UIModeling.Core;
using Blackbaud.AppFx.Constituent.UIModel;

namespace UM.CustomFx.GratefulPatient.UIModel
{

    public partial class UMHSEmailAddressViewDataFormUIModel
    {

        private void UMHSEmailAddressViewDataFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
        {
            this.HYPHEN.Value = " - ";
            this.UpdatePrimaryImage();
            this.UpdateDoNotEmailDescription();
            //this.UpdateBouncedEmailDescription();
            //this.UpdateInformationSource();
            //this.UpdateContactInfo();
            this.UpdateDateRangeInformation();
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
            if (this.DONOTEMAIL.Value)
            {
                this.DONOTEMAILIMAGE.ValueDisplayStyle = ValueDisplayStyle.WarningImageOnly;
                this.DONOTEMAILDESCRIPTION.Value = "Do not email";//Content.Email_DoNotEmail;
                this.DONOTEMAILDESCRIPTION.ValueDisplayStyle = ValueDisplayStyle.WarningTextOnly;
            }
            else
                this.DONOTEMAIL.Visible = false;
        }

        //private void UpdateInformationSource()
        //{
        //    StringField infosourcecode = this.INFOSOURCECODE;
        //    EmailAddressViewFormUIModel.ORIGINCODES? nullable1;
        //    int? nullable2;
        //    int? nullable3;
        //    bool? nullable4;
        //    int num1;
        //    if (this.INFOSOURCECODE.HasValue())
        //    {
        //        int num2 = 0;
        //        nullable1 = this.ORIGINCODE.Value;
        //        int? nullable5;
        //        if (!nullable1.HasValue)
        //        {
        //            nullable5 = new int?();
        //        }
        //        else
        //        {
        //            nullable2 = new int?((int)nullable1.GetValueOrDefault());
        //            nullable5 = nullable2;
        //        }
        //        nullable3 = nullable5;
        //        bool? nullable6;
        //        if (!nullable3.HasValue)
        //        {
        //            nullable6 = new bool?();
        //        }
        //        else
        //        {
        //            nullable4 = new bool?(nullable3.GetValueOrDefault() == num2);
        //            nullable6 = nullable4;
        //        }
        //        if ((bool)nullable6)
        //        {
        //            num1 = 1;
        //            goto label_10;
        //        }
        //    }
        //    num1 = 0;
        //    label_10:
        //    infosourcecode.Visible = num1 != 0;
        //    this.INFOSOURCECOMMENTS.Visible = this.INFOSOURCECOMMENTS.HasValue();
        //    StringField origincodetranslation = this.ORIGINCODETRANSLATION;
        //    int num3 = 0;
        //    nullable1 = this.ORIGINCODE.Value;
        //    int? nullable7;
        //    if (!nullable1.HasValue)
        //    {
        //        nullable7 = new int?();
        //    }
        //    else
        //    {
        //        nullable3 = new int?((int)nullable1.GetValueOrDefault());
        //        nullable7 = nullable3;
        //    }
        //    nullable2 = nullable7;
        //    bool? nullable8;
        //    if (!nullable2.HasValue)
        //    {
        //        nullable8 = new bool?();
        //    }
        //    else
        //    {
        //        nullable4 = new bool?(nullable2.GetValueOrDefault() != num3);
        //        nullable8 = nullable4;
        //    }
        //    int num4 = (bool)nullable8 ? 1 : 0;
        //    origincodetranslation.Visible = num4 != 0;
        //    this.ORIGINCODETRANSLATION.Value = this.ORIGINCODE.Translation();
        //}

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
        }


        #region "Event handlers"

        partial void OnCreated()
        {
            this.Loaded += UMHSEmailAddressViewDataFormUIModel_Loaded;
        }

        #endregion

    }
}


