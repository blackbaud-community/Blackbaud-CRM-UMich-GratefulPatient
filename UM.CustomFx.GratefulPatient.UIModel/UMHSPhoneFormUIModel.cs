using Blackbaud.AppFx.UIModeling.Core;
using System;
using UM.CustomFx.GratefulPatient.UIModel.Helpers;
using Blackbaud.AppFx.Constituent.UIModel.My.Resources;
using Blackbaud.AppFx.Server;
using Microsoft.VisualBasic.CompilerServices;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class UMHSPhoneFormUIModel
	{
        private Dictionary<Guid, string> _countryCodesDictionary;
        private CountryGetFormatsReply _countryFormats;


        private void UMHSPhoneFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
            this._countryCodesDictionary = new Dictionary<Guid, string>();

            switch (this.Mode)
            {
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Edit:
                    this.FORMHEADER.Value = "Edit MIMED Phone";
                    break;
                case Blackbaud.AppFx.UIModeling.Core.DataFormMode.Add:
                    this.FORMHEADER.Value = "Add MIMED Phone";                    
                    break;
            }
            _infosourcecodeid.ResetDataSource();
            this._countryFormats = GetCountryFormats(this.GetRequestContext());
            this.ReformatPhoneNumber();
           // this.GetCountryCodes();
            this.DONOTCALLREASONCODEID.Enabled = this.DONOTCALL.Value;
            if (this.ISPRIMARY.Value)
            {
                this.ISPRIMARY.Enabled = false;
            }           
        }

        internal static CountryGetFormatsReply GetCountryFormats(RequestContext context)
        {
            if (context == null)
                return (CountryGetFormatsReply)null;
            CountryGetFormatsRequest req = new CountryGetFormatsRequest();
            return new AppFxWebService(context).CountryGetFormats(req);
        }

        private void ReformatPhoneNumber()
        {
            CountryGetFormatsCountryRow formatsCountryRow = GetCountryFormatsCountryRow(this._countryFormats, this._countryid.Value);
            this._number.FormatCode = formatsCountryRow == null ? PhoneFormat.Unformatted : (PhoneFormat)Conversions.ToInteger(Enum.Parse(typeof(PhoneFormat), formatsCountryRow.PhoneFormatCode.ToString((IFormatProvider)CultureInfo.InvariantCulture)));
            if (this._number.Value == null)
                return;
            this._number.Value = this._number.GetFormattedPhoneNumber(this._number.Value);
        }

        internal static CountryGetFormatsCountryRow GetCountryFormatsCountryRow(CountryGetFormatsReply countryFormats, Guid countryID)
        {
            List<CountryGetFormatsCountryRow>.Enumerator enumerator;
            enumerator = countryFormats.Countries.GetEnumerator();
            try
            {                
                while (enumerator.MoveNext())
                {
                    CountryGetFormatsCountryRow current = enumerator.Current;
                    if (current.ID == countryID)
                        return current;
                }
            }
            finally
            {
                enumerator.Dispose();
            }
            return (CountryGetFormatsCountryRow)null;
        }

       

        //private void GetCountryCodes()
        //{
        //    UIModelCollection<UMHSPhoneFormUIModel> uiModelCollection = this._countrycodes.Value;
        //    if (uiModelCollection == null)
        //        return;
        //    if (uiModelCollection.Count <= 0)
        //        return;
        //        IEnumerator<UMHSPhoneFormUIModel> enumerator;
        //        enumerator = uiModelCollection.GetEnumerator();
        //    try
        //    {
                
        //        while (enumerator.MoveNext())
        //        {
        //            UMHSPhoneFormUIModel current = enumerator.Current;
        //            this._countryCodesDictionary.Add(current.COUNTRYID.Value, current.COUNTRYCODE.Value);
        //        }
        //    }
        //    finally
        //    {
        //        if (enumerator != null)
        //            enumerator.Dispose();
        //    }
        //}

        private void _donotcall_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            this.CALLBEFORE.Enabled = !this.DONOTCALL.Value;
            this.CALLBEFORE.Enabled = !this.DONOTCALL.Value;
            this.DONOTCALLREASONCODEID.Enabled = this.DONOTCALL.Value;
        }
        

        private void _enddate_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            this.ISPRIMARY.Enabled = !this.ENDDATE.HasValue();
            this.DONOTCALL.Enabled = !this.ENDDATE.HasValue();
            if (!this.ENDDATE.HasValue())
                return;
            this.DONOTCALL.Value = true;
        }

        private void PhoneAddFormUIModel_Validating(object sender, ValidatingEventArgs e)
        {
            if (this.ISPRIMARY.Value)
                this.ENDDATE.Value = DateTime.MinValue;
            ValidatingEventArgs validatingEventArgs1 = e;
            ValidatingEventArgs validatingEventArgs2 = e;
            string invalidReason = validatingEventArgs2.InvalidReason;
            validatingEventArgs2.InvalidReason = invalidReason;
        }

        #region "Event handlers"

        //public virtual CollectionField<UMHSPhoneFormUIModel> _countrycodes
        //{
        //    get
        //    {
        //        return this._countrycodes;
        //    }
        //    [MethodImpl(MethodImplOptions.Synchronized)]
        //    set
        //    {
        //        this._countrycodes = value;
        //    }
        //}

        partial void OnCreated()
		{
			this.Loaded += UMHSPhoneFormUIModel_Loaded;
            this.DONOTCALL.ValueChanged += _donotcall_ValueChanged;

            //this._countrycodes = new CollectionField<UMHSPhoneFormUIModel>();
            //this._countrycodes.Name = "COUNTRYCODES";
            //this._countrycodes.Caption = "COUNTRYCODES";
            //this._countrycodes.Visible = false;
            //this._countrycodes.DBReadOnly = true;
            //this.Fields.Add((UIField)this._countrycodes);

        }

#endregion

	}

}