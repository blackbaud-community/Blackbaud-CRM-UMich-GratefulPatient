using System;
using System.Collections.Generic;
using Blackbaud.AppFx.UIModeling.Core;
using Blackbaud.AppFx.Server;
using Blackbaud.AppFx.XmlTypes;
using Microsoft.VisualBasic;

namespace UM.CustomFx.GratefulPatient.UIModel
{
    public class UMHDataConstituentSearchHelper
    {

        private RootUIModel withEventsField__model;
        private RootUIModel _model
        {
            get { return withEventsField__model; }
            set
            {
                if (withEventsField__model != null)
                {
                    withEventsField__model.Validating -= _model_validating;
                }
                withEventsField__model = value;
                if (withEventsField__model != null)
                {
                    withEventsField__model.Validating += _model_validating;
                }
            }
        }

        private SearchListOutputType _outputDefinition;
        private int _numConstituentTypesAllowed;

        private bool _includeOrgIsEnabled = true;
        //Formfields
        private BooleanField withEventsField__checkmergedconstituents;
        private BooleanField _checkmergedconstituents
        {
            get { return withEventsField__checkmergedconstituents; }
            set
            {
                if (withEventsField__checkmergedconstituents != null)
                {
                    withEventsField__checkmergedconstituents.ValueChanged -= _checkmergedconstituents_ValueChanged;
                }
                withEventsField__checkmergedconstituents = value;
                if (withEventsField__checkmergedconstituents != null)
                {
                    withEventsField__checkmergedconstituents.ValueChanged += _checkmergedconstituents_ValueChanged;
                }
            }
        }
        private BooleanField withEventsField__exactmatchonly;
        private BooleanField _exactmatchonly
        {
            get { return withEventsField__exactmatchonly; }
            set
            {
                if (withEventsField__exactmatchonly != null)
                {
                    withEventsField__exactmatchonly.ValueChanged -= _exactmatchonly_ValueChanged;
                }
                withEventsField__exactmatchonly = value;
                if (withEventsField__exactmatchonly != null)
                {
                    withEventsField__exactmatchonly.ValueChanged += _exactmatchonly_ValueChanged;
                }
            }
        }
        private GenericUIAction withEventsField__hideadvancedoptions;
        private GenericUIAction _hideadvancedoptions
        {
            get { return withEventsField__hideadvancedoptions; }
            set
            {
                if (withEventsField__hideadvancedoptions != null)
                {
                    withEventsField__hideadvancedoptions.InvokeAction -= _hideAdvancedOptions_InvokeAction;
                }
                withEventsField__hideadvancedoptions = value;
                if (withEventsField__hideadvancedoptions != null)
                {
                    withEventsField__hideadvancedoptions.InvokeAction += _hideAdvancedOptions_InvokeAction;
                }
            }
        }
        private GenericUIAction withEventsField__showadvancedoptions;
        private GenericUIAction _showadvancedoptions
        {
            get { return withEventsField__showadvancedoptions; }
            set
            {
                if (withEventsField__showadvancedoptions != null)
                {
                    withEventsField__showadvancedoptions.InvokeAction -= _showAdvancedOptions_InvokeAction;
                }
                withEventsField__showadvancedoptions = value;
                if (withEventsField__showadvancedoptions != null)
                {
                    withEventsField__showadvancedoptions.InvokeAction += _showAdvancedOptions_InvokeAction;
                }
            }
        }
        private BooleanField withEventsField__includeindividuals;
        private BooleanField _includeindividuals
        {
            get { return withEventsField__includeindividuals; }
            set
            {
                if (withEventsField__includeindividuals != null)
                {
                    withEventsField__includeindividuals.ValueChanged -= _includeindividuals_ValueChanged;
                }
                withEventsField__includeindividuals = value;
                if (withEventsField__includeindividuals != null)
                {
                    withEventsField__includeindividuals.ValueChanged += _includeindividuals_ValueChanged;
                }
            }
        }
        private SimpleDataListField<Guid> withEventsField__countryId;
        private SimpleDataListField<Guid> _countryId
        {
            get { return withEventsField__countryId; }
            set
            {
                if (withEventsField__countryId != null)
                {
                    withEventsField__countryId.ValueChanged -= _countryid_ValueChanged;
                }
                withEventsField__countryId = value;
                if (withEventsField__countryId != null)
                {
                    withEventsField__countryId.ValueChanged += _countryid_ValueChanged;
                }
            }
        }
        private DateField _minimumdate;
        private StringField _keyname;
        private StringField _firstname;
        private StringField _addressblock;
        private StringField _city;
        private SimpleDataListField<Guid> _stateid;
        private SearchListField<string> _postcode;
        private BooleanField _checknickname;
        private BooleanField _checkaliases;
        private BooleanField _onlyprimaryaddress;
        private StringField _primarybusiness;
        private YearField _classof;
        private StringField _emailaddress;
        private StringField _constituentquickfind;
        private BooleanField _includeorganizations;
        private BooleanField _includegroups;
        private BooleanField _excludecustomgroups;
        private BooleanField _excludehouseholds;
        private StringField _ssn;
        private BooleanField _fuzzysearchonname;

        private StringField _searchin;

        private static Guid DEFAULTCOUNTRY_VIEWFORMID = new Guid("DC02EFEE-963C-475B-A407-EE9721DB0422");
        public UMHDataConstituentSearchHelper(RootUIModel model, Blackbaud.AppFx.XmlTypes.SearchListOutputType outputDefinition)
        {
            _model = model;
            _outputDefinition = outputDefinition;

            _checkmergedconstituents = (BooleanField)_model.Fields["CHECKMERGEDCONSTITUENTS"];
            _exactmatchonly = (BooleanField)_model.Fields["EXACTMATCHONLY"];
            _includeindividuals = (BooleanField)_model.Fields["INCLUDEINDIVIDUALS"];
            _checknickname = (BooleanField)_model.Fields["CHECKNICKNAME"];
            _checkaliases = (BooleanField)_model.Fields["CHECKALIASES"];
            _onlyprimaryaddress = (BooleanField)_model.Fields["ONLYPRIMARYADDRESS"];
            _includeorganizations = (BooleanField)_model.Fields["INCLUDEORGANIZATIONS"];
            _includegroups = (BooleanField)_model.Fields["INCLUDEGROUPS"];
            _excludecustomgroups = (BooleanField)_model.Fields["EXCLUDECUSTOMGROUPS"];
            _excludehouseholds = (BooleanField)_model.Fields["EXCLUDEHOUSEHOLDS"];
            _fuzzysearchonname = (BooleanField)_model.Fields["FUZZYSEARCHONNAME"];
            _countryId = (SimpleDataListField<Guid>)_model.Fields["COUNTRYID"];
            _stateid = (SimpleDataListField<Guid>)_model.Fields["STATEID"];
            _postcode = (SearchListField<string>)_model.Fields["POSTCODE"];
            _keyname = (StringField)_model.Fields["KEYNAME"];
            _firstname = (StringField)_model.Fields["FIRSTNAME"];
            _addressblock = (StringField)_model.Fields["ADDRESSBLOCK"];
            _city = (StringField)_model.Fields["CITY"];
            _primarybusiness = (StringField)_model.Fields["PRIMARYBUSINESS"];
            _emailaddress = (StringField)_model.Fields["EMAILADDRESS"];
            _ssn = (StringField)_model.Fields["SSN"];
            _searchin = (StringField)_model.Fields["SEARCHIN"];
            _minimumdate = (DateField)_model.Fields["MINIMUMDATE"];
            _classof = (YearField)_model.Fields["CLASSOF"];

            UIField constituentQuickFindField = null;
            if (_model.Fields.TryGetValue("CONSTITUENTQUICKFIND", ref constituentQuickFindField))
            {
                _constituentquickfind = (StringField)constituentQuickFindField;
            }

            _hideadvancedoptions = (GenericUIAction)_model.Actions["HIDEADVANCEDOPTIONS"];
            _showadvancedoptions = (GenericUIAction)_model.Actions["SHOWADVANCEDOPTIONS"];
        }

        static internal CountryGetFormatsFormatRow GetCountryFormatRow(CountryGetFormatsReply countryFormats, Guid countryID)
        {
            foreach (var row_loopVariable in countryFormats.Formats)
            {
                var row = row_loopVariable;
                if (row.ID == countryID)
                {
                    return row;
                }
            }
            return null;
        }

        static internal CountryGetFormatsCountryRow GetCountryFormatsCountryRow(CountryGetFormatsReply countryFormats, Guid countryID)
        {
            foreach (var row_loopVariable in countryFormats.Countries)
            {
                var row = row_loopVariable;
                if (row.ID == countryID)
                {
                    return row;
                }
            }
            return null;
        }

        static internal CountryGetFormatsReply GetCountryFormats(RequestContext context)
        {

            if ((context == null))
                return null;

            dynamic req = new CountryGetFormatsRequest();
            dynamic svc = new AppFxWebService(context);
            return svc.CountryGetFormats(req);

        }

        internal void Init()
        {
            List<string> keyNameCaptionTypes = new List<string>(3);

            if (_includeindividuals.Value)
                _numConstituentTypesAllowed += 1;
            if (_includeorganizations.Value)
                _numConstituentTypesAllowed += 1;
            if (_includegroups.Value)
                _numConstituentTypesAllowed += 1;

            if (!_includeorganizations.Enabled)
            {
                _includeOrgIsEnabled = false;
            }

            if (_includeindividuals.Value)
            {
                keyNameCaptionTypes.Add("Last");
            }

            if (_includeorganizations.Value)
            {
                if (_numConstituentTypesAllowed == 1)
                {
                    keyNameCaptionTypes.Add("Organization");
                }
                else
                {
                    keyNameCaptionTypes.Add("Org");
                }
            }

            if (_includegroups.Value)
            {
                if (_excludecustomgroups.Value)
                {
                    keyNameCaptionTypes.Add("Household");
                }
                else if (_numConstituentTypesAllowed == 1)
                {
                    keyNameCaptionTypes.Add("Group/Household");
                }
                else
                {
                    keyNameCaptionTypes.Add("Group");
                }
            }

            EnableConstituentTypeOptions();

            _keyname.Caption = string.Format("{0} name", Strings.Join(keyNameCaptionTypes.ToArray(), "/"));

            if (_excludecustomgroups.Value)
            {
                _includegroups.Caption = "Households";
            }
            else if (_excludehouseholds.Value)
            {
                _includegroups.Caption = "Groups";
            }

            DataFormLoadRequest req = new DataFormLoadRequest { FormID = new Guid("12c490b6-5f6b-4eab-85a7-3c38ee492b44") };
            dynamic reply = ServiceMethods.DataFormLoad(req, _model.GetRequestContext());


            //BVS-SOTEY commented out

            //bool AVAILABLEZIPSEARCH = false;
            //if (reply.DataFormItem.TryGetValue("AVAILABLEZIPSEARCH", AVAILABLEZIPSEARCH))
            //{
            //    //Searching Postcodes is enabled based on whether there are available Zip codes to search.
            //    //Otherwise, it is just a text box to enter.
            //    _postcode.SearchEnabled = AVAILABLEZIPSEARCH;
            //}

            if (_ssn.Visible)
            {
                if (!_model.GetRequestContext().UserIsGrantedPrivilege(new Guid("d908668e-210f-47a8-bd45-ef9517e4894f")))
                {
                    _ssn.Visible = false;
                }
            }

            //Setting Labels based on Default Country
            CountryGetFormatsReply _countryFormats = GetCountryFormats(_model.GetRequestContext());
            CountryGetFormatsFormatRow labelRow = GetCountryFormatRow(_countryFormats, GetDefaultCountryID());

            if (labelRow != null)
            {
                _city.Caption = labelRow.City;
                _stateid.Caption = labelRow.State;
                _postcode.Caption = labelRow.PostCode;

                //Updating Grid Columns Header
                foreach (SearchListOutputFieldType outputField in _outputDefinition.OutputFields)
                {
                    switch (outputField.FieldID)
                    {
                        case "City":
                            outputField.Caption = labelRow.City;
                            break;
                        case "State":
                            outputField.Caption = labelRow.State;
                            break;
                        case "Postcode":
                            outputField.Caption = labelRow.PostCode;
                            break;
                    }
                }
            }
        }

        //Gets Default CountryID
        private Guid GetDefaultCountryID()
        {

            DataFormLoadRequest request = new DataFormLoadRequest
            {
                FormID = DEFAULTCOUNTRY_VIEWFORMID,
                IncludeMetaData = false
            };
            DataFormLoadReply reply = default(DataFormLoadReply);
            Guid countryID = Guid.Empty;

            reply = ServiceMethods.DataFormLoad(request, _model.GetRequestContext());

            if (reply.DataFormItem.TryGetValue("COUNTRYID", ref countryID))
            {
                return countryID;
            }

            return Guid.Empty;

        }


        private void _showAdvancedOptions_InvokeAction(object sender, InvokeActionEventArgs e)
        {
            _showadvancedoptions.Visible = false;
            _hideadvancedoptions.Visible = true;
        }

        private void _hideAdvancedOptions_InvokeAction(object sender, InvokeActionEventArgs e)
        {
            _showadvancedoptions.Visible = true;
            _hideadvancedoptions.Visible = false;
        }

        private void _exactmatchonly_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (_exactmatchonly.Value)
            {
                _fuzzysearchonname.Value = false;
                _fuzzysearchonname.Enabled = false;
            }
            else
            {
                _fuzzysearchonname.Enabled = true;
            }
        }


        private void _checkmergedconstituents_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            dynamic skipMerged = !_checkmergedconstituents.Value;

            _minimumdate.Visible = !skipMerged;

            _keyname.Enabled = skipMerged;
            _firstname.Enabled = skipMerged && _includeindividuals.Value;
            _exactmatchonly.Enabled = skipMerged;
            _addressblock.Enabled = skipMerged;
            _city.Enabled = skipMerged;
            _stateid.Enabled = skipMerged;
            _postcode.Enabled = skipMerged;
            _checknickname.Enabled = skipMerged;
            _checkaliases.Enabled = skipMerged;
            _onlyprimaryaddress.Enabled = skipMerged;
            _primarybusiness.Enabled = skipMerged;
            _classof.Enabled = skipMerged;
            _emailaddress.Enabled = skipMerged;

            //This field is only present in ConstituentSearchByNameOrLookupIDUIModel.
            if (_constituentquickfind != null)
            {
                _constituentquickfind.Enabled = skipMerged;
            }

            EnableConstituentTypeOptions();

        }

        private void _model_validating(object sender, ValidatingEventArgs e)
        {
            if (!(_includeindividuals.Value || _includeorganizations.Value || _includegroups.Value))
            {
                e.Valid = false;
                e.InvalidFieldName = _includeindividuals.Name;
                e.InvalidReason = "You must select at least one constituent type.";
            }
        }

        private void _includeindividuals_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            _firstname.Enabled = _includeindividuals.Value;
        }


        private void EnableConstituentTypeOptions()
        {
            dynamic skipMerged = !_checkmergedconstituents.Value;
            dynamic allowsMultipleTypes = _numConstituentTypesAllowed > 1;

            _searchin.Enabled = skipMerged && allowsMultipleTypes;
            _includeindividuals.Enabled = skipMerged && allowsMultipleTypes;
            _includeorganizations.Enabled = skipMerged && allowsMultipleTypes && _includeOrgIsEnabled;
            _includegroups.Enabled = skipMerged && allowsMultipleTypes;

        }

        private void _countryid_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (!_countryId.Model.RootUIModel().Loading)
            {
                _stateid.Value = Guid.Empty;
            }

            _stateid.ResetDataSource();

            CountryGetFormatsReply _countryFormats = GetCountryFormats(_model.GetRequestContext());
            CountryGetFormatsFormatRow labelRow = GetCountryFormatRow(_countryFormats, _countryId.Value);

            if (labelRow != null)
            {
                _city.Caption = labelRow.City;
                _stateid.Caption = labelRow.State;
                _postcode.Caption = labelRow.PostCode;
            }
        }
    }
}