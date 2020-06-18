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
/// Represents the UI model for the 'Revenue Batch UMHS Duplicate Constituent Search' data form
/// </summary>
[global::Blackbaud.AppFx.UIModeling.Core.SearchListUIModelMetadata("60dca9c9-9c1a-4f50-b255-ef0eb1414779", "Revenue", "DATABatchDedupe.html")]
public partial class @UMHSDuplicateConstituentSearchUIModel : global::Blackbaud.AppFx.UIModeling.Core.SearchListUIModel
{

#region "Extensibility methods"

	partial void OnCreated();

#endregion

    private global::Blackbaud.AppFx.UIModeling.Core.StringField _editconstituentcontext;
    private global::Blackbaud.AppFx.UIModeling.Core.GuidField _constituentid;
    private global::Blackbaud.AppFx.UIModeling.Core.SearchListField<System.Guid> _constituentlookupid;
    private global::Blackbaud.AppFx.UIModeling.Core.CollectionField<UMHSDuplicateConstituentSearchNEWCONSTITUENTUIModel> _newconstituent;
    private global::Blackbaud.AppFx.UIModeling.Core.DecimalField _overallmatchthreshold;
    private global::Blackbaud.AppFx.UIModeling.Core.DecimalField _automatchthreshold;
    private global::Blackbaud.AppFx.UIModeling.Core.GuidField _batchid;
    private global::Blackbaud.AppFx.UIModeling.Core.StringField _constituentname;
    private global::Blackbaud.AppFx.UIModeling.Core.StringField _constituentaddress;
    private global::Blackbaud.AppFx.UIModeling.Core.BooleanField _gotonextrecord;
    private global::Blackbaud.AppFx.UIModeling.Core.GenericUIAction _save;
    private global::Blackbaud.AppFx.UIModeling.Core.GenericUIAction _cancel;

	[System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public @UMHSDuplicateConstituentSearchUIModel() : base()
	{


        _editconstituentcontext = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        _constituentid = new global::Blackbaud.AppFx.UIModeling.Core.GuidField();
        _constituentlookupid = new global::Blackbaud.AppFx.UIModeling.Core.SearchListField<System.Guid>();
        _newconstituent = new global::Blackbaud.AppFx.UIModeling.Core.CollectionField<UMHSDuplicateConstituentSearchNEWCONSTITUENTUIModel>();
        _overallmatchthreshold = new global::Blackbaud.AppFx.UIModeling.Core.DecimalField();
        _automatchthreshold = new global::Blackbaud.AppFx.UIModeling.Core.DecimalField();
        _batchid = new global::Blackbaud.AppFx.UIModeling.Core.GuidField();
        _constituentname = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        _constituentaddress = new global::Blackbaud.AppFx.UIModeling.Core.StringField();
        _gotonextrecord = new global::Blackbaud.AppFx.UIModeling.Core.BooleanField();
        _save = new global::Blackbaud.AppFx.UIModeling.Core.GenericUIAction();
        _cancel = new global::Blackbaud.AppFx.UIModeling.Core.GenericUIAction();

        this.SearchListId = new System.Guid("60dca9c9-9c1a-4f50-b255-ef0eb1414779");
        this.SearchRecordType = "Revenue";
        this.FORMHEADER.Value = "Revenue Batch MIMED Duplicate Constituent Search";
        this.HelpKey = "DATABatchDedupe.html";
        this.TranslationFunctionId = new System.Guid("ec1bc00b-fc28-435a-a6ad-07938211558f");
        this.UserInterfaceUrl = "browser/htmlforms/custom/um.customfx.gratefulpatient/UMHSDuplicateConstituentSearch.html";

        //
        //_editconstituentcontext
        //
        _editconstituentcontext.Name = "EDITCONSTITUENTCONTEXT";
        _editconstituentcontext.Caption = "EDITCONSTITUENTCONTEXT";
        _editconstituentcontext.Visible = false;
        _editconstituentcontext.MaxLength = 110;
        this.Fields.Add(_editconstituentcontext);
        //
        //_constituentid
        //
        _constituentid.Name = "CONSTITUENTID";
        _constituentid.Caption = "Constituent ID";
        this.Fields.Add(_constituentid);
        //
        //_constituentlookupid
        //
        _constituentlookupid.Name = "CONSTITUENTLOOKUPID";
        _constituentlookupid.Caption = "Lookup ID";
        _constituentlookupid.SearchListId = new System.Guid("6c1da9a2-766e-4b02-8878-5f5af7ec527e");
        _constituentlookupid.EnableQuickFind = true;
        _constituentlookupid.SearchListAddForms.Add(new global::Blackbaud.AppFx.UIModeling.Core.SearchListAddForm(new System.Guid("98819aef-f26b-48ab-ae31-146720533211"), "Individual"));
        _constituentlookupid.SearchListAddForms.Add(new global::Blackbaud.AppFx.UIModeling.Core.SearchListAddForm(new System.Guid("6dd8c507-481a-430b-925c-4b6a3a8663ae"), "Household"));
        _constituentlookupid.SearchListAddForms.Add(new global::Blackbaud.AppFx.UIModeling.Core.SearchListAddForm(new System.Guid("6e0e7fb5-4551-4135-bebf-c4356bc25fd5"), "Group"));
        _constituentlookupid.SearchListAddForms.Add(new global::Blackbaud.AppFx.UIModeling.Core.SearchListAddForm(new System.Guid("f38b0a00-51cb-43a6-8c82-a4f0ee371435"), "Organization"));
        this.Fields.Add(_constituentlookupid);
        //
        //_newconstituent
        //
        _newconstituent.Name = "NEWCONSTITUENT";
        _newconstituent.Caption = "New constituent";
        _newconstituent.Visible = false;
        _newconstituent.AllowSingleRowOnly = true;
        this.Fields.Add(_newconstituent);
        //
        //_overallmatchthreshold
        //
        _overallmatchthreshold.Name = "OVERALLMATCHTHRESHOLD";
        _overallmatchthreshold.Caption = "Overall match threshold";
        _overallmatchthreshold.Visible = false;
        _overallmatchthreshold.AvailableToClient = false;
        _overallmatchthreshold.Precision = 20;
        _overallmatchthreshold.Scale = 4;
        this.Fields.Add(_overallmatchthreshold);
        //
        //_automatchthreshold
        //
        _automatchthreshold.Name = "AUTOMATCHTHRESHOLD";
        _automatchthreshold.Caption = "Auto match threshold";
        _automatchthreshold.Visible = false;
        _automatchthreshold.AvailableToClient = false;
        _automatchthreshold.Precision = 20;
        _automatchthreshold.Scale = 4;
        this.Fields.Add(_automatchthreshold);
        //
        //_batchid
        //
        _batchid.Name = "BATCHID";
        _batchid.Caption = "BATCHID";
        _batchid.Visible = false;
        this.Fields.Add(_batchid);
        //
        //_constituentname
        //
        _constituentname.Name = "CONSTITUENTNAME";
        _constituentname.Caption = "CONSTITUENTNAME";
        _constituentname.DBReadOnly = true;
        this.Fields.Add(_constituentname);
        //
        //_constituentaddress
        //
        _constituentaddress.Name = "CONSTITUENTADDRESS";
        _constituentaddress.Caption = "CONSTITUENTADDRESS";
        _constituentaddress.DBReadOnly = true;
        this.Fields.Add(_constituentaddress);
        //
        //_gotonextrecord
        //
        _gotonextrecord.Name = "GOTONEXTRECORD";
        _gotonextrecord.Caption = "Automatically go to the next duplicate exception";
        _gotonextrecord.DBReadOnly = true;
        _gotonextrecord.Value = true;
        this.Fields.Add(_gotonextrecord);
        //
        //_save
        //
        _save.Name = "SAVE";
        _save.Caption = "Skip";
        this.Actions.Add(_save);
        //
        //_cancel
        //
        _cancel.Name = "CANCEL";
        _cancel.Caption = "Cancel";
        this.Actions.Add(_cancel);

		OnCreated();

	}

    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @EDITCONSTITUENTCONTEXT {
		get { return _editconstituentcontext; }
	}

    /// <summary>
    /// Constituent ID
    /// </summary>
    [System.ComponentModel.Description("Constituent ID")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.GuidField @CONSTITUENTID {
		get { return _constituentid; }
	}

    /// <summary>
    /// Lookup ID
    /// </summary>
    [System.ComponentModel.Description("Lookup ID")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.SearchListField<System.Guid> @CONSTITUENTLOOKUPID {
		get { return _constituentlookupid; }
	}

    /// <summary>
    /// New constituent
    /// </summary>
    [System.ComponentModel.Description("New constituent")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.CollectionField<UMHSDuplicateConstituentSearchNEWCONSTITUENTUIModel> @NEWCONSTITUENT {
		get { return _newconstituent; }
	}

    /// <summary>
    /// Overall match threshold
    /// </summary>
    [System.ComponentModel.Description("Overall match threshold")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.DecimalField @OVERALLMATCHTHRESHOLD {
		get { return _overallmatchthreshold; }
	}

    /// <summary>
    /// Auto match threshold
    /// </summary>
    [System.ComponentModel.Description("Auto match threshold")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.DecimalField @AUTOMATCHTHRESHOLD {
		get { return _automatchthreshold; }
	}

    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.GuidField @BATCHID {
		get { return _batchid; }
	}

    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @CONSTITUENTNAME {
		get { return _constituentname; }
	}

    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.StringField @CONSTITUENTADDRESS {
		get { return _constituentaddress; }
	}

    /// <summary>
    /// Automatically go to the next duplicate exception
    /// </summary>
    [System.ComponentModel.Description("Automatically go to the next duplicate exception")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.BooleanField @GOTONEXTRECORD {
		get { return _gotonextrecord; }
	}

    /// <summary>
    /// Skip
    /// </summary>
    [System.ComponentModel.Description("Skip")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.GenericUIAction @SAVE {
		get { return _save; }
	}

    /// <summary>
    /// Cancel
    /// </summary>
    [System.ComponentModel.Description("Cancel")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "4.0.165.0")]
	public global::Blackbaud.AppFx.UIModeling.Core.GenericUIAction @CANCEL {
		get { return _cancel; }
	}

}


}