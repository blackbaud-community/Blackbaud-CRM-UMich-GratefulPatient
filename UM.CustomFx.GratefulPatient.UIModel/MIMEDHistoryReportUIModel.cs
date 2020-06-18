using Blackbaud.AppFx.UIModeling.Core;
using Blackbaud.AppFx.UIModeling.Core.Utilities;
using Blackbaud.AppFx.XmlTypes.DataForms;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class MIMEDHistoryReportUIModel
	{

		private void MIMEDHistoryReportUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{

		}
        public override DataFormItem ToDataFormItem(bool includeDBReadOnlyFields)
        {
            DataFormItem dataFormItem = base.ToDataFormItem(includeDBReadOnlyFields);
            dataFormItem.SetValue(this.STARTDATE.Name, (object)this.DateRangeHandler.FromDateValue);
            dataFormItem.SetValue(this.ENDDATE.Name, (object)this.DateRangeHandler.ToDateValue);
            dataFormItem.SetValue(this.DATERANGEDISPLAY.Name, (object)this.DateRangeHandler.DateRangeDisplay);
            return dataFormItem;
        }     
        
    #region "Event handlers"

    partial void OnCreated()
		{
			this.Loaded += MIMEDHistoryReportUIModel_Loaded;
            this.DateRangeHandler = new ReportDateRangeHandler((RootUIModel)this, (ValueListField)this.DATETYPE, this.STARTDATE, this.ENDDATE, true);
        }

        #endregion

    }

}