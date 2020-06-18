﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by BBUIModelLibrary
//     Version:  4.0.163.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace UM.CustomFx.GratefulPatient.UIModel
{

/// <summary>
/// Represents the UI model for the 'UMHS Relationship Edit Form' data form
/// </summary>
[global::Blackbaud.AppFx.UIModeling.Core.DataFormUIModelMetadata(global::Blackbaud.AppFx.UIModeling.Core.DataFormMode.Edit, "9ae6c14d-4d35-4fb4-a25d-820e0719b243", "204f8d3a-2e0b-47ca-bc7e-3e10cdf41d49", "MIMED Relationship")]
public partial class @UMHSRelationshipEditFormUIModel : global::Blackbaud.AppFx.UIModeling.Core.DataFormUIModel
{

#region "Extensibility methods"

	partial void OnCreated();

#endregion

    private global::Blackbaud.AppFx.UIModeling.Core.StringField _patientname;
    private global::Blackbaud.AppFx.UIModeling.Core.StringField _relatedindividual;
    private global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListField<System.Guid> _relationshiptypecodeid;
    private global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListField<System.Guid> _reciprocaltypecodeid;
    private global::Blackbaud.AppFx.UIModeling.Core.DateField _startdate;
    private global::Blackbaud.AppFx.UIModeling.Core.DateField _enddate;
    private global::Blackbaud.AppFx.UIModeling.Core.CodeTableField _contacttypecodeid;
    private global::Blackbaud.AppFx.UIModeling.Core.GuidField _relationshipsetid;
    private global::Blackbaud.AppFx.UIModeling.Core.BooleanField _isspouse;
    private global::Blackbaud.AppFx.UIModeling.Core.BooleanField _isprimarybusiness;
    private global::Blackbaud.AppFx.UIModeling.Core.BooleanField _iscontact;
    private global::Blackbaud.AppFx.UIModeling.Core.BooleanField _isprimarycontact;
    private global::Blackbaud.AppFx.UIModeling.Core.BooleanField _ismatchinggiftrelationship;
    private global::Blackbaud.AppFx.UIModeling.Core.BooleanField _isemergencycontact;
    private global::Blackbaud.AppFx.UIModeling.Core.BooleanField _receivesreportcard;
    private global::Blackbaud.AppFx.UIModeling.Core.BooleanField _isguarantor;
    private global::Blackbaud.AppFx.UIModeling.Core.StringField _comments;
    private global::Blackbaud.AppFx.UIModeling.Core.GroupField _relationshipgroup;
    private global::Blackbaud.AppFx.UIModeling.Core.GroupField _commentsgroup;

	[System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public @UMHSRelationshipEditFormUIModel() : base()
	{


        _patientname = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        _relatedindividual = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        _relationshiptypecodeid = new global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListField<System.Guid>();
        _reciprocaltypecodeid = new global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListField<System.Guid>();
        _startdate = new global::Blackbaud.AppFx.UIModeling.Core.DateField();
        _enddate = new global::Blackbaud.AppFx.UIModeling.Core.DateField();
        _contacttypecodeid = new global::Blackbaud.AppFx.UIModeling.Core.CodeTableField();
        _relationshipsetid = new global::Blackbaud.AppFx.UIModeling.Core.GuidField();
        _isspouse = new global::Blackbaud.AppFx.UIModeling.Core.BooleanField();
        _isprimarybusiness = new global::Blackbaud.AppFx.UIModeling.Core.BooleanField();
        _iscontact = new global::Blackbaud.AppFx.UIModeling.Core.BooleanField();
        _isprimarycontact = new global::Blackbaud.AppFx.UIModeling.Core.BooleanField();
        _ismatchinggiftrelationship = new global::Blackbaud.AppFx.UIModeling.Core.BooleanField();
        _isemergencycontact = new global::Blackbaud.AppFx.UIModeling.Core.BooleanField();
        _receivesreportcard = new global::Blackbaud.AppFx.UIModeling.Core.BooleanField();
        _isguarantor = new global::Blackbaud.AppFx.UIModeling.Core.BooleanField();
        _comments = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        _relationshipgroup = new global::Blackbaud.AppFx.UIModeling.Core.GroupField();
        _commentsgroup = new global::Blackbaud.AppFx.UIModeling.Core.GroupField();

        this.Mode = global::Blackbaud.AppFx.UIModeling.Core.DataFormMode.Edit;
        this.DataFormTemplateId = new System.Guid("9ae6c14d-4d35-4fb4-a25d-820e0719b243");
        this.DataFormInstanceId = new System.Guid("204f8d3a-2e0b-47ca-bc7e-3e10cdf41d49");
        this.RecordType = "MIMED Data";
        this.FixedDialog = true;
        this.UserInterfaceUrl = "browser/htmlforms/UMHSRelationshipEditForm.html";

        //
        //_patientname
        //
        _patientname.Name = "PATIENTNAME";
        _patientname.Caption = "Patient name";
        _patientname.Visible = false;
        _patientname.DBReadOnly = true;
        _patientname.MaxLength = 100;
        this.Fields.Add(_patientname);
        //
        //_relatedindividual
        //
        _relatedindividual.Name = "RELATEDINDIVIDUAL";
        _relatedindividual.Caption = "Related individual";
        _relatedindividual.DBReadOnly = true;
        _relatedindividual.MaxLength = 100;
        this.Fields.Add(_relatedindividual);
        //
        //_relationshiptypecodeid
        //
        _relationshiptypecodeid.Name = "RELATIONSHIPTYPECODEID";
        _relationshiptypecodeid.Caption = "Relationship type";
        _relationshiptypecodeid.Required = true;
        _relationshiptypecodeid.SimpleDataListId = new System.Guid("4e869c5a-9b9d-4e84-b6e0-1fc66bafbafc");
        _relationshiptypecodeid.Parameters.Add(new global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListParameter("CONSTITUENTID", "Form!ContextID"));
        this.Fields.Add(_relationshiptypecodeid);
        //
        //_reciprocaltypecodeid
        //
        _reciprocaltypecodeid.Name = "RECIPROCALTYPECODEID";
        _reciprocaltypecodeid.Caption = "Individual is the";
        _reciprocaltypecodeid.Required = true;
        _reciprocaltypecodeid.SimpleDataListId = new System.Guid("c3018803-2ea5-4f62-91cf-412e88d15f9b");
        _reciprocaltypecodeid.Parameters.Add(new global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListParameter("APPLIESTOCONSTITUENTTYPE", "Fields!CONSTITUENTTYPE"));
        _reciprocaltypecodeid.Parameters.Add(new global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListParameter("CONSTITUENTID", "Fields!RECIPROCALCONSTITUENTID"));
        _reciprocaltypecodeid.Parameters.Add(new global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListParameter("APPLIESTORELATIONSHIPTYPEID", "Fields!RELATIONSHIPTYPECODEID"));
        this.Fields.Add(_reciprocaltypecodeid);
        //
        //_startdate
        //
        _startdate.Name = "STARTDATE";
        _startdate.Caption = "Start date";
        this.Fields.Add(_startdate);
        //
        //_enddate
        //
        _enddate.Name = "ENDDATE";
        _enddate.Caption = "End date";
        this.Fields.Add(_enddate);
        //
        //_contacttypecodeid
        //
        _contacttypecodeid.Name = "CONTACTTYPECODEID";
        _contacttypecodeid.Caption = "Contact type";
        _contacttypecodeid.CodeTableName = "CONTACTTYPECODE";
        this.Fields.Add(_contacttypecodeid);
        //
        //_relationshipsetid
        //
        _relationshipsetid.Name = "RELATIONSHIPSETID";
        _relationshipsetid.Caption = "Relationship set";
        _relationshipsetid.Visible = false;
        this.Fields.Add(_relationshipsetid);
        //
        //_isspouse
        //
        _isspouse.Name = "ISSPOUSE";
        _isspouse.Caption = "Spouse";
        this.Fields.Add(_isspouse);
        //
        //_isprimarybusiness
        //
        _isprimarybusiness.Name = "ISPRIMARYBUSINESS";
        _isprimarybusiness.Caption = "Primary business";
        this.Fields.Add(_isprimarybusiness);
        //
        //_iscontact
        //
        _iscontact.Name = "ISCONTACT";
        _iscontact.Caption = "Contact";
        this.Fields.Add(_iscontact);
        //
        //_isprimarycontact
        //
        _isprimarycontact.Name = "ISPRIMARYCONTACT";
        _isprimarycontact.Caption = "Primary contact";
        this.Fields.Add(_isprimarycontact);
        //
        //_ismatchinggiftrelationship
        //
        _ismatchinggiftrelationship.Name = "ISMATCHINGGIFTRELATIONSHIP";
        _ismatchinggiftrelationship.Caption = "Matching gift relationship";
        this.Fields.Add(_ismatchinggiftrelationship);
        //
        //_isemergencycontact
        //
        _isemergencycontact.Name = "ISEMERGENCYCONTACT";
        _isemergencycontact.Caption = "Emergency contact";
        this.Fields.Add(_isemergencycontact);
        //
        //_receivesreportcard
        //
        _receivesreportcard.Name = "RECEIVESREPORTCARD";
        _receivesreportcard.Caption = "Receives report card";
        this.Fields.Add(_receivesreportcard);
        //
        //_isguarantor
        //
        _isguarantor.Name = "ISGUARANTOR";
        _isguarantor.Caption = "Is guarantor";
        this.Fields.Add(_isguarantor);
        //
        //_comments
        //
        _comments.Name = "COMMENTS";
        _comments.Caption = "Comments";
        this.Fields.Add(_comments);
        //
        //_relationshipgroup
        //
        _relationshipgroup.Name = "RELATIONSHIPGROUP";
        _relationshipgroup.Caption = "Relationship";
        _relationshipgroup.Fields.Add("RELATEDINDIVIDUAL");
        _relationshipgroup.Fields.Add("RELATIONSHIPTYPECODEID");
        _relationshipgroup.Fields.Add("RECIPROCALTYPECODEID");
        _relationshipgroup.Fields.Add("STARTDATE");
        _relationshipgroup.Fields.Add("ENDDATE");
        _relationshipgroup.Fields.Add("CONTACTTYPECODEID");
        _relationshipgroup.Fields.Add("RELATIONSHIPSETID");
        this.Fields.Add(_relationshipgroup);
        //
        //_commentsgroup
        //
        _commentsgroup.Name = "COMMENTSGROUP";
        _commentsgroup.Caption = "Comments";
        _commentsgroup.Fields.Add("COMMENTS");
        _commentsgroup.Fields.Add("ISSPOUSE");
        _commentsgroup.Fields.Add("ISPRIMARYBUSINESS");
        _commentsgroup.Fields.Add("ISCONTACT");
        _commentsgroup.Fields.Add("ISPRIMARYCONTACT");
        _commentsgroup.Fields.Add("ISMATCHINGGIFTRELATIONSHIP");
        _commentsgroup.Fields.Add("ISEMERGENCYCONTACT");
        _commentsgroup.Fields.Add("RECEIVESREPORTCARD");
        _commentsgroup.Fields.Add("ISGUARANTOR");
        this.Fields.Add(_commentsgroup);

		OnCreated();

	}

    /// <summary>
    /// Patient name
    /// </summary>
    [System.ComponentModel.Description("Patient name")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @PATIENTNAME {
		get { return _patientname; }
	}

    /// <summary>
    /// Related individual
    /// </summary>
    [System.ComponentModel.Description("Related individual")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @RELATEDINDIVIDUAL {
		get { return _relatedindividual; }
	}

    /// <summary>
    /// Relationship type
    /// </summary>
    [System.ComponentModel.Description("Relationship type")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListField<System.Guid> @RELATIONSHIPTYPECODEID {
		get { return _relationshiptypecodeid; }
	}

    /// <summary>
    /// Reciprocal  type
    /// </summary>
    [System.ComponentModel.Description("Reciprocal  type")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.SimpleDataListField<System.Guid> @RECIPROCALTYPECODEID {
		get { return _reciprocaltypecodeid; }
	}

    /// <summary>
    /// Start date
    /// </summary>
    [System.ComponentModel.Description("Start date")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.DateField @STARTDATE {
		get { return _startdate; }
	}

    /// <summary>
    /// End date
    /// </summary>
    [System.ComponentModel.Description("End date")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.DateField @ENDDATE {
		get { return _enddate; }
	}

    /// <summary>
    /// Contact type
    /// </summary>
    [System.ComponentModel.Description("Contact type")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.CodeTableField @CONTACTTYPECODEID {
		get { return _contacttypecodeid; }
	}

    /// <summary>
    /// Relationship set
    /// </summary>
    [System.ComponentModel.Description("Relationship set")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.GuidField @RELATIONSHIPSETID {
		get { return _relationshipsetid; }
	}

    /// <summary>
    /// Spouse
    /// </summary>
    [System.ComponentModel.Description("Spouse")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.BooleanField @ISSPOUSE {
		get { return _isspouse; }
	}

    /// <summary>
    /// Primary business
    /// </summary>
    [System.ComponentModel.Description("Primary business")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.BooleanField @ISPRIMARYBUSINESS {
		get { return _isprimarybusiness; }
	}

    /// <summary>
    /// Contact
    /// </summary>
    [System.ComponentModel.Description("Contact")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.BooleanField @ISCONTACT {
		get { return _iscontact; }
	}

    /// <summary>
    /// Primary contact
    /// </summary>
    [System.ComponentModel.Description("Primary contact")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.BooleanField @ISPRIMARYCONTACT {
		get { return _isprimarycontact; }
	}

    /// <summary>
    /// Matching gift relationship
    /// </summary>
    [System.ComponentModel.Description("Matching gift relationship")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.BooleanField @ISMATCHINGGIFTRELATIONSHIP {
		get { return _ismatchinggiftrelationship; }
	}

    /// <summary>
    /// Emergency contact
    /// </summary>
    [System.ComponentModel.Description("Emergency contact")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.BooleanField @ISEMERGENCYCONTACT {
		get { return _isemergencycontact; }
	}

    /// <summary>
    /// Receives report card
    /// </summary>
    [System.ComponentModel.Description("Receives report card")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.BooleanField @RECEIVESREPORTCARD {
		get { return _receivesreportcard; }
	}

    /// <summary>
    /// Is guarantor
    /// </summary>
    [System.ComponentModel.Description("Is guarantor")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.BooleanField @ISGUARANTOR {
		get { return _isguarantor; }
	}

    /// <summary>
    /// Comments
    /// </summary>
    [System.ComponentModel.Description("Comments")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @COMMENTS {
		get { return _comments; }
	}

    /// <summary>
    /// Relationship
    /// </summary>
    [System.ComponentModel.Description("Relationship")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.GroupField @RELATIONSHIPGROUP {
		get { return _relationshipgroup; }
	}

    /// <summary>
    /// Comments
    /// </summary>
    [System.ComponentModel.Description("Comments")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.163.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.GroupField @COMMENTSGROUP {
		get { return _commentsgroup; }
	}

}


}
