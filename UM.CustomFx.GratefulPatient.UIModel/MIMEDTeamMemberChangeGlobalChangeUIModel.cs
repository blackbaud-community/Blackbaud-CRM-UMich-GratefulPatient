namespace UM.CustomFx.GratefulPatient.UIModel
{
	public partial class MIMEDTeamMemberChangeGlobalChangeUIModel
	{
	    private void CURRENTTEAMMEMBERID_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
	    {
            MIMEDTeamMemberGlobalChange.AutoPopulateTeamMemberSite(this.GetRequestContext().AppDBConnectionString(),
	            this.CURRENTTEAMMEMBERID.Value, _currentteammembersiteid);
	    }

	    private void NEWTEAMMEMBERID_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
	    {
	        MIMEDTeamMemberGlobalChange.AutoPopulateTeamMemberSite(this.GetRequestContext().AppDBConnectionString(),
	            this.NEWTEAMMEMBERID.Value, _newteammembersiteid);
        }
        
	    #region "Event handlers"

        partial void OnCreated()
		{
			this.CURRENTTEAMMEMBERID.ValueChanged += CURRENTTEAMMEMBERID_ValueChanged;
		    this.NEWTEAMMEMBERID.ValueChanged += NEWTEAMMEMBERID_ValueChanged;
        }

        #endregion
	}
}