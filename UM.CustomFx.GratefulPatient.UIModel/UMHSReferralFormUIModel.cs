using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using Blackbaud.AppFx.BatchUI;
using Blackbaud.AppFx.Server;
using UM.CustomFx.GratefulPatient.Models;

namespace UM.CustomFx.GratefulPatient.UIModel
{

    public partial class UMHSReferralFormUIModel
    {
        UMHSDataRepo umhsData = new UMHSDataRepo();

        private void UMHSReferralFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
        {
            switch (this.Mode)
            {
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Edit:
                    this.FORMHEADER.Value = "Edit MIMED Referral";
                    switch (this.FOLLOWUPSTATUSCODE.Value)
                    {
                        case FOLLOWUPSTATUSCODES.@Successful:
                            this.FOLLOWUPSTATUSCODEID.Value = Guid.Parse("4a4cd034-1fb4-4ba4-8940-08bd066baf42");
                            //this.FOLLOWUPSTATUSCODE.Value = FOLLOWUPSTATUSCODES.Successful;
                            break;
                        case FOLLOWUPSTATUSCODES.@Unsuccessful:
                            this.FOLLOWUPSTATUSCODEID.Value = Guid.Parse("10043a3f-e0d6-4e24-8a85-26a23895bd90");
                            break;
                    }
                    this.FOLLOWUPSTATUSCODEID.Required = this._followupcompleted.Value;
                    this.FOLLOWUPSTATUSCODEID.Enabled = this._followupcompleted.Value;
                    if (this.FOLLOWUPSTATUSCODEID.Enabled == false)
                    {
                        this.FOLLOWUPSTATUSCODEID.Value = Guid.Empty;
                    }

                    

                    break;
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add:
                    this.FORMHEADER.Value = "Add MIMED Referral";
                    _subdepartmentid.Value = Guid.Empty;
                    _departmentid.Value = Guid.Empty;
                    this.FOLLOWUPSTATUSCODEID.Required = this._followupcompleted.Value;
                    this.FOLLOWUPSTATUSCODEID.Enabled = this._followupcompleted.Value;
                    if (this.FOLLOWUPSTATUSCODEID.Enabled == false)
                    {
                        this.FOLLOWUPSTATUSCODEID.Value = Guid.Empty;
                        this.FOLLOWUPSTATUSCODE.Value = FOLLOWUPSTATUSCODES.@Empty;
                    }
                    break;
            }
            
        }
        private void _referrerid_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            try
            {
                this.DEPARTMENTID.ResetDataSource();
                this.SUBDEPARTMENTID.ResetDataSource();
                _subdepartmentid.Value = Guid.Empty;
                _departmentid.Value = Guid.Empty;

                _departmentid.Value = umhsData.GetDepartment(this.REFERRERID.Value).Value;

                if (_departmentid.HasValue())
                {
                    _subdepartmentid.Value = umhsData.GetSubDepartment(_referrerid.Value, _departmentid.Value).Value;
                }
            }
            catch { }
        }

        private void _followupcompleted_ValueChanged(object sender,Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            if (!this.Loading)
            { 
                this.FOLLOWUPSTATUSCODEID.Required = this._followupcompleted.Value;
                this.FOLLOWUPSTATUSCODEID.Enabled = this._followupcompleted.Value;
                if (this.FOLLOWUPSTATUSCODEID.Enabled == false && this._followupcompleted.Value == false)
                {
                    this.FOLLOWUPSTATUSCODEID.Value = Guid.Empty;
                    this.FOLLOWUPSTATUSCODE.Value = FOLLOWUPSTATUSCODES.@Empty;
                }
            }
        }


        private void _Saving(object sender, Blackbaud.AppFx.UIModeling.Core.SavingEventArgs e)
        {
            switch (this.FOLLOWUPSTATUSCODEID.Value.ToString())
            {
                case "4a4cd034-1fb4-4ba4-8940-08bd066baf42":
                    _followupstatuscode.Value = FOLLOWUPSTATUSCODES.@Successful;
                    break;
                case "10043a3f-e0d6-4e24-8a85-26a23895bd90":
                    this.FOLLOWUPSTATUSCODE.Value = FOLLOWUPSTATUSCODES.@Unsuccessful;
                    break;
            }

            if (this._followupcompleted.Value == false)
            {
                this.FOLLOWUPSTATUSCODEID.Value = Guid.Empty;
                this.FOLLOWUPSTATUSCODE.Value = FOLLOWUPSTATUSCODES.@Empty;
            }
        }


        #region "Event handlers"

        partial void OnCreated()
        {
            this.Loaded += UMHSReferralFormUIModel_Loaded;
            this.REFERRERID.ValueChanged += _referrerid_ValueChanged;
            this.DEPARTMENTID.ValueChanged += DEPARTMENTID_ValueChanged;
            this.FOLLOWUPCOMPLETED.ValueChanged += _followupcompleted_ValueChanged;
            this.Saving += _Saving;
        }

        private void DEPARTMENTID_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            this.SUBDEPARTMENTID.ResetDataSource();
            _subdepartmentid.Value = Guid.Empty;
        }

        #endregion

    }

}