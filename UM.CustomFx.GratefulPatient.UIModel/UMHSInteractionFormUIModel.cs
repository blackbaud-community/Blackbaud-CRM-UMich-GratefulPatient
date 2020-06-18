using Blackbaud.AppFx.UIModeling.Core;
using Microsoft.VisualBasic.CompilerServices;
using System;

namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class UMHSInteractionFormUIModel
	{

		private void UMHSInteractionFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
                       
            this.UpdateActualDateEnabledRequired();
            this.UpdateSubCategoryEnabledRequired();

            switch (this.Mode)
            {
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Edit:
                    this.FORMHEADER.Value = "Edit MIMED Interaction";
                    break;
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add:
                    this.FORMHEADER.Value = "Add MIMED Interaction";
                    break;
            }

        }

        private void _statuscode_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            this.UpdateActualDateEnabledRequired();
        }

        private  void _actualdate_ValueCahnged(object sender, ValueChangedEventArgs e)
        {
            this.UpdateActualDateEnabledRequired();
        }
        private void _interactioncategoryid_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (!this.Loading)
              this._interactionsubcategoryid.ValueObject = (object)Guid.Empty;
            this._interactionsubcategoryid.ResetDataSource();
            this.UpdateSubCategoryEnabledRequired();
        }

        private void UpdateActualDateEnabledRequired()
        {
            string codeTableValue = ((CodeTableField)this._statuscodeid).GetDescription();

            switch(codeTableValue)
            {
                case "Planned":                    
                    ACTUALDATE.Enabled = false;
                    ACTUALDATE.Required = false;
                    ACTUALDATE.Value = DateTime.MinValue;
                    EXPECTEDDATE.Enabled = true;
                    EXPECTEDDATE.Required = true;
                    break;
                case "Pending":
                    ACTUALDATE.Enabled = false;
                    ACTUALDATE.Required = false;
                    ACTUALDATE.Value = DateTime.MinValue;
                    EXPECTEDDATE.Enabled = true;
                    EXPECTEDDATE.Required = true;
                    break;
                case "Completed":
                    ACTUALDATE.Enabled = true;
                    ACTUALDATE.Required = true;
                    EXPECTEDDATE.Enabled = false;
                    EXPECTEDDATE.Required = false;
                    if (EXPECTEDDATE.Value == null || EXPECTEDDATE.Value == DateTime.MinValue)
                    {
                            EXPECTEDDATE.Value = ACTUALDATE.Value;// DateTime.MinValue; 
                    }
                    break;
                case "Unsuccessful":
                    ACTUALDATE.Enabled = true;
                    ACTUALDATE.Required = false;
                    EXPECTEDDATE.Enabled = false;
                    EXPECTEDDATE.Required = false;
                    if (EXPECTEDDATE.Value == null || EXPECTEDDATE.Value == DateTime.MinValue)
                    {
                            EXPECTEDDATE.Value = ACTUALDATE.Value;// DateTime.MinValue; 
                    }
                    break;
                case "Cancelled":
                    ACTUALDATE.Enabled = false;
                    ACTUALDATE.Required = false;
                    ACTUALDATE.Value = DateTime.MinValue;
                    EXPECTEDDATE.Enabled = false;
                    EXPECTEDDATE.Required = false;
                    if (EXPECTEDDATE.Value == null || EXPECTEDDATE.Value == DateTime.MinValue)
                    {
                            EXPECTEDDATE.Value = ACTUALDATE.Value;// DateTime.MinValue; 
                    }
                    break;
                case "Declined":
                    ACTUALDATE.Enabled = false;
                    ACTUALDATE.Required = false;
                    ACTUALDATE.Value = DateTime.MinValue;
                    EXPECTEDDATE.Enabled = false;
                    EXPECTEDDATE.Required = false;
                    if (EXPECTEDDATE.Value == null || EXPECTEDDATE.Value == DateTime.MinValue)
                    {
                            EXPECTEDDATE.Value = ACTUALDATE.Value;// DateTime.MinValue; 
                    }
                    break;
                default:
                    ACTUALDATE.Enabled = false;
                    ACTUALDATE.Required = false;
                    ACTUALDATE.Value = DateTime.MinValue;
                    EXPECTEDDATE.Enabled = false;
                    EXPECTEDDATE.Required = false;
                    EXPECTEDDATE.Value = DateTime.MinValue;
                    break;
            }

            
        }

        private void UpdateSubCategoryEnabledRequired()
        {
            bool flag = this._interactioncategoryid.ValueObject != null && new Guid(this._interactioncategoryid.ValueObject.ToString()) != Guid.Empty;
            this._interactionsubcategoryid.Enabled = flag;
            this._interactionsubcategoryid.Required = flag;
        }

        #region "Event handlers"

        partial void OnCreated()
		{
			this.Loaded += UMHSInteractionFormUIModel_Loaded;
            this._statuscodeid.ValueChanged += _statuscode_ValueChanged;
            this._interactioncategoryid.ValueChanged += _interactioncategoryid_ValueChanged;
            this._actualdate.ValueChanged += _actualdate_ValueCahnged;
        }

#endregion

	}

}