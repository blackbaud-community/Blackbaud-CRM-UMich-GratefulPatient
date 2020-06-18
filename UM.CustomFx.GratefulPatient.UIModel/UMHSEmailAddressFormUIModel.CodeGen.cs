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
/// Represents the UI model for the 'UMHS Email Address Add Form' data form
/// </summary>
[global::Blackbaud.AppFx.UIModeling.Core.DataFormUIModelMetadata(global::Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add, "95c3f69c-9adf-4b45-9b4c-4861329a53d8", "3e721531-9f45-4351-bcc3-9f7226551251", "MIMED Email Address", "MIMED Data")]
public partial class @UMHSEmailAddressFormUIModel : global::Blackbaud.AppFx.UIModeling.Core.DataFormUIModel
{

#region "Extensibility methods"

	partial void OnCreated();

#endregion

    private global::Blackbaud.AppFx.UIModeling.Core.CodeTableField _emailaddresstypecodeid;
    private global::Blackbaud.AppFx.UIModeling.Core.EmailAddressField _emailaddress;
    private global::Blackbaud.AppFx.UIModeling.Core.BooleanField _isprimary;
    private global::Blackbaud.AppFx.UIModeling.Core.BooleanField _donotemail;
    private global::Blackbaud.AppFx.UIModeling.Core.CodeTableField _infosourcecodeid;
    private global::Blackbaud.AppFx.UIModeling.Core.StringField _infosourcecomments;
    private global::Blackbaud.AppFx.UIModeling.Core.DateField _startdate;
    private global::Blackbaud.AppFx.UIModeling.Core.DateField _enddate;
    private global::Blackbaud.AppFx.UIModeling.Core.BooleanField _isconfidential;
    private global::Blackbaud.AppFx.UIModeling.Core.StringField _invalidaccount;
    private global::Blackbaud.AppFx.UIModeling.Core.StringField _historicalstartdatemessage;
    private global::Blackbaud.AppFx.UIModeling.Core.BooleanField _showinvalid;
    private global::Blackbaud.AppFx.UIModeling.Core.StringField _invalidfield1;
    private global::Blackbaud.AppFx.UIModeling.Core.StringField _invalidfield2;
    private global::Blackbaud.AppFx.UIModeling.Core.StringField _origincodetranslation;
    private global::Blackbaud.AppFx.UIModeling.Core.GroupField _sourceinformation;

	[System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public @UMHSEmailAddressFormUIModel() : base()
	{


        _emailaddresstypecodeid = new global::Blackbaud.AppFx.UIModeling.Core.CodeTableField();
        _emailaddress = new global::Blackbaud.AppFx.UIModeling.Core.EmailAddressField();
        _isprimary = new global::Blackbaud.AppFx.UIModeling.Core.BooleanField();
        _donotemail = new global::Blackbaud.AppFx.UIModeling.Core.BooleanField();
        _infosourcecodeid = new global::Blackbaud.AppFx.UIModeling.Core.CodeTableField();
        _infosourcecomments = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        _startdate = new global::Blackbaud.AppFx.UIModeling.Core.DateField();
        _enddate = new global::Blackbaud.AppFx.UIModeling.Core.DateField();
        _isconfidential = new global::Blackbaud.AppFx.UIModeling.Core.BooleanField();
        _invalidaccount = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        _historicalstartdatemessage = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        _showinvalid = new global::Blackbaud.AppFx.UIModeling.Core.BooleanField();
        _invalidfield1 = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        _invalidfield2 = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        _origincodetranslation = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        _sourceinformation = new global::Blackbaud.AppFx.UIModeling.Core.GroupField();

        this.Mode = global::Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add;
        this.DataFormTemplateId = new System.Guid("95c3f69c-9adf-4b45-9b4c-4861329a53d8");
        this.DataFormInstanceId = new System.Guid("3e721531-9f45-4351-bcc3-9f7226551251");
        this.RecordType = "MIMED Email Address";
        this.ContextRecordType = "MIMED Data";
        this.FixedDialog = true;
        this.FORMHEADER.Value = "Add MIMED Email Address";
        this.UserInterfaceUrl = "browser/htmlforms/custom/um.customfx.gratefulpatient/UMHSEmailAddressForm.html";

        //
        //_emailaddresstypecodeid
        //
        _emailaddresstypecodeid.Name = "EMAILADDRESSTYPECODEID";
        _emailaddresstypecodeid.Caption = "Email address type";
        _emailaddresstypecodeid.CodeTableName = "EMAILADDRESSTYPECODE";
        this.Fields.Add(_emailaddresstypecodeid);
        //
        //_emailaddress
        //
        _emailaddress.Name = "EMAILADDRESS";
        _emailaddress.Caption = "Email address";
        _emailaddress.Required = true;
        this.Fields.Add(_emailaddress);
        //
        //_isprimary
        //
        _isprimary.Name = "ISPRIMARY";
        _isprimary.Caption = "Set as primary email address";
        this.Fields.Add(_isprimary);
        //
        //_donotemail
        //
        _donotemail.Name = "DONOTEMAIL";
        _donotemail.Caption = "Do not send email to this address";
        this.Fields.Add(_donotemail);
        //
        //_infosourcecodeid
        //
        _infosourcecodeid.Name = "INFOSOURCECODEID";
        _infosourcecodeid.Caption = "Information source";
        _infosourcecodeid.CodeTableName = "INFOSOURCECODE";
        this.Fields.Add(_infosourcecodeid);
        //
        //_infosourcecomments
        //
        _infosourcecomments.Name = "INFOSOURCECOMMENTS";
        _infosourcecomments.Caption = "Comments";
        _infosourcecomments.MaxLength = 256;
        _infosourcecomments.Multiline = true;
        this.Fields.Add(_infosourcecomments);
        //
        //_startdate
        //
        _startdate.Name = "STARTDATE";
        _startdate.Caption = "Start date";
        _startdate.Value = System.DateTime.Today;
        this.Fields.Add(_startdate);
        //
        //_enddate
        //
        _enddate.Name = "ENDDATE";
        _enddate.Caption = "End date";
        this.Fields.Add(_enddate);
        //
        //_isconfidential
        //
        _isconfidential.Name = "ISCONFIDENTIAL";
        _isconfidential.Caption = "Is confidential";
        this.Fields.Add(_isconfidential);
        //
        //_invalidaccount
        //
        _invalidaccount.Name = "INVALIDACCOUNT";
        _invalidaccount.Caption = "INVALIDACCOUNT";
        _invalidaccount.DBReadOnly = true;
        this.Fields.Add(_invalidaccount);
        //
        //_historicalstartdatemessage
        //
        _historicalstartdatemessage.Name = "HISTORICALSTARTDATEMESSAGE";
        _historicalstartdatemessage.Caption = "HISTORICALSTARTDATEMESSAGE";
        _historicalstartdatemessage.DBReadOnly = true;
        this.Fields.Add(_historicalstartdatemessage);
        //
        //_showinvalid
        //
        _showinvalid.Name = "SHOWINVALID";
        _showinvalid.Caption = "SHOWINVALID";
        _showinvalid.Visible = false;
        _showinvalid.DBReadOnly = true;
        this.Fields.Add(_showinvalid);
        //
        //_invalidfield1
        //
        _invalidfield1.Name = "INVALIDFIELD1";
        _invalidfield1.Caption = "INVALIDFIELD1";
        _invalidfield1.Visible = false;
        _invalidfield1.DBReadOnly = true;
        this.Fields.Add(_invalidfield1);
        //
        //_invalidfield2
        //
        _invalidfield2.Name = "INVALIDFIELD2";
        _invalidfield2.Caption = "INVALIDFIELD2";
        _invalidfield2.Visible = false;
        _invalidfield2.DBReadOnly = true;
        this.Fields.Add(_invalidfield2);
        //
        //_origincodetranslation
        //
        _origincodetranslation.Name = "ORIGINCODETRANSLATION";
        _origincodetranslation.Caption = "Information source";
        _origincodetranslation.DBReadOnly = true;
        this.Fields.Add(_origincodetranslation);
        //
        //_sourceinformation
        //
        _sourceinformation.Name = "SOURCEINFORMATION";
        _sourceinformation.Caption = "Email source";
        _sourceinformation.Fields.Add("INFOSOURCECODEID");
        _sourceinformation.Fields.Add("INFOSOURCECOMMENTS");
        this.Fields.Add(_sourceinformation);

		OnCreated();

	}

    /// <summary>
    /// Email address type
    /// </summary>
    [System.ComponentModel.Description("Email address type")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.CodeTableField @EMAILADDRESSTYPECODEID {
		get { return _emailaddresstypecodeid; }
	}

    /// <summary>
    /// Email address
    /// </summary>
    [System.ComponentModel.Description("Email address")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.EmailAddressField @EMAILADDRESS {
		get { return _emailaddress; }
	}

    /// <summary>
    /// Is primary
    /// </summary>
    [System.ComponentModel.Description("Is primary")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.BooleanField @ISPRIMARY {
		get { return _isprimary; }
	}

    /// <summary>
    /// Do not email
    /// </summary>
    [System.ComponentModel.Description("Do not email")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.BooleanField @DONOTEMAIL {
		get { return _donotemail; }
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
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @INFOSOURCECOMMENTS {
		get { return _infosourcecomments; }
	}

    /// <summary>
    /// Start date
    /// </summary>
    [System.ComponentModel.Description("Start date")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.DateField @STARTDATE {
		get { return _startdate; }
	}

    /// <summary>
    /// End date
    /// </summary>
    [System.ComponentModel.Description("End date")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.DateField @ENDDATE {
		get { return _enddate; }
	}

    /// <summary>
    /// Is confidential
    /// </summary>
    [System.ComponentModel.Description("Is confidential")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.BooleanField @ISCONFIDENTIAL {
		get { return _isconfidential; }
	}

    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @INVALIDACCOUNT {
		get { return _invalidaccount; }
	}

    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @HISTORICALSTARTDATEMESSAGE {
		get { return _historicalstartdatemessage; }
	}

    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.BooleanField @SHOWINVALID {
		get { return _showinvalid; }
	}

    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @INVALIDFIELD1 {
		get { return _invalidfield1; }
	}

    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @INVALIDFIELD2 {
		get { return _invalidfield2; }
	}

    /// <summary>
    /// Information source
    /// </summary>
    [System.ComponentModel.Description("Information source")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @ORIGINCODETRANSLATION {
		get { return _origincodetranslation; }
	}

    /// <summary>
    /// Email source
    /// </summary>
    [System.ComponentModel.Description("Email source")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.159.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.GroupField @SOURCEINFORMATION {
		get { return _sourceinformation; }
	}

}


}