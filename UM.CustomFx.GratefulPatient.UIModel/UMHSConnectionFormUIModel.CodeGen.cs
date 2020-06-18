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
/// Represents the UI model for the 'UMHS Connection Add Form' data form
/// </summary>
[global::Blackbaud.AppFx.UIModeling.Core.DataFormUIModelMetadata(global::Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add, "0d96a634-6f95-4317-b097-27268de0a69c", "6085707b-f469-4d8a-bbc4-f6111aa092e6", "MIMED Connection", "MIMED Data")]
public partial class @UMHSConnectionFormUIModel : global::Blackbaud.AppFx.UIModeling.Core.DataFormUIModel
{

#region "Enums"

    /// <summary>
    /// Enumerated values for use with the GENDERCODE property
    /// </summary>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public enum GENDERCODES : int
	{
        @Unknown = 0,
        @Male = 1,
        @Female = 2
	}

#endregion

#region "Extensibility methods"

	partial void OnCreated();

#endregion

    private global::Blackbaud.AppFx.UIModeling.Core.StringField _mrn;
    private global::Blackbaud.AppFx.UIModeling.Core.StringField _cpisequence;
    private global::Blackbaud.AppFx.UIModeling.Core.StringField _firstname;
    private global::Blackbaud.AppFx.UIModeling.Core.StringField _keyname;
    private global::Blackbaud.AppFx.UIModeling.Core.CodeTableField _relationshipcodeid;
    private global::Blackbaud.AppFx.UIModeling.Core.FuzzyDateField _birthdate;
    private global::Blackbaud.AppFx.UIModeling.Core.TinyIntField _age;
    private global::Blackbaud.AppFx.UIModeling.Core.ValueListField<System.Nullable<GENDERCODES>> _gendercode;
    private global::Blackbaud.AppFx.UIModeling.Core.CodeTableField _infosourcecodeid;
    private global::Blackbaud.AppFx.UIModeling.Core.StringField _comments;
   // private global::Blackbaud.AppFx.UIModeling.Core.StringField _relexists;

	[System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public @UMHSConnectionFormUIModel() : base()
	{


        _mrn = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        _cpisequence = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        _firstname = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        _keyname = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        _relationshipcodeid = new global::Blackbaud.AppFx.UIModeling.Core.CodeTableField();
        _birthdate = new global::Blackbaud.AppFx.UIModeling.Core.FuzzyDateField();
        _age = new global::Blackbaud.AppFx.UIModeling.Core.TinyIntField();
        _gendercode = new global::Blackbaud.AppFx.UIModeling.Core.ValueListField<System.Nullable<GENDERCODES>>();
        _infosourcecodeid = new global::Blackbaud.AppFx.UIModeling.Core.CodeTableField();
        _comments = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        //_relexists = new global::Blackbaud.AppFx.UIModeling.Core.StringField();

        this.Mode = global::Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add;
        this.DataFormTemplateId = new System.Guid("0d96a634-6f95-4317-b097-27268de0a69c");
        this.DataFormInstanceId = new System.Guid("6085707b-f469-4d8a-bbc4-f6111aa092e6");
        this.RecordType = "MIMED Data";
        this.ContextRecordType = "MIMED Data";
        this.FixedDialog = true;
        this.FORMHEADER.Value = "Add Patient Connection";
        this.UserInterfaceUrl = "browser/htmlforms/custom/um.customfx.gratefulpatient/UMHSConnectionForm.html";

        //
        //_mrn
        //
        _mrn.Name = "MRN";
        _mrn.Caption = "MRN";
        _mrn.Required = true;
        _mrn.MaxLength = 50;
        this.Fields.Add(_mrn);
        //
        //_cpisequence
        //
        _cpisequence.Name = "CPISEQUENCE";
        _cpisequence.Caption = "CPI sequence";
        _cpisequence.Required = true;
        _cpisequence.MaxLength = 50;
        this.Fields.Add(_cpisequence);
        //
        //_firstname
        //
        _firstname.Name = "FIRSTNAME";
        _firstname.Caption = "First name";
        _firstname.MaxLength = 50;
        this.Fields.Add(_firstname);
        //
        //_keyname
        //
        _keyname.Name = "KEYNAME";
        _keyname.Caption = "Last name";
        _keyname.MaxLength = 100;
        this.Fields.Add(_keyname);
        //
        //_relationshipcodeid
        //
        _relationshipcodeid.Name = "RELATIONSHIPCODEID";
        _relationshipcodeid.Caption = "Relationship";
        _relationshipcodeid.CodeTableName = "USR_UMHS_RELATIONSHIPCODE";
        this.Fields.Add(_relationshipcodeid);
        //
        //_birthdate
        //
        _birthdate.Name = "BIRTHDATE";
        _birthdate.Caption = "Birth date";
        _birthdate.Required = true;
        this.Fields.Add(_birthdate);
        //
        //_age
        //
        _age.Name = "AGE";
        _age.Caption = "Age";
        _age.DBReadOnly = true;
        this.Fields.Add(_age);
        //
        //_gendercode
        //
        _gendercode.Name = "GENDERCODE";
        _gendercode.Caption = "Gender";
        _gendercode.DataSource.Add(new global::Blackbaud.AppFx.UIModeling.Core.ValueListItem<System.Nullable<GENDERCODES>> {Value = GENDERCODES.@Unknown, Translation = "Unknown"});
        _gendercode.DataSource.Add(new global::Blackbaud.AppFx.UIModeling.Core.ValueListItem<System.Nullable<GENDERCODES>> {Value = GENDERCODES.@Male, Translation = "Male"});
        _gendercode.DataSource.Add(new global::Blackbaud.AppFx.UIModeling.Core.ValueListItem<System.Nullable<GENDERCODES>> {Value = GENDERCODES.@Female, Translation = "Female"});
        _gendercode.Value = GENDERCODES.@Unknown;
        this.Fields.Add(_gendercode);
        //
        //_infosourcecodeid
        //
        _infosourcecodeid.Name = "INFOSOURCECODEID";
        _infosourcecodeid.Caption = "Information source";
        _infosourcecodeid.CodeTableName = "INFOSOURCECODE";
        this.Fields.Add(_infosourcecodeid);
        //
        //_comments
        //
        _comments.Name = "COMMENTS";
        _comments.Caption = "Comments";
        this.Fields.Add(_comments);

        ////
        ////_relexists
        ////
        //_relexists.Name = "RELEXISTS";
        //_relexists.Caption = "Relationship Exists";
        //this.Fields.Add(_relexists);

        OnCreated();

	}

    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @MRN {
		get { return _mrn; }
	}

    /// <summary>
    /// CPI sequence
    /// </summary>
    [System.ComponentModel.Description("CPI sequence")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @CPISEQUENCE {
		get { return _cpisequence; }
	}

    /// <summary>
    /// First name
    /// </summary>
    [System.ComponentModel.Description("First name")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @FIRSTNAME {
		get { return _firstname; }
	}

    /// <summary>
    /// Last name
    /// </summary>
    [System.ComponentModel.Description("Last name")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @KEYNAME {
		get { return _keyname; }
	}

    /// <summary>
    /// Relationship
    /// </summary>
    [System.ComponentModel.Description("Relationship")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.CodeTableField @RELATIONSHIPCODEID {
		get { return _relationshipcodeid; }
	}

    /// <summary>
    /// Birth date
    /// </summary>
    [System.ComponentModel.Description("Birth date")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.FuzzyDateField @BIRTHDATE {
		get { return _birthdate; }
	}

    /// <summary>
    /// Age
    /// </summary>
    [System.ComponentModel.Description("Age")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.TinyIntField @AGE {
		get { return _age; }
	}

    /// <summary>
    /// Gender
    /// </summary>
    [System.ComponentModel.Description("Gender")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.ValueListField<System.Nullable<GENDERCODES>> @GENDERCODE {
		get { return _gendercode; }
	}

    /// <summary>
    /// Information source
    /// </summary>
    [System.ComponentModel.Description("Information source")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.CodeTableField @INFOSOURCECODEID {
		get { return _infosourcecodeid; }
	}

    /// <summary>
    /// Comments
    /// </summary>
    [System.ComponentModel.Description("Comments")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @COMMENTS {
		get { return _comments; }
	}

        ///// <summary>
        ///// Relexists
        ///// </summary>
        //[System.ComponentModel.Description("Relationship Exists")]
        //[System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
        //public global::Blackbaud.AppFx.UIModeling.Core.UIField @RELEXISTS
        //{
        //    get { return _relexists; }
        //}

    }


}