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
/// Represents the UI model for the 'UMHS Interaction Copy to DART' data form
/// </summary>
[global::Blackbaud.AppFx.UIModeling.Core.DataFormUIModelMetadata(global::Blackbaud.AppFx.UIModeling.Core.DataFormMode.Edit, "85021f57-46f4-4198-880d-5af69efae00f", "459f298b-b383-451a-8dbe-fbc2cf26ebda", "MIMED Interaction")]
public partial class @UMHSInteractionCopyUIModel : global::Blackbaud.AppFx.UIModeling.Core.DataFormUIModel
{

#region "Extensibility methods"

	partial void OnCreated();

#endregion

    private global::Blackbaud.AppFx.UIModeling.Core.CollectionField<UMHSInteractionCopyINTERACTIONSUIModel> _interactions;
    private global::Blackbaud.AppFx.UIModeling.Core.GenericUIAction _selectall;

	[System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public @UMHSInteractionCopyUIModel() : base()
	{


        _interactions = new global::Blackbaud.AppFx.UIModeling.Core.CollectionField<UMHSInteractionCopyINTERACTIONSUIModel>();
        _selectall = new global::Blackbaud.AppFx.UIModeling.Core.GenericUIAction();

        this.Mode = global::Blackbaud.AppFx.UIModeling.Core.DataFormMode.Edit;
        this.DataFormTemplateId = new System.Guid("85021f57-46f4-4198-880d-5af69efae00f");
        this.DataFormInstanceId = new System.Guid("459f298b-b383-451a-8dbe-fbc2cf26ebda");
        this.RecordType = "MIMED Interaction";
        this.FORMHEADER.Value = "Copy MIMED Interactions";
        this.UserInterfaceUrl = "browser/htmlforms/custom/um.customfx.gratefulpatient/UMHSInteractionCopy.html";

        //
        //_interactions
        //
        _interactions.Name = "INTERACTIONS";
        _interactions.Caption = "Interactions";
        this.Fields.Add(_interactions);
        //
        //_selectall
        //
        _selectall.Name = "SELECTALL";
        _selectall.Caption = "Select All";
        this.Actions.Add(_selectall);

		OnCreated();

	}

    /// <summary>
    /// Interactions
    /// </summary>
    [System.ComponentModel.Description("Interactions")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.CollectionField<UMHSInteractionCopyINTERACTIONSUIModel> @INTERACTIONS {
		get { return _interactions; }
	}

    /// <summary>
    /// Select All
    /// </summary>
    [System.ComponentModel.Description("Select All")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.GenericUIAction @SELECTALL {
		get { return _selectall; }
	}

}


}
