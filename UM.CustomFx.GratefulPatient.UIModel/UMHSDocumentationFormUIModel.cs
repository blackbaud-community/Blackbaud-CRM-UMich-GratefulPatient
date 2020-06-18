using Blackbaud.AppFx.Server;
using Blackbaud.AppFx.UIModeling.Core;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class UMHSDocumentationFormUIModel
	{

		private void UMHSDocumentationFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
        {
            switch (this.Mode)
            {
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Edit:
                    this.FORMHEADER.Value = "Edit MIMED Documentation Note";
                    break;
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add:
                    this.FORMHEADER.Value = "Add MIMED Documentation Note";
                    break;
            }

            if (this.Mode == Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add)
                this.DATEENTERED.Value = DateTime.Today;
            this.NOTETYPECODEID.CodeTableName = this.FormMetaData.GetFormFieldByID("NOTETYPECODEID").CodeTableDescriptor.CodeTableName;
            CodeTablePerm[] codeTablePermArray = this.CodeTableSecurityPermissions();
            int index = 0;
            while (index < codeTablePermArray.Length)
            {
                CodeTablePerm codeTablePerm = codeTablePermArray[index];
                if (codeTablePerm.CodeTableName.Equals(this.NOTETYPECODEID.CodeTableName, StringComparison.OrdinalIgnoreCase))
                {
                    this.NOTETYPECODEID.AllowAdd = codeTablePerm.AllowAdd;
                    this.NOTETYPECODEID.AllowUpdate = codeTablePerm.AllowUpdate;
                    this.NOTETYPECODEID.AllowDelete = codeTablePerm.AllowDelete;
                }
                checked { ++index; }
            }
        }

        #region "Event handlers"

        partial void OnCreated()
		{
			this.Loaded += UMHSDocumentationFormUIModel_Loaded;
		}

#endregion

	}

}