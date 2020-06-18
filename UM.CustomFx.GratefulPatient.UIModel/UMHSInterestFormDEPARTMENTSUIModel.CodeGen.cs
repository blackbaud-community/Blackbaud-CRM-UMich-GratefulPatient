﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by BBUIModelLibrary
//     Version:  4.0.159.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace UM.CustomFx.GratefulPatient.UIModel
{

/// <summary>
/// Represents the UI model for the 'UMHSInterestFormDEPARTMENTS' data form
/// </summary>
public partial class @UMHSInterestFormDEPARTMENTSUIModel : global::Blackbaud.AppFx.UIModeling.Core.UIModel
{

#region "Extensibility methods"

	partial void OnCreated();

#endregion

    private global::Blackbaud.AppFx.UIModeling.Core.GuidField _id;
    private global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListField<System.Guid> _departmentid;
    private global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListField<System.Guid> _subdepartmentid;

	[System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public @UMHSInterestFormDEPARTMENTSUIModel() : base()
	{


        _id = new global::Blackbaud.AppFx.UIModeling.Core.GuidField();
        _departmentid = new global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListField<System.Guid>();
        _subdepartmentid = new global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListField<System.Guid>();

        //
        //_id
        //
        _id.Name = "ID";
        _id.Caption = "ID";
        _id.Visible = false;
        this.Fields.Add(_id);
        //
        //_departmentid
        //
        _departmentid.Name = "DEPARTMENTID";
        _departmentid.Caption = "MIMED department";        
        _departmentid.SimpleDataListId = new System.Guid("0cbdb7f1-f629-4e7b-a7e1-c3e4ebd7d325");
            _departmentid.Required = true;
            this.Fields.Add(_departmentid);
        //
        //_subdepartmentid
        //
        _subdepartmentid.Name = "SUBDEPARTMENTID";
        _subdepartmentid.Caption = "MIMED subdepartment";
        _subdepartmentid.SimpleDataListId = new System.Guid("879b4e21-6a71-4c60-9b41-771fa39ef9cf");
        _subdepartmentid.Parameters.Add(new global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListParameter("DEPARTMENTID", "Fields!DEPARTMENTID"));
        this.Fields.Add(_subdepartmentid);

		OnCreated();

	}

    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.GuidField @ID {
		get { return _id; }
	}

    /// <summary>
    /// UMHS Department
    /// </summary>
    [System.ComponentModel.Description("MIMED department")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListField<System.Guid> @DEPARTMENTID {
		get { return _departmentid; }
	}

    /// <summary>
    /// UMHS Subdepartment
    /// </summary>
    [System.ComponentModel.Description("MIMED subdepartment")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListField<System.Guid> @SUBDEPARTMENTID {
		get { return _subdepartmentid; }
	}

}


}
