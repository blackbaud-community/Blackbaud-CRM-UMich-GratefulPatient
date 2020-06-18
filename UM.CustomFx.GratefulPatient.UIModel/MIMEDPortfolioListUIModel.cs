namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class MIMEDPortfolioListUIModel
	{

		private void MIMEDPortfolioListUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
		    this._includehistorical.Value = true;
        }
	    

#region "Event handlers"

        partial void OnCreated()
		{
			this.Loaded += MIMEDPortfolioListUIModel_Loaded;
		    //this.INCLUDEHISTORICAL.ValueChanged += INCLUDEHISTORICAL_ValueChanged;
		}

#endregion

	}

}