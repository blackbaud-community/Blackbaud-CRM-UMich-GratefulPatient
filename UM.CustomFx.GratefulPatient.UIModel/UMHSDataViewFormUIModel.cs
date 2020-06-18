using Blackbaud.AppFx.UIModeling.Core;
namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class UMHSDataViewFormUIModel
	{

		private void UMHSDataViewFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
            this.UpdatePrimaryImage();

            if (_isdmineligible.Value == true)
            {
                _dmineligiblereasoncodeid.Visible = true;
            }
            else
            {
                _dmineligiblereasoncodeid.Visible = false;
            }
        }

        private void UpdatePrimaryImage()
        {
            if (this.ISDMINELIGIBLE.Value)
                // this.PRIMARYIMAGE.ValueDisplayStyle = ValueDisplayStyle.GoodImageOnly;
                this.ISDMINELIGIBLE.Value = true;
            else
                // this.PRIMARYIMAGE.ValueDisplayStyle = ValueDisplayStyle.Hidden;
                this.ISDMINELIGIBLE.Value = false;
        }

        #region "Event handlers"

        partial void OnCreated()
		{
			this.Loaded += UMHSDataViewFormUIModel_Loaded;
		}

#endregion

	}

}