using System;

namespace UM.CustomFx.GratefulPatient.UIModel
{

    public partial class MIMEDAffiliationDeleteGlobalChangeUIModel
    {

        private void MIMEDAffiliationDeleteGlobalChangeUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
        {

        }

        #region "Event handlers"

        partial void OnCreated()
        {
            this.Loaded += MIMEDAffiliationDeleteGlobalChangeUIModel_Loaded;
            this.AFFILIATIONBASENAMECODEID.ValueChanged += AFFILIATIONBASENAMECODEID_ValueChanged;
            this.AFFILIATIONCATEGORYCODEID.ValueChanged += AFFILIATIONCATEGORYCODEID_ValueChanged;
            this.DEPARTMENTID.ValueChanged += DEPARTMENTID_ValueChanged;

        }

        private void DEPARTMENTID_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            _subdepartmentid.Value = Guid.Empty;
            _subdepartmentid.ResetDataSource();

        }

        private void AFFILIATIONCATEGORYCODEID_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            if (this.AFFILIATIONCATEGORYCODEID.Value != Guid.Empty)
            {
                AFFILIATIONBASENAMECODEID.ResetDataSource();
            }

        }

        private void AFFILIATIONBASENAMECODEID_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            if (this.AFFILIATIONCATEGORYCODEID.Value != Guid.Empty)
            {
                AFFILIATIONBASENAMECODEID.ResetDataSource();
            }
        }
        #endregion
    }
}