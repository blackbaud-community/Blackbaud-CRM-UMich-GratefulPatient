using System;
using System.Diagnostics;
using Blackbaud.AppFx.UIModeling.Core;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualBasic.CompilerServices;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class AddDataFormTemplateSpecAddDataFormUIModel
	{

		private void AddDataFormTemplateSpecAddDataFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
            this.IMPORTFILEERROR.ValueDisplayStyle = ValueDisplayStyle.WarningImageAndText;
            this.IMPORTFILEERROR.Visible = false;
            this.RECORDTYPEID.Visible = true;
            this.SaveButtonCaption = "Save Import Selection";
            this.FILENAME.Value = "File Not Specified";
            this.IDFIELDUI.Enabled = false;
            this.IDTYPECODEUI.Enabled = false;
            this.ALTERNATELOOKUPIDTYPECODEID.Visible = false;
        }

#region "Event handlers"

		partial void OnCreated()
		{
			this.Loaded += AddDataFormTemplateSpecAddDataFormUIModel_Loaded;
            this._recordtypeid.ValueChanged += _recordtypeid_ValueChanged;
            this._filename.ValueChanged += _filename_ValueChanged;
            this._idfieldui.ValueChanged += _idfieldui_ValueChanged;
            this._idtypecodeui.ValueChanged += _idtypecodeui_ValueChanged;
		}

        private void ImportSelectionAddFormUIModel_BeginValidate(object sender, BeginValidateEventArgs e)
        {
            if (!this.IMPORTFILEERROR.Visible)
                return;
            e.Valid = false;
            e.InvalidFieldName = this.FILENAME.Name;
            e.InvalidReason = this.IMPORTFILEERROR.Value;
        }
     
        private void _recordtypeid_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            this.IDTYPECODEUI.Value = (byte)0;
            this.IDTYPECODEUI.ResetDataSource();
            this.IDTYPECODEUI.Enabled = this.IDFIELDUI.Enabled && !this.RECORDTYPEID.Value.Equals(Guid.Empty);
            //this.ALTERNATELOOKUPIDTYPECODEID.Enabled = this.IDFIELDUI.Enabled && !this.RECORDTYPEID.Value.Equals(Guid.Empty);
        }
        private void _idtypecodeui_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            this.IDTYPECODE.Value = (IDTYPECODES)this.IDTYPECODEUI.Value;
           // this.ALTERNATELOOKUPIDTYPECODEID.Visible = this.IDTYPECODE.Value == IDTYPECODES.AlternateLookupID;
           // this.ALTERNATELOOKUPIDTYPECODEID.Required = this.IDTYPECODE.Value == IDTYPECODES.AlternateLookupID;
        }

        private void _idfieldui_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            this.IDFIELD.Value = this.IDFIELDUI.Value;
        }

        private void _filename_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            if (Operators.CompareString(this.FILENAME.Value, string.Empty, false) == 0)
            {
                this.FILENAME.Value = "File Not Specified";
                this.IDFIELDUI.Enabled = false;
                this.IMPORTFILENAME.Value = (string)null;
                this.IDTYPECODEUI.Enabled = false;
                this.ALTERNATELOOKUPIDTYPECODEID.Enabled = false;
                this.IMPORTFILEERROR.Visible = false;
            }
            else if (!this.FILENAME.Value.EndsWith(".csv", StringComparison.InvariantCultureIgnoreCase) && Operators.CompareString(this.FILENAME.Value, "File Not Specified", false) != 0)
            {
                this.IMPORTFILEERROR.Visible = true;
                this.IMPORTFILEERROR.Value = "The File Must be a CSV";
            }
            else
            {
                this.IMPORTFILENAME.Value = this.ModelInstanceId.ToString() + "." + this.FILE.Name + ".tmp";
                this.IDFIELDUI.Enabled = true;
                this.IDTYPECODEUI.Enabled = !this.RECORDTYPEID.Value.Equals(Guid.Empty);
                this.ALTERNATELOOKUPIDTYPECODEID.Enabled = !this.RECORDTYPEID.Value.Equals(Guid.Empty);
                this.IMPORTFILEERROR.Visible = false;
            }
            this.IDFIELDUI.Value = (string)null;
            this.IDFIELDUI.ResetDataSource();
            if (this.IDFIELDUI.DataSource != null)
                return;
            this.IMPORTFILEERROR.Visible = true;
            this.IMPORTFILEERROR.Value = "Duplicate Column Header";
        }



        #endregion

    }

}