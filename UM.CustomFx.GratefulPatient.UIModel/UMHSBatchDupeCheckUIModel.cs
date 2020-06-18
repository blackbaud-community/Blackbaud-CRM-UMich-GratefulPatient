using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Blackbaud.AppFx;
using Blackbaud.AppFx.Constituent.UIModel;
using Blackbaud.AppFx.Constituent.UIModel.DuplicateCheck;
using Blackbaud.AppFx.Server;
using Blackbaud.AppFx.UIModeling.Core;
using Blackbaud.AppFx.XmlTypes.DataForms;
using Microsoft.VisualBasic.CompilerServices;

namespace UM.CustomFx.GratefulPatient.UIModel
{

    public partial class UMHSBatchDupeCheckUIModel
    {

        private DuplicateResolutionUIModel _customModel;
        private const int TIMEOUT = 3600;

        private void UMHSBatchDupeCheckUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
        {
            Type type = typeof(DuplicateResolutionUIModel);
            CreateCustomUIModelArgs createCustomUIModelArg = new CreateCustomUIModelArgs()
            {
                AssemblyName = type.Assembly.ToString(),
                ClassName = type.FullName,
                RootModel = this,
                Context = this.GetRequestContext()
            };
            this._customModel = (DuplicateResolutionUIModel)CustomUIModel.CreateModel(createCustomUIModelArg);
            this._customModel.Initialize();

            _customModel.SaveCaptionChanged += _customModel_SaveCaptionChanged;

            this.HostedModels.Add(this._customModel);
            this.DialogActions.Add(this.SAVE);
            this.DialogActions.Add(this.CANCEL);
            UMHSBatchDupeCheckUIModel _UMHSBatchDupeCheckUIModel = this;
            this._customModel.GOTONEXTRECORD.ValueChanged += new EventHandler<ValueChangedEventArgs>(_UMHSBatchDupeCheckUIModel.nextRecord_ValueChanged);
            this._customModel.SaveButtonCaption = "Save";
            this.SAVE.Caption = "Save";
            this.DUPLICATERECORDID.Value = BATCHCONSTITUENTID.Value;

            //if (this._conflictFields.Count == 0 && !this._isAlias && (this._primaryAddressModelInstanceId.Equals(Guid.Empty) && this._secondaryAddressModelInstanceId.Equals(Guid.Empty)) && (this._primaryEmailAddressModelInstanceId.Equals(Guid.Empty) && this._secondaryEmailAddressModelInstanceId.Equals(Guid.Empty) && (this._primaryPhoneModelInstanceId.Equals(Guid.Empty) && this._secondaryPhoneModelInstanceId.Equals(Guid.Empty))) && this._constituentUpdateBatchID.Equals(Guid.Empty))
            //    flag = false;

            //typeof(DuplicateResolutionUIModel)
            //    .GetField("CONSTITUENTDUPLICATEMATCHEDITFORMID", BindingFlags.Instance | BindingFlags.NonPublic)
            //    .SetValue(_customModel, "4417055D-0EFD-4F02-AF00-EA24CD901E7E");

            //FieldInfo field = typeof(DuplicateResolutionUIModel).GetField("CONSTITUENTDUPLICATEMATCHEDITFORMID", BindingFlags.Instance | BindingFlags.NonPublic);
            //field.SetValue(_customModel, "4417055D-0EFD-4F02-AF00-EA24CD901E7E");
        }

        private void _customModel_SaveCaptionChanged(object sender, DuplicateResolutionSaveCaptionChangedEventArgs e)
        {
            this.SAVE.Caption = e.SaveCaption;
        }

        #region "Event handlers"

        partial void OnCreated()
        {
            this.Loaded += UMHSBatchDupeCheckUIModel_Loaded;
            this.DUPLICATERECORDID.ValueChanged += DUPLICATERECORDID_ValueChanged;
            this.SearchResultsLoaded += UMHSBatchDupeCheckUIModel_SearchResultsLoaded;
            _save.InvokeAction += _save_InvokeAction;
            _cancel.InvokeAction += _cancel_InvokeAction;
            
        }



        private void UMHSBatchDupeCheckUIModel_SearchResultsLoaded(object sender, SearchResultsLoadedEventArgs e)
        {
            this.DUPLICATERECORDID.Value = BATCHCONSTITUENTID.Value;
            this._customModel.LoadMatches(e, "ID", null);
        }

        private void _cancel_InvokeAction(object sender, InvokeActionEventArgs e)
        {
            this.GOTONEXTRECORD.Value = false;
            this.DialogResult = UIModelDialogResult.Canceled;
        }

        private void DUPLICATERECORDID_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (this.DUPLICATERECORDID.HasValue())
            {
                this._customModel.LoadFromDataFormItem(this.ToDataFormItem());
                this.LoadFromBatch(this.DUPLICATERECORDID.Value);
                SetRules();
                DisplayRules();

                string empty = string.Empty;
                if (this._customModel.FIRSTNAME.HasValue())
                {
                    empty = string.Concat(empty, string.Concat(this._customModel.FIRSTNAME.Value, " "));
                }
                empty = string.Concat(empty, string.Concat(this._customModel.LASTNAME.Value, " "));
                StringField fORMHEADER = this.FORMHEADER;
                CultureInfo currentCulture = CultureInfo.CurrentCulture;
                string duplicateResolutionFormHeaderText = string.Format("Reconciliation for {0}", empty);

                fORMHEADER.Value = string.Format(currentCulture, duplicateResolutionFormHeaderText);
            }
        }
        private void SetRules()
        {
            try
            {
                DataFormLoadRequest dataFormLoadRequest = new DataFormLoadRequest();
                dataFormLoadRequest.FormID = new Guid("c22b08b6-eb30-4026-95da-ef4f010ddeaf");
                dataFormLoadRequest.SecurityContext = this.GetRequestSecurityContext();
                DataFormLoadReply dataFormLoadReply = ServiceMethods.DataFormLoad(dataFormLoadRequest, this.GetRequestContext());
                _customModel.NAMECODE.Value = (DuplicateResolutionUIModel.NAMECODES)Conversions.ToInteger(dataFormLoadReply.DataFormItem.Values["NAMECODE"].Value);
                _customModel.SIMILARADDRESSCODE.Value = (DuplicateResolutionUIModel.SIMILARADDRESSCODES)Conversions.ToInteger(dataFormLoadReply.DataFormItem.Values["SIMILARADDRESSCODE"].Value);
                _customModel.UNSIMILARADDRESSCODE.Value = (DuplicateResolutionUIModel.UNSIMILARADDRESSCODES)Conversions.ToInteger(dataFormLoadReply.DataFormItem.Values["UNSIMILARADDRESSCODE"].Value);
                _customModel.NEWADDRESSPRIMARYCODE.Value = (DuplicateResolutionUIModel.NEWADDRESSPRIMARYCODES)Conversions.ToInteger(dataFormLoadReply.DataFormItem.Values["NEWADDRESSPRIMARYCODE"].Value);
                _customModel.DIFFERENTPHONECODE.Value = (DuplicateResolutionUIModel.DIFFERENTPHONECODES)Conversions.ToInteger(dataFormLoadReply.DataFormItem.Values["DIFFERENTPHONECODE"].Value);
                _customModel.NEWPHONEPRIMARYCODE.Value = (DuplicateResolutionUIModel.NEWPHONEPRIMARYCODES)Conversions.ToInteger(dataFormLoadReply.DataFormItem.Values["NEWPHONEPRIMARYCODE"].Value);
                _customModel.DIFFERENTEMAILCODE.Value = (DuplicateResolutionUIModel.DIFFERENTEMAILCODES)Conversions.ToInteger(dataFormLoadReply.DataFormItem.Values["DIFFERENTEMAILCODE"].Value);
                _customModel.NEWEMAILPRIMARYCODE.Value = (DuplicateResolutionUIModel.NEWEMAILPRIMARYCODES)Conversions.ToInteger(dataFormLoadReply.DataFormItem.Values["NEWEMAILPRIMARYCODE"].Value);
                _customModel.BIRTHDATERULECODE.Value = (DuplicateResolutionUIModel.BIRTHDATERULECODES)Conversions.ToInteger(dataFormLoadReply.DataFormItem.Values["BIRTHDATERULECODE"].Value);

            }
            catch (Exception expr_1DA)
            {
                ProjectData.SetProjectError(expr_1DA);
                Exception ex = expr_1DA;
                throw new InvalidUIModelException(ex.Message);
            }
        }

        private void DisplayRules()
        {
            ValueListField valueListField = _customModel.NAMECODE;
            DisplayRule(ref valueListField);
            valueListField = _customModel.SIMILARADDRESSCODE;
            DisplayRule(ref valueListField);
            valueListField = _customModel.UNSIMILARADDRESSCODE;
            DisplayRule(ref valueListField);
            valueListField = _customModel.NEWADDRESSPRIMARYCODE;
            DisplayRule(ref valueListField);
            valueListField = _customModel.DIFFERENTPHONECODE;
            DisplayRule(ref valueListField);
            valueListField = _customModel.NEWPHONEPRIMARYCODE;
            DisplayRule(ref valueListField);
            valueListField = _customModel.DIFFERENTEMAILCODE;
            DisplayRule(ref valueListField);
            valueListField = _customModel.NEWEMAILPRIMARYCODE;
            DisplayRule(ref valueListField);
        }
        private static void DisplayRule(ref ValueListField field)
        {
            System.Collections.IEnumerator enumerator = field.DataSourceBase.GetEnumerator();
            try
            {

                while (enumerator.MoveNext())
                {
                    object objectValue = System.Runtime.CompilerServices.RuntimeHelpers.GetObjectValue(enumerator.Current);
                    ValueListItem valueListItem = objectValue as ValueListItem;
                    valueListItem.Visible = Operators.ConditionalCompareObjectEqual(field.ValueObject, valueListItem.ValueObject, false);
                }
            }
            finally
            {

                if (enumerator is System.IDisposable)
                {
                    (enumerator as System.IDisposable).Dispose();
                }
            }
        }


        private void _save_InvokeAction(object sender, InvokeActionEventArgs e)
        {
            if (this._customModel.GetMatchStatus == DuplicateResolutionUIModel.MATCHSTATUSValues.MatchConfirmed)
            {
                this.DialogResult = UIModelDialogResult.Completed;
                this.DialogResultRecordId = SaveConstituentInfo().ToString();


                ////this._customModel.MATCHCONSTITUENTID.Value = Guid.Empty;

                //Guid id = this._customModel.SaveForm();
                //this.DialogResultRecordId = id.ToString();
                //SaveConstituentInfo();
                ////typeof(DuplicateResolutionUIModel).GetMethod("SaveConstituentInfo", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_customModel, null); 

                //this.ID.Value = id;

                //LoadFromBatch(BATCHCONSTITUENTREVENUEID.Value);
            }
            else if (this._customModel.GetMatchStatus == DuplicateResolutionUIModel.MATCHSTATUSValues.AddAsNew)
            {
                this.DialogResult = UIModelDialogResult.Ignored;
            }
            else if (this._customModel.GetMatchStatus == DuplicateResolutionUIModel.MATCHSTATUSValues.Unresolved)
            {
                this.DialogResult = UIModelDialogResult.Canceled;
            }
        }

        public Guid SaveConstituentInfo()
        {
            //if (this.BIRTHDATE.HasValue())
            //    this.ApplyBirthdateRule();
            //this.ApplyNameFormatsRule();
            //if (!this.MatchUpdated())
            //{
            //    int num = 0;
            //    DuplicateResolutionUIModel.BATCHTYPES? nullable1 = this.BATCHTYPE.Value;
            //    int? nullable2 = nullable1.HasValue ? new int?((int)nullable1.GetValueOrDefault()) : new int?();
            //    if (!(nullable2.HasValue ? new bool?(nullable2.GetValueOrDefault() == num) : new bool?()).GetValueOrDefault())
            //        return this.MATCHCONSTITUENTID.Value;
            //}

            DataFormSaveRequest request = new DataFormSaveRequest();
            try
            {
                DataFormSaveRequest dataFormSaveRequest = request;
                dataFormSaveRequest.FormID = new Guid("4417055D-0EFD-4F02-AF00-EA24CD901E7E");
                dataFormSaveRequest.ID = this.DUPLICATERECORDID.Value.ToString();
                dataFormSaveRequest.SecurityContext = this.GetRequestSecurityContext();
                dataFormSaveRequest.DataFormItem = _customModel.GetSaveDataFormItem;
                dataFormSaveRequest.AggregateExceptions = true;
                ServiceMethods.DataFormSave(request, this.GetRequestContext());

            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                string message = ex.Message;

                if (null != ex.InnerException)
                {
                    message += $" {ex.InnerException.Message}";
                }

                throw new InvalidUIModelException(message);
            }

            return _customModel.MATCHCONSTITUENTID.Value;
        }

        private void ConstituentBatchDuplicateLookupIDSearchUIModel_SearchResultsLoaded(object sender, SearchResultsLoadedEventArgs e)
        {
            this._customModel.LoadMatches(e, "ID", null);
        }

        private void nextRecord_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            this.GOTONEXTRECORD.Value = bool.Parse(e.NewValue.ToString());
        }

        private void LoadFromBatch(Guid batchconstituentrevenueId)
        {
            Guid id = Guid.Empty;
            using (var con = new SqlConnection(this.GetRequestContext().AppDBConnectionString()))
            {
                using (var command = con.CreateCommand())
                {
                    command.CommandText = "USR_USP_UMHS_CONSTITUENTDUPLICATESEARCHRESOLUTION";
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = TIMEOUT;

                    if (this.BATCHCONSTITUENTID.Value != Guid.Empty)
                        command.Parameters.AddWithValue("@BATCHCONSTITUENTID", (object)this.BATCHCONSTITUENTID.Value);
                    else
                        return;
                    command.Parameters.Add("@BATCHID", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@NAME", SqlDbType.NVarChar, 154).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@LASTNAME", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@FIRSTNAME", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@MIDDLENAME", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@SUFFIXCODEID", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@BIRTHDATE", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ADDRESS_COUNTRYID", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ADDRESS_ADDRESSBLOCK", SqlDbType.NVarChar, 150).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ADDRESS_CITY", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ADDRESS_STATEID", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ADDRESS_POSTCODE", SqlDbType.NVarChar, 12).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@PHONENUMBER", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@EMAILADDRESS", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@BATCHNUMBER", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@CREATEDON", SqlDbType.Date).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@BATCHID", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@BATCHTYPE", SqlDbType.Int).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@DATALOADED", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ADDRESS_TYPECODEID", SqlDbType.UniqueIdentifier).Direction =ParameterDirection.Output;
                    //command.Parameters.Add("@TITLECODEID", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@PHONETYPECODEID", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@EMAILADDRESSTYPECODEID", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ADDRESSTYPECODEID", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@PRIMARYRECORDID", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@BATCHROWID", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@NAMECODE", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@SIMILARADDRESSCODE", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@UNSIMILARADDRESSCODE", SqlDbType.TinyInt).Direction =   ParameterDirection.Output;
                    //command.Parameters.Add("@NEWADDRESSPRIMARYCODE", SqlDbType.TinyInt).Direction =   ParameterDirection.Output;
                    //command.Parameters.Add("@DIFFERENTPHONECODE", SqlDbType.TinyInt).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@NEWPHONEPRIMARYCODE", SqlDbType.TinyInt).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@DIFFERENTEMAILCODE", SqlDbType.TinyInt).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@NEWEMAILPRIMARYCODE", SqlDbType.TinyInt).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@BIRTHDATERULECODE", SqlDbType.TinyInt).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@INCOMINGADDRESSID", SqlDbType.UniqueIdentifier).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@INCOMINGEMAILID", SqlDbType.UniqueIdentifier).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@INCOMINGPHONEID", SqlDbType.UniqueIdentifier).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@MAIDENNAME", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@NICKNAME", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@GENDERCODE", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@DECEASED", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@DECEASEDDATE", SqlDbType.Char, 8).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@GIVESANONYMOUSLY", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@MARITALSTATUSCODEID", SqlDbType.UniqueIdentifier).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@WEBADDRESS", SqlDbType.NVarChar, 2047).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSHISTORICALSTARTDATE", SqlDbType.Date).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSHISTORICALENDDATE", SqlDbType.Date).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSDONOTMAIL", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSDONOTMAILREASONCODEID", SqlDbType.UniqueIdentifier).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSSTARTDATE", SqlDbType.NVarChar, 4).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSENDDATE", SqlDbType.NVarChar, 4).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSDPC", SqlDbType.NVarChar, 4000).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSCART", SqlDbType.NVarChar, 4000).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSLOT", SqlDbType.NVarChar, 5).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSINFOSOURCECODEID", SqlDbType.UniqueIdentifier).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSINFOSOURCECOMMENTS", SqlDbType.NVarChar, 256).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSCOUNTYCODEID", SqlDbType.UniqueIdentifier).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSREGIONCODEID", SqlDbType.UniqueIdentifier).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSCONGRESSIONALDISTRICTCODEID", SqlDbType.UniqueIdentifier).Direction  = ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSSTATEHOUSEDISTRICTCODEID", SqlDbType.UniqueIdentifier).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSSTATESENATEDISTRICTCODEID", SqlDbType.UniqueIdentifier).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSLOCALPRECINCTCODEID", SqlDbType.UniqueIdentifier).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSCERTIFICATIONDATA", SqlDbType.Int).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSLASTVALIDATIONATTEMPTDATE", SqlDbType.Date).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSOMITFROMVALIDATION", SqlDbType.Bit).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSVALIDATIONMESSAGE", SqlDbType.NVarChar, 200).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@PHONEDONOTCALL", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@PHONESTARTTIME", SqlDbType.Char, 4).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@PHONEENDTIME", SqlDbType.Char, 4).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@PHONEINFOSOURCECODEID", SqlDbType.UniqueIdentifier).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@PHONECOUNTRYID", SqlDbType.UniqueIdentifier).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@PHONESTARTDATE", SqlDbType.Date).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@PHONEENDDATE", SqlDbType.Date).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@PHONESEASONALSTARTDATE", SqlDbType.Char, 4).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@PHONESEASONALENDDATE", SqlDbType.Char, 4).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@EMAILADDRESSDONOTEMAIL", SqlDbType.Bit).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@EMAILADDRESSINFOSOURCECODEID", SqlDbType.UniqueIdentifier).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@EMAILADDRESSSTARTDATE", SqlDbType.Date).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@EMAILADDRESSENDDATE", SqlDbType.Date).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@NAMEFORMATS", SqlDbType.Xml).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@ADDRESSISPRIMARY", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@PHONEISPRIMARY", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@EMAILISPRIMARY", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@CONSTITUENCIES", SqlDbType.Xml).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@ORIGINAL_KEYNAME", SqlDbType.NVarChar, 100).Direction =  ParameterDirection.Output;
                    //command.Parameters.Add("@ORIGINAL_FIRSTNAME", SqlDbType.NVarChar, 50).Direction =  ParameterDirection.Output;

                    con.Open();
                    command.ExecuteNonQuery();

                    this._customModel.BATCHID.Value = new Guid(command.Parameters["@BATCHID"].Value.ToString()); 
                    this._customModel.LASTNAME.Value = command.Parameters["@LASTNAME"].Value.ToString();
                    this._customModel.FIRSTNAME.Value = command.Parameters["@FIRSTNAME"].Value.ToString();
                    this._customModel.MIDDLENAME.Value = command.Parameters["@MIDDLENAME"].Value.ToString();
                    this._customModel.NAME.Value = "Incoming constituent data";

                    //_customModel.BATCHCREATEDTEXT.Value = string.Format("Batch: {0}; Created: {1}", command.Parameters["@BATCHNUMBER"].Value.ToString(), DateTime.Parse(command.Parameters["@CREATEDON"].Value.ToString()).ToShortDateString());

                    //if (!string.IsNullOrEmpty(command.Parameters["@SUFFIXCODEID"].Value.ToString()))
                    //    this._customModel.SUFFIXCODEID.Value =
                    //        Guid.Parse(command.Parameters["@SUFFIXCODEID"].Value.ToString());

                    //if (this._customModel.SUFFIXCODEID.HasValue())
                    //    _customModel.SUFFIX.Value = this.SUFFIXCODEID.GetDescription();
                    //else
                    //    _customModel.SUFFIX.Value = string.Empty;

                    //if (!string.IsNullOrEmpty(command.Parameters["@TITLECODEID"].Value.ToString()))
                    //    this._customModel.TITLECODEID.Value =
                    //        Guid.Parse(command.Parameters["@TITLECODEID"].Value.ToString());
                    //if (this._customModel.TITLECODEID.HasValue())
                    //    _customModel.TITLE.Value = _customModel.TITLECODEID.GetDescription();
                    //else
                    //    _customModel.TITLE.Value = string.Empty;

                    //if (command.Parameters["@BIRTHDATE"].Value.ToString() != "00000000")
                    //    _customModel.BIRTHDATE.Value = FuzzyDate.Parse(command.Parameters["@BIRTHDATE"].Value.ToString());

                    _customModel.ADDRESS_ADDRESSBLOCK.Value =
                        command.Parameters["@ADDRESS_ADDRESSBLOCK"].Value.ToString();
                    if (!string.IsNullOrEmpty(command.Parameters["@ADDRESS_TYPECODEID"].Value.ToString()))
                    {
                        this._customModel.ADDRESS_ADDRESSTYPECODEID.Value =
                           Guid.Parse(command.Parameters["@ADDRESS_TYPECODEID"].Value.ToString());
                        this._customModel.ADDRESS_ADDRESSTYPE.Value = this._customModel.ADDRESS_ADDRESSTYPECODEID.GetDescription();
                    }

                    this._customModel.ADDRESS_CITY.Value = command.Parameters["@ADDRESS_CITY"].Value.ToString();
                    if (!string.IsNullOrEmpty(command.Parameters["@ADDRESS_STATEID"].Value.ToString()))
                        this._customModel.ADDRESS_STATEID.Value =
                            Guid.Parse(command.Parameters["@ADDRESS_STATEID"].Value.ToString());
                    if (this._customModel.ADDRESS_STATEID.HasValue())
                        _customModel.ADDRESS_STATE.Value = this._customModel.ADDRESS_STATEID.GetCurrentLabel();
                    else
                        _customModel.ADDRESS_STATE.Value = string.Empty;
                    this._customModel.ADDRESS_POSTCODE.Value = command.Parameters["@ADDRESS_POSTCODE"].Value.ToString();

                    if (!string.IsNullOrEmpty(command.Parameters["@PHONETYPECODEID"].Value.ToString()))
                    {
                        this._customModel.PHONETYPECODEID.Value =
                            Guid.Parse(command.Parameters["@PHONETYPECODEID"].Value.ToString());
                    }

                    if (this._customModel.PHONETYPECODEID.HasValue())
                        _customModel.PHONETYPE.Value = this._customModel.PHONETYPECODEID.GetDescription();

                    _customModel.PHONENUMBER.Value = command.Parameters["@PHONENUMBER"].Value.ToString();
                    _customModel.EMAILADDRESS.Value = command.Parameters["@EMAILADDRESS"].Value.ToString();
                    if (!string.IsNullOrEmpty(command.Parameters["@EMAILADDRESSTYPECODEID"].Value.ToString()))
                        this._customModel.EMAILADDRESSTYPECODEID.Value =
                            Guid.Parse(command.Parameters["@EMAILADDRESSTYPECODEID"].Value.ToString());

                    if (this._customModel.EMAILADDRESSTYPECODEID.HasValue())
                        this._customModel.EMAILADDRESSTYPE.Value = this._customModel.EMAILADDRESSTYPECODEID.GetDescription();

                    //TODO: CONSTITUENTCIES
                    //_customModel.CONSTITUENCIES.Value = command.Parameters["@CONSTITUENCYCODEID"].Value;

                    //if (!string.IsNullOrEmpty(command.Parameters["@BATCHID"].Value.ToString()))
                    //    this._customModel.BATCHID.Value = Guid.Parse(command.Parameters["@BATCHID"].Value.ToString());
                    //_customModel.BATCHTYPE.Value = (DuplicateResolutionUIModel.BATCHTYPES)int.Parse(command.Parameters["@BATCHTYPE"].Value.ToString());

                    //_customModel.NAMECODE.Value = (DuplicateResolutionUIModel.NAMECODES)int.Parse(command.Parameters["@NAMECODE"].Value.ToString());
                    //_customModel.SIMILARADDRESSCODE.Value = (DuplicateResolutionUIModel.SIMILARADDRESSCODES)int.Parse(command.Parameters["@SIMILARADDRESSCODE"].Value.ToString());
                    //_customModel.UNSIMILARADDRESSCODE.Value = (DuplicateResolutionUIModel.UNSIMILARADDRESSCODES)int.Parse(command.Parameters["@UNSIMILARADDRESSCODE"].Value.ToString());
                    //_customModel.NEWADDRESSPRIMARYCODE.Value = (DuplicateResolutionUIModel.NEWADDRESSPRIMARYCODES)int.Parse(command.Parameters["@NEWADDRESSPRIMARYCODE"].Value.ToString());
                    //_customModel.DIFFERENTPHONECODE.Value = (DuplicateResolutionUIModel.DIFFERENTPHONECODES)int.Parse(command.Parameters["@DIFFERENTPHONECODE"].Value.ToString());
                    //_customModel.NEWPHONEPRIMARYCODE.Value = (DuplicateResolutionUIModel.NEWPHONEPRIMARYCODES)int.Parse(command.Parameters["@NEWPHONEPRIMARYCODE"].Value.ToString());
                    //_customModel.DIFFERENTEMAILCODE.Value = (DuplicateResolutionUIModel.DIFFERENTEMAILCODES)int.Parse(command.Parameters["@DIFFERENTEMAILCODE"].Value.ToString());
                    //_customModel.NEWEMAILPRIMARYCODE.Value = (DuplicateResolutionUIModel.NEWEMAILPRIMARYCODES)int.Parse(command.Parameters["@NEWEMAILPRIMARYCODE"].Value.ToString());
                    //_customModel.BIRTHDATERULECODE.Value = (DuplicateResolutionUIModel.BIRTHDATERULECODES)int.Parse(command.Parameters["@BIRTHDATERULECODE"].Value.ToString());
                    //if (_customModel.AUTOMATCHRECORDID.HasValue())
                    //{
                    //    //dataFormLoadReply.DataFormItem.TryGetValue("ADDRESSISPRIMARY", ref this._incomingAddressPrimary);
                    //    //dataFormLoadReply.DataFormItem.TryGetValue("PHONEISPRIMARY", ref this._incomingPhonePrimary);
                    //    //dataFormLoadReply.DataFormItem.TryGetValue("EMAILISPRIMARY", ref this._incomingEmailPrimary);
                    //}

                    //_customModel.GENDERCODE.Value = (DuplicateResolutionUIModel.GENDERCODES)int.Parse(command.Parameters["@GENDERCODE"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@MARITALSTATUSCODEID"].Value.ToString()))
                    //    this._customModel.MARITALSTATUSCODEID.Value = Guid.Parse(command.Parameters["@MARITALSTATUSCODEID"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@GIVESANONYMOUSLY"].Value.ToString()))
                    //    _customModel.GIVESANONYMOUSLY.Value = bool.Parse(command.Parameters["@GIVESANONYMOUSLY"].Value.ToString());

                    //_customModel.NICKNAME.Value = command.Parameters["@NICKNAME"].Value.ToString();
                    //_customModel.MAIDENNAME.Value = command.Parameters["@MAIDENNAME"].Value.ToString();
                    //_customModel.DECEASED.Value = bool.Parse(command.Parameters["@DECEASED"].Value.ToString());

                    //if (!string.IsNullOrEmpty(command.Parameters["@DECEASEDDATE"].Value.ToString()) &&
                    //    command.Parameters["@DECEASEDDATE"].Value.ToString() != "00000000")
                    //    _customModel.DECEASEDDATE.Value = FuzzyDate.Parse(command.Parameters["@DECEASEDDATE"].Value.ToString());
                    //_customModel.WEBADDRESS.Value = command.Parameters["@WEBADDRESS"].Value.ToString();

                    //if (!string.IsNullOrEmpty(command.Parameters["@ADDRESSHISTORICALSTARTDATE"].Value.ToString()))
                    //    _customModel.ADDRESSHISTORICALSTARTDATE.Value = DateTime.Parse(command.Parameters["@ADDRESSHISTORICALSTARTDATE"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@ADDRESSHISTORICALENDDATE"].Value.ToString()))
                    //    _customModel.ADDRESSHISTORICALENDDATE.Value = DateTime.Parse(command.Parameters["@ADDRESSHISTORICALENDDATE"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@ADDRESSDONOTMAIL"].Value.ToString()))
                    //    _customModel.ADDRESS_DONOTMAIL.Value = bool.Parse(command.Parameters["@ADDRESSDONOTMAIL"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@ADDRESSDONOTMAILREASONCODEID"].Value.ToString()))
                    //    this._customModel.ADDRESSDONOTMAILREASONCODEID.Value = Guid.Parse(command.Parameters["@ADDRESSDONOTMAILREASONCODEID"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@ADDRESSSTARTDATE"].Value.ToString()) &&
                    //    command.Parameters["@ADDRESSSTARTDATE"].Value.ToString() != "0000")
                    //    _customModel.ADDRESSSTARTDATE.Value = MonthDay.Parse(command.Parameters["@ADDRESSSTARTDATE"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@ADDRESSENDDATE"].Value.ToString()) &&
                    //    command.Parameters["@ADDRESSENDDATE"].Value.ToString() != "0000")
                    //    _customModel.ADDRESSENDDATE.Value = MonthDay.Parse(command.Parameters["@ADDRESSENDDATE"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@ADDRESSINFOSOURCECODEID"].Value.ToString()))
                    //    this._customModel.ADDRESSINFOSOURCECODEID.Value = Guid.Parse(command.Parameters["@ADDRESSINFOSOURCECODEID"].Value.ToString());

                    //_customModel.ADDRESSINFOSOURCECOMMENTS.Value = command.Parameters["@ADDRESSINFOSOURCECOMMENTS"].Value.ToString();
                    //_customModel.ADDRESSDPC.Value = command.Parameters["@ADDRESSDPC"].Value.ToString();
                    //_customModel.ADDRESSCART.Value = command.Parameters["@ADDRESSCART"].Value.ToString();
                    //_customModel.ADDRESSLOT.Value = command.Parameters["@ADDRESSLOT"].Value.ToString();

                    //if (!string.IsNullOrEmpty(command.Parameters["@ADDRESSCOUNTYCODEID"].Value.ToString()))
                    //    this._customModel.ADDRESSCOUNTYCODEID.Value = Guid.Parse(command.Parameters["@ADDRESSCOUNTYCODEID"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@ADDRESSCONGRESSIONALDISTRICTCODEID"].Value.ToString()))
                    //    this._customModel.ADDRESSCONGRESSIONALDISTRICTCODEID.Value = Guid.Parse(command.Parameters["@ADDRESSCONGRESSIONALDISTRICTCODEID"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@ADDRESSSTATEHOUSEDISTRICTCODEID"].Value.ToString()))
                    //    this._customModel.ADDRESSSTATEHOUSEDISTRICTCODEID.Value = Guid.Parse(command.Parameters["@ADDRESSSTATEHOUSEDISTRICTCODEID"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@ADDRESSSTATESENATEDISTRICTCODEID"].Value.ToString()))
                    //    this._customModel.ADDRESSSTATESENATEDISTRICTCODEID.Value = Guid.Parse(command.Parameters["@ADDRESSSTATESENATEDISTRICTCODEID"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@ADDRESSLOCALPRECINCTCODEID"].Value.ToString()))
                    //    this._customModel.ADDRESSLOCALPRECINCTCODEID.Value = Guid.Parse(command.Parameters["@ADDRESSLOCALPRECINCTCODEID"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@ADDRESSCERTIFICATIONDATA"].Value.ToString()))
                    //    _customModel.ADDRESSCERTIFICATIONDATA.Value = int.Parse(command.Parameters["@ADDRESSCERTIFICATIONDATA"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@ADDRESSLASTVALIDATIONATTEMPTDATE"].Value.ToString()))
                    //    _customModel.ADDRESSLASTVALIDATIONATTEMPTDATE.Value = DateTime.Parse(command.Parameters["@ADDRESSLASTVALIDATIONATTEMPTDATE"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@ADDRESSOMITFROMVALIDATION"].Value.ToString()))
                    //    _customModel.ADDRESSOMITFROMVALIDATION.Value = bool.Parse(command.Parameters["@ADDRESSOMITFROMVALIDATION"].Value.ToString());
                    //_customModel.ADDRESSVALIDATIONMESSAGE.Value = command.Parameters["@ADDRESSVALIDATIONMESSAGE"].Value.ToString();
                    //if (!string.IsNullOrEmpty(command.Parameters["@PHONECOUNTRYID"].Value.ToString()))
                    //    this._customModel.PHONECOUNTRYID.Value = Guid.Parse(command.Parameters["@PHONECOUNTRYID"].Value.ToString());
                    //_customModel.PHONEDONOTCALL.Value = false;
                    //if (!string.IsNullOrEmpty(command.Parameters["@PHONESEASONALSTARTDATE"].Value.ToString()) &&
                    //    command.Parameters["@PHONESEASONALSTARTDATE"].Value.ToString() != "0000")
                    //    _customModel.PHONESEASONALSTARTDATE.Value = MonthDay.Parse(command.Parameters["@PHONESEASONALSTARTDATE"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@PHONESEASONALENDDATE"].Value.ToString()) &&
                    //    command.Parameters["@PHONESEASONALENDDATE"].Value.ToString() != "0000")
                    //    _customModel.PHONESEASONALENDDATE.Value = MonthDay.Parse(command.Parameters["@PHONESEASONALENDDATE"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@PHONESTARTTIME"].Value.ToString()) &&
                    //    command.Parameters["@PHONESTARTTIME"].Value.ToString() != "0000")
                    //    _customModel.PHONESTARTTIME.Value = HourMinute.Parse(command.Parameters["@PHONESTARTTIME"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@PHONEENDTIME"].Value.ToString()) &&
                    //    command.Parameters["@PHONEENDTIME"].Value.ToString() != "0000")
                    //    _customModel.PHONEENDTIME.Value = HourMinute.Parse(command.Parameters["@PHONEENDTIME"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@PHONESTARTDATE"].Value.ToString()))
                    //    _customModel.PHONESTARTDATE.Value = DateTime.Parse(command.Parameters["@PHONESTARTDATE"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@PHONEENDDATE"].Value.ToString()))
                    //    _customModel.PHONEENDDATE.Value = DateTime.Parse(command.Parameters["@PHONEENDDATE"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@PHONEINFOSOURCECODEID"].Value.ToString()))
                    //    this._customModel.PHONEINFOSOURCECODEID.Value = Guid.Parse(command.Parameters["@PHONEINFOSOURCECODEID"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@EMAILADDRESSDONOTEMAIL"].Value.ToString()))
                    //    _customModel.EMAILADDRESSDONOTEMAIL.Value = bool.Parse(command.Parameters["@EMAILADDRESSDONOTEMAIL"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@EMAILADDRESSSTARTDATE"].Value.ToString()))
                    //    _customModel.EMAILADDRESSSTARTDATE.Value = DateTime.Parse(command.Parameters["@EMAILADDRESSSTARTDATE"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@EMAILADDRESSENDDATE"].Value.ToString()))
                    //    _customModel.@EMAILADDRESSENDDATE.Value = DateTime.Parse(command.Parameters["@EMAILADDRESSENDDATE"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@EMAILADDRESSINFOSOURCECODEID"].Value.ToString()))
                    //    this._customModel.EMAILADDRESSINFOSOURCECODEID.Value = Guid.Parse(command.Parameters["@EMAILADDRESSINFOSOURCECODEID"].Value.ToString());
                    //if (!string.IsNullOrEmpty(command.Parameters["@DECEASED"].Value.ToString()))
                    //    _customModel.DECEASED.Value = bool.Parse(command.Parameters["@DECEASED"].Value.ToString());
                }
            }
        }
        #endregion

    }
}
