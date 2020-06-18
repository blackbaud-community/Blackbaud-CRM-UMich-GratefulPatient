using Blackbaud.AppFx.UIModeling.Core;
using Blackbaud.AppFx.UIModeling.Core.Utilities;
using Blackbaud.AppFx.XmlTypes.DataForms;

namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class MIMEDLoggingReportReportUIModel
	{

		private void MIMEDLoggingReportReportUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
		    
        }

	    public override DataFormItem ToDataFormItem(bool includeDBReadOnlyFields)
	    {
	        DataFormItem dataFormItem = base.ToDataFormItem(includeDBReadOnlyFields);
	        dataFormItem.SetValue(this.DATERAGESTART.Name, (object)this.DateRangeHandler.FromDateValue);
	        dataFormItem.SetValue(this.DATERAGEEND.Name, (object)this.DateRangeHandler.ToDateValue);
	        dataFormItem.SetValue(this.DATERANGEDISPLAY.Name, (object)this.DateRangeHandler.DateRangeDisplay);
	        return dataFormItem;

        }


        #region "Event handlers"

        partial void OnCreated()
		{
			this.Loaded += MIMEDLoggingReportReportUIModel_Loaded;
		    this.DateRangeHandler = new ReportDateRangeHandler((RootUIModel)this, (ValueListField)this.DATETYPE, this.DATERAGESTART, this.DATERAGEEND, true);

        }

        #endregion

    }

}