namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class MIMEDPortfolioReferralsDataListUIModel
	{

		private void MIMEDPortfolioReferralsDataListUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
		    this._includehistorical.Value = true;
        }

#region "Event handlers"

		partial void OnCreated()
		{
			this.Loaded += MIMEDPortfolioReferralsDataListUIModel_Loaded;
		}

#endregion

	}

}