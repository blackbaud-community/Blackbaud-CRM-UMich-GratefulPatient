using Blackbaud.AppFx.Constituent.UIModel.DuplicateCheck;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class UMHSDataConstituentSearchUIModel
	{

        private int _numConstituentTypesAllowed;
        private void UMHSDataConstituentSearchUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
            UMHDataConstituentSearchHelper helper = new UMHDataConstituentSearchHelper(this.RootUIModel(), this.OutputDefinition);
            helper.Init();

            _advancedsearchoptionsgroup.Visible = false;

            _showadvancedoptions.Visible = true;
            _hideadvancedoptions.Visible = false;

            List<string> keyNameCaptionTypes = new List<string>(3);

            if (_includeindividuals.Value)
                _numConstituentTypesAllowed += 1;
            if (_includeorganizations.Value)
                _numConstituentTypesAllowed += 1;
            if (_includegroups.Value)
                _numConstituentTypesAllowed += 1;

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
        }

        #region "Event handlers"

        partial void OnCreated()
		{
			this.Loaded += UMHSDataConstituentSearchUIModel_Loaded;
            this._showadvancedoptions.InvokeAction += _showadvancedoptions_InvokeAction;
            this._hideadvancedoptions.InvokeAction += _hideadvancedoptions_InvokeAction;
            this._exactmatchonly.ValueChanged += _exactmatchonly_ValueChanged;
            this._checkmergedconstituents.ValueChanged += _checkmergedconstituents_ValueChanged;
            this._includeindividuals.ValueChanged += _includeindividuals_ValueChanged;
            this._countryid.ValueChanged += _countryid_ValueChanged;

            this.Validating += UMHSDataConstituentSearchUIModel_Validating;
		}

        private void EnableConstituentTypeOptions()
        {
            dynamic skipMerged = !_checkmergedconstituents.Value;
            dynamic allowsMultipleTypes = _numConstituentTypesAllowed > 1;

            _searchin.Enabled = skipMerged && allowsMultipleTypes;
            _includeindividuals.Enabled = skipMerged && allowsMultipleTypes;
            _includeorganizations.Enabled = skipMerged && allowsMultipleTypes;
            _includegroups.Enabled = skipMerged && allowsMultipleTypes;

        }

        private void _countryid_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            this.STATEID.ResetDataSource();
        }

        private void _includeindividuals_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            _firstname.Enabled = _includeindividuals.Value;
        }

        private void UMHSDataConstituentSearchUIModel_Validating(object sender, Blackbaud.AppFx.UIModeling.Core.ValidatingEventArgs e)
        {
            if (!(_includeindividuals.Value || _includeorganizations.Value || _includegroups.Value))
            {
                e.Valid = false;
                e.InvalidFieldName = _includeindividuals.Name;
                e.InvalidReason = "You must select at least one constituent type.";
            }
        }

        private void _checkmergedconstituents_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
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

            EnableConstituentTypeOptions();
        }

        private void _exactmatchonly_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
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

        private void _hideadvancedoptions_InvokeAction(object sender, Blackbaud.AppFx.UIModeling.Core.InvokeActionEventArgs e)
        {
            _showadvancedoptions.Visible = true;
            _hideadvancedoptions.Visible = false;
        }

        private void _showadvancedoptions_InvokeAction(object sender, Blackbaud.AppFx.UIModeling.Core.InvokeActionEventArgs e)
        {
            _showadvancedoptions.Visible = false;
            _hideadvancedoptions.Visible = true;      
        }

        #endregion

    }

}