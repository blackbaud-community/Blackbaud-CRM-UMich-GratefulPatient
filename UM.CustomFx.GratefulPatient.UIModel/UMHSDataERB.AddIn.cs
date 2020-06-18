using Blackbaud.AppFx.BatchUI;
using Blackbaud.AppFx.UIModeling.Core;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using Blackbaud.AppFx.Server;
using Blackbaud.AppFx.XmlTypes.DataForms;
using Microsoft.VisualBasic;
using UM.CustomFx.GratefulPatient.Models;
//using Blackbaud.CustomFx.Revenue.UIModel;

namespace UM.CustomFx.GratefulPatient.UIModel
{
    public sealed class UMHSDataERBBatchEntryHandler : BatchEntryHandler
    {

        private const string BUTTON_KEY = "FINDFRIENDBUTTONKEY";
        private const string BUTTON_KEY_SRCH = "SEARCHMIMED";
        private const string BUTTON_KEY_CON = "UPDATECONTACT";

        private const string IMAGE_KEY = "CATALOG:Blackbaud.AppFx.Platform.Catalog,Blackbaud.AppFx.Platform.Catalog.validate_16.png";
        private const string IMAGE_KEY_SECH = "CATALOG:UM.CustomFx.GratefulPatient.Catalog,UM.CustomFx.GratefulPatient.Catalog.Images.patient_16.png";
        private const string IMAGE_KEY_CON = "CATALOG:Blackbaud.AppFx.Platform.Catalog,Blackbaud.AppFx.Platform.Catalog.validate_16.png";


        private static readonly Guid MIMEDDataViewForm = new Guid("cba9128a-4b66-4e7f-9eda-83452cd77baf");
        private const string RECOGNITIONS_FIELDID = "RECOGNITIONS";
        private const string REVENUESTREAMS_FIELDID = "REVENUESTREAMS";

        private string editConstituentContext = string.Empty;

        //BatchShowCustomFormAction findFriendAction = null;
        BatchAction findFriendAction = null;
        private BatchShowSearchFormUIAction searchmimed = null;
        private BatchAction UpdateContactForFriend = null;

        //private UIAction msgstr = null;
        ERBRepo erb = new ERBRepo();
        
        public UMHSDataERBBatchEntryHandler()
        {
            this.DefineActions += UMHSDataERBBatchEntryHandler_DefineActions;
            this.ActionInvoked += UMHSDataERBBatchEntryHandler_ActionInvoked;
        }

        private void UMHSDataERBBatchEntryHandler_ActionInvoked(object sender, BatchActionEventArgs e)
        {
            BATCHREVENUECONSTITUENT brc = null;

            try
            {
                switch (e.InvokeArgs.OriginatingEventName)
                {

                    case BatchShowSearchFormUIAction.EVENT_SEARCH:

                        //prepopulate search with "newconstituent" sub table
                        var searchArgs = (SearchEventArgs) e.InvokeArgs.OriginatingEventArgs;

                        editConstituentContext = ((String[]) e.Model.Fields["EDITCONSTITUENTCONTEXT"].ValueObject
                            .ToString().Split('|'))[0];

                        brc = erb.Load(new Guid(editConstituentContext));

                        searchArgs.SearchModel.Fields["KEYNAME"].ValueObject = brc.KEYNAME;
                        searchArgs.SearchModel.Fields["FIRSTNAME"].ValueObject = brc.FIRSTNAME;
                        searchArgs.SearchModel.Fields["PATIENTSONLY"].ValueObject = false;

                        break;

                    case BatchActionBase.EVENT_INVOKEACTION:
                        //FindFriend

                        foreach (var item in this.ModelCollection())
                        {
                            switch (e.Action.Key)
                            {
                                case "FINDFRIENDBUTTONKEY":
                                    if (((SearchListField) item.Fields["CONSTITUENTLOOKUPID"]).SearchDisplayText
                                        .Length == 0)
                                    {
                                        editConstituentContext =
                                        ((String[]) item.Fields["EDITCONSTITUENTCONTEXT"].ValueObject.ToString()
                                            .Split('|'))[0].ToString();
                                        if (erb.FindFriend(new Guid(editConstituentContext), this.BatchID) > 0)
                                        {
                                            this.ClearAllRowMessages(); //first clear all rwo messages before adding again.
                                            this.AddFieldAnnotation(item.Fields["CONSTITUENTID"],
                                                AnnotationCategory.BusinessRuleError, "Potential MIMED constituent",
                                                false);
                                            this.SetRowError(item, "Potential MIMED constituent");
                                            item.RowMessageText.ValueDisplayStyle =
                                                ValueDisplayStyle.WarningImageAndText;
                                        }
                                    }
                                    break;
                                    ;
                                case "UPDATECONTACT":
                                    if (((SearchListField) item.Fields["CONSTITUENTLOOKUPID"]).SearchDisplayText
                                        .Length != 0)
                                        
                                    {

                                        if (null != item.Fields["REVENUEATTRIBUTE75F0FBA05FB0477FA4D3D610C1362050"].ValueObject &&
                                            item.Fields["REVENUEATTRIBUTE75F0FBA05FB0477FA4D3D610C1362050"].ValueObject
                                                .ToString() != "")
                                        {
                                            
                                                //grab the donor information saved in attribute field and parse it in to separate fields.
                                                string cid = item.Fields["CONSTITUENTLOOKUPID"].ValueObject.ToString();

                                                parsefieldstoUpdate(
                                                    item.Fields["REVENUEATTRIBUTE75F0FBA05FB0477FA4D3D610C1362050"]
                                                        .ValueObject.ToString(), cid);
                                            
                                        }
                                    }
                                    break;
                            }
                        }
                        if (e.Action.Key == "UPDATECONTACT")
                        {
                            ShowPrompt("Save contact information for friends is complete!",
                                "Save contact information for friends is complete!", UIPromptImageStyle.Information,
                                UIPromptButtonStyle.Ok);
                        }


                        break;
                    //If search action is clicked: 
                    case BatchShowSearchFormUIAction.EVENT_SEARCHITEMSELECTED:

                        SearchItemSelectedEventArgs args =
                            e.InvokeArgs.OriginatingEventArgs as SearchItemSelectedEventArgs;
                        args.SearchModel.QuickFindCriteriaFieldId =
                            GetValueTranslationFromFieldID(e.Model, "CONSTITUENTID", null);
                        args.SearchModel.Fields["PATIENTSONLY"].ValueObject = false;

                        string donorinfo =
                            e.Model.Fields["REVENUEATTRIBUTE75F0FBA05FB0477FA4D3D610C1362050"].ValueObject.ToString();

                        using (BatchGridRowUIModel.CreateChangeSuppressor(e.Model))
                        {
                            SwitchConstituentMIMED(this, e.Model, Guid.Parse(args.SelectedId), donorinfo);
                        }

                        break;
                }

            }

            catch (System.InvalidOperationException x)
            {
                throw new Exception(x + "The MIMED Constituent is already matched for this record! Please search for the record which does not have lookupid.");
            }

            catch (Exception x)
            {
                throw new Exception(x.ToString());
            }
           

        }
        

        //Function to switch the lookup id and constituent name as per search
        public static void SwitchConstituentMIMED(UMHSDataERBBatchEntryHandler handler, Blackbaud.AppFx.UIModeling.Core.UIModel batchModel, Guid MedConstituentId, string donorinfo)
        {
            DataFormLoadRequest reqlkpid = new DataFormLoadRequest
            {
                FormID = MIMEDDataViewForm,
                RecordID = MedConstituentId.ToString()
            };

            DataFormLoadReply replylkpid = null;
            try
            {
                replylkpid = ServiceMethods.DataFormLoad(reqlkpid, batchModel.RootUIModel().GetRequestContext());
            }
            catch (Exception ex)
            {
                throw ex; //nom nom nom            
            }

            //' Save the current constituent's name and guid and lookupid before we start changing anything
            Guid currentConstituentId = Guid.Parse(GetValueFromFieldID(batchModel, "CONSTITUENTID", null).ToString());
            string currentConstituentName = GetValueTranslationFromFieldID(batchModel, "CONSTITUENTID", null);
            string currentLookupId = GetValueTranslationFromFieldID(batchModel, "CONSTITUENTLOOKUPID", null);
            if (donorinfo.Length > 0)
            {
                if (donorinfo.Contains("N:"))
                {
                    string[] s = donorinfo.Split(char.Parse("|"));
                    foreach (string i in s)
                    {
                        if ((i.Contains("N:")))
                        {
                            donorinfo = i.Substring(4);
                        }
                    }
                }

            }

            //AndAlso replyname IsNot Nothing AndAlso replyname.PageData IsNot Nothing Then
            if (replylkpid != null && replylkpid.DataFormItem != null)
            {
                DataFormFieldValue dfv = null;
                //Dim dfn As DataFormFieldValue = Nothing
                string name = string.Empty;
                string lookupid = string.Empty;
                //If replyname.PageData.TryGetValue("NAME", dfn) AndAlso dfn IsNot Nothing AndAlso dfn.Value IsNot Nothing Then
                if ((donorinfo != null) || !string.IsNullOrEmpty(donorinfo))
                {
                    name = donorinfo;
                }
                //End If
                if (replylkpid.DataFormItem.TryGetValue("LOOKUPID", ref dfv) && dfv != null && dfv.Value != null)
                {
                    lookupid = dfv.Value.ToString();
                }

                //SET Constituent Name
                //' UIModel allows us to easily move values from the related constituent form to the batch row
                TrySetValueForFieldID(batchModel, "CONSTITUENTID", MedConstituentId, name);
                TrySetValueForFieldID(batchModel, "CONSTITUENTLOOKUPID", MedConstituentId, lookupid);

            }
            handler.UpdateContextWindow();

        }


        private void UMHSDataERBBatchEntryHandler_DefineActions(object sender, EventArgs e)
        {
            BatchActionRegion actionRegion = new BatchActionRegion("CUSTOM_TASK_REGION", "Custom");
            BatchActionGroup actionGroup = new BatchActionGroup("REVENUE_GROUP", "MIMED Data");

            findFriendAction = new BatchAction(BUTTON_KEY, "Find Friends", IMAGE_KEY);
            findFriendAction.Size = BatchActionSize.Small;
            findFriendAction.Enabled = true;

            //searchmimed = new BatchAction(BUTTON_KEY_SRCH, "Find Friends", IMAGE_KEY_SECH);
            searchmimed = new BatchShowSearchFormUIAction(BUTTON_KEY_SRCH, "MIMED Constituent Search", IMAGE_KEY_SECH,
                Guid.Parse("747530a1-be80-4054-a021-d2a599248261"));
           // searchArgs.SearchModel.Fields["PATIENTSONLY"].ValueObject = true;

            searchmimed.Size = BatchActionSize.Small;
            searchmimed.Enabled = true;

            UpdateContactForFriend = new BatchAction(BUTTON_KEY_CON, "Save Contact Info for Friends", IMAGE_KEY_CON);
            UpdateContactForFriend.Size = BatchActionSize.Small;
            UpdateContactForFriend.Enabled = true;


            actionGroup.Add(findFriendAction);
            actionGroup.Add(searchmimed);
            actionGroup.Add(UpdateContactForFriend);

            actionRegion.Add(actionGroup);
            this.Actions.Add(actionRegion);
        }

        private void parsefieldstoUpdate(string donorinformation, string cid)
        {
            string donorinfo = donorinformation;
            string[] donorinfoarr = donorinfo.Split(char.Parse("|"));
            // string namestr = null;
            string keyname = null;
            string firstname = null;
            string titlestr = null;
            string addressstr = null;
            string citystr = null;
            string statestr = null;
            string emailstr = null;
            string phonestr = null;
            string zipstr = null;
            string typestr = null;

            foreach (string s in donorinfoarr)
            {
                if (s.Contains("N:"))
                {
                    string[] namearr = s.Split(char.Parse(" ")); ;
                    titlestr = namearr[2];
                    firstname = namearr[3];
                    keyname = namearr[4];

                }
                else if (s.Contains("E:"))
                {
                    emailstr = s.Substring(3).Trim(char.Parse(" "));
                }
                else if (s.Contains("P:"))
                {
                    phonestr = s.Substring(3).Trim(char.Parse(" "));
                }
                else if (s.Contains("T:"))
                {
                    typestr = s.Substring(3).Trim(char.Parse(" "));
                }
                else if (s.Contains("A:"))
                {
                    string[] addressarr = null;
                    string[] stziparr = null;
                    //separate out the address string in to addressblock, city, state-zip
                    addressarr = s.Split(char.Parse(","));
                    addressstr = addressarr[0].Substring(3).Trim(char.Parse(" "));
                    citystr = addressarr[1].Trim(char.Parse(" "));
                    //separate out the state and zip
                    stziparr = addressarr[2].Split(char.Parse(" "));
                    statestr = stziparr[1].Trim(char.Parse(" "));
                    zipstr = stziparr[2].Trim(char.Parse(" "));
                }
            }

            //TODO: Update into related tables.
            updateDonorInformationinDARTproper(typestr, titlestr, firstname, keyname, addressstr, citystr, statestr, zipstr, phonestr, emailstr, cid);
        }

        private void updateDonorInformationinDARTproper(string typestr, string titlestr, string firstname, string keyname, string addressblock, string citystr,
            string statestr, string zipstr, string phonestr, string emailstr, string cid)
        {
            //TODO: save to related tables.
            //call stored procedures.

            SqlConnection conn = new SqlConnection(ConnectionStringHelper.GetConnectionString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("EXEC USR_USP_UMHS_ADDCONTACTINFORMATIONFORBBISMIMEDDONOR " +
                                            "@TITLESTR, @KEYNAME, @FIRSTNAME, @CONSTITUENTID, @TYPESTR, @COUNTRYID, @STATESTR, " +
                                            "@ADDRESSBLOCK, @CITY, @POSTCODE, @INFOSOURCECODEID, @INFOSOURCECOMMENTS, @NUMBER, @EMAILADDRESS", conn);
            cmd.Parameters.AddWithValue("@TITLESTR", titlestr);
            cmd.Parameters.AddWithValue("@KEYNAME", keyname);
            cmd.Parameters.AddWithValue("@FIRSTNAME", firstname);
            cmd.Parameters.AddWithValue("@CONSTITUENTID", cid);
            cmd.Parameters.AddWithValue("@TYPESTR", typestr);
            cmd.Parameters.AddWithValue("@COUNTRYID", "27cf0f7d-cdaf-448f-80de-e9196bf28d36"); //United States by default
            cmd.Parameters.AddWithValue("@STATESTR", statestr);
            cmd.Parameters.AddWithValue("@ADDRESSBLOCK", addressblock);
            cmd.Parameters.AddWithValue("@CITY", citystr);
            cmd.Parameters.AddWithValue("@POSTCODE", zipstr);
            cmd.Parameters.AddWithValue("@INFOSOURCECODEID", "ff298ab5-74c6-4c30-924d-e2271175d7a4"); //--Web-Self Reported,
            cmd.Parameters.AddWithValue("@INFOSOURCECOMMENTS", "");
            cmd.Parameters.AddWithValue("@NUMBER", phonestr);
            cmd.Parameters.AddWithValue("@EMAILADDRESS", emailstr);
            cmd.Parameters.AddWithValue("@STARTDATE",  DateTime.Now.Month + DateTime.Now.Day);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                //Console.WriteLine(e);
                throw new Exception(e.ToString());
            }
            catch (InvalidCastException e)
            {
                //Console.WriteLine(e);
                throw new Exception(e.ToString());
            }
            catch (IOException e)
            {
                //Console.WriteLine(e);
                throw new Exception(e.ToString());
            }
            catch (ObjectDisposedException e)
            {
                //Console.WriteLine(e);
                throw new Exception(e.ToString());
            }
            finally
            {
                conn.Close();
                
            }
        }

    }

}