using UM.CustomFx.GratefulPatient.Models;
using System.Linq;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity.Core.Objects;

namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class UMHSAffiliationDataFormUIModel
	{

		private void UMHSAffiliationEditFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
            switch (this.Mode)
            {
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Edit:
                    this.FORMHEADER.Value = "Edit MIMED Affiliation";
                    break;
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add:
                    this.FORMHEADER.Value = "Add MIMED Affiliation";
                    //_infosourcecodeid.ResetDataSource();
                    break;
            }
           // _infosourcecodeid.ResetDataSource();
        }
        private void _affiliationcategorycodeid_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            _affiliationbasenamecodeid.Value = Guid.Empty;
            _affiliationbasenamecodeid.ResetDataSource();            

        }

        private void _departmentid_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            _subdepartmentid.Value = Guid.Empty;
            _subdepartmentid.ResetDataSource();

        }
        //private void _infosourcecodeid_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        //{
        //    _infosourcecodeid.Value = Guid.Empty;
        //    _infosourcecodeid.ResetDataSource();
        //}


        #region "Event handlers"

        partial void OnCreated()
		{
			this.Loaded += UMHSAffiliationEditFormUIModel_Loaded;
            this._affiliationcategorycodeid.ValueChanged += _affiliationcategorycodeid_ValueChanged;
            this._departmentid.ValueChanged += _departmentid_ValueChanged;
            //this._infosourcecodeid.ValueChanged += _infosourcecodeid_ValueChanged;
        }

#endregion

	}

}