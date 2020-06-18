﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by BBUIModelLibrary
//     Version:  4.0.165.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace UM.CustomFx.GratefulPatient.UIModel
{

/// <summary>
/// Represents the UI model for the 'UMHSInteractionFormSITES' data form
/// </summary>
public partial class @UMHSInteractionFormSITESUIModel : global::Blackbaud.AppFx.UIModeling.Core.UIModel
{

#region "Extensibility methods"

	partial void OnCreated();

#endregion

    private global::Blackbaud.AppFx.UIModeling.Core.GuidField _id;
    private global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListField<System.Guid> _siteid;
    private global::Blackbaud.AppFx.UIModeling.Core.ShowSearchFormUIAction _siteidsearchaction;

	[System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public @UMHSInteractionFormSITESUIModel() : base()
	{


        _id = new global::Blackbaud.AppFx.UIModeling.Core.GuidField();
        _siteid = new global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListField<System.Guid>();
        _siteidsearchaction = new global::Blackbaud.AppFx.UIModeling.Core.ShowSearchFormUIAction();

        //
        //_id
        //
        _id.Name = "ID";
        _id.Caption = "ID";
        _id.Visible = false;
        this.Fields.Add(_id);
        //
        //_siteid
        //
        _siteid.Name = "SITEID";
        _siteid.Caption = "Site";
        _siteid.Required = true;
        _siteid.SimpleDataListId = new System.Guid("c8e8d3ba-2725-421f-a796-e2fcc1202780");
        _siteid.LinkedActions.Add(_siteidsearchaction);
        this.Fields.Add(_siteid);
        //
        //_siteidsearchaction
        //
        _siteidsearchaction.Name = "SITEIDSEARCHACTION";
        _siteidsearchaction.Caption = "Search";
        _siteidsearchaction.SearchListId = new System.Guid("27a63ede-a0d4-4074-a85a-196df4adc0dd");
        this.Actions.Add(_siteidsearchaction);

		OnCreated();

	}

    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.GuidField @ID {
		get { return _id; }
	}

    /// <summary>
    /// Site
    /// </summary>
    [System.ComponentModel.Description("Site")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListField<System.Guid> @SITEID {
		get { return _siteid; }
	}

    /// <summary>
    /// Search
    /// </summary>
    [System.ComponentModel.Description("Search")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.ShowSearchFormUIAction @SITEIDSEARCHACTION {
		get { return _siteidsearchaction; }
	}

}


}
