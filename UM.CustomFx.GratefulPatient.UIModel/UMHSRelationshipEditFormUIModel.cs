using Blackbaud.AppFx.Constituent.UIModel;
using Blackbaud.AppFx.Server;
using Blackbaud.AppFx.UIModeling.Core;
using Blackbaud.AppFx.XmlTypes.DataForms;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace UM.CustomFx.GratefulPatient.UIModel
{

	public partial class UMHSRelationshipEditFormUIModel
	{

		private void UMHSRelationshipEditFormUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
		{
            this.RELATIONSHIPTYPECODEID.Caption = string.Format("{0} is the", this.PATIENTNAME.Value);
            this.RECIPROCALTYPECODEID.Caption = string.Format("{0} is the", this.RELATEDINDIVIDUAL.Value);
        }
        private void RECIPROCALUMHSID_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            this.RECIPROCALTYPECODEID.Caption = string.Format("{0} is the ", this.RELATEDINDIVIDUAL.Value);
            RefreshReciprocalRelationshipTypeID((SimpleDataListField)this.RECIPROCALTYPECODEID);
        }
        private void UpdateRelationshipNameReference()
        {
            this.RELATIONSHIPTYPECODEID.Caption = string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The Individual", new object[1]
            {
                (object) this.PATIENTNAME.Value
            });
        }

        private void UpdateReciprocalNameReference()
        {
            if (string.IsNullOrEmpty(RECIPROCALTYPECODEID.Caption))
            {
                //this._relatedName = Content.Individual_lower;
                this.RECIPROCALTYPECODEID.Caption = "Individual is the";
            }
            else
                this.RECIPROCALTYPECODEID.Caption = string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Individual is the", new object[1]
                {
          (object) RECIPROCALTYPECODEID.Caption
                });
            // this.SetNameRefrences(this.PATIENTNAME.Value, this.RECIPROCALTYPECODEID.Caption);
        }


        //private void _reciprocalUMHSid_ValueChanged(object sender, ValueChangedEventArgs e)
        //{
        //    if (this._relationshipsetid.Value != this.RECIPROCALUMHSID.Value)
        //    {
        //        this._relationshipsetid.Value = this.RECIPROCALUMHSID.Value;
        //        this._patientname.Value = this.RECIPROCALUMHSID.SearchDisplayText;
        //        this.UpdateReciprocalNameReference();
        //    }

        //    RefreshReciprocalRelationshipTypeID((SimpleDataListField)this.RECIPROCALTYPECODEID);
        //}


        public static void RefreshReciprocalRelationshipTypeID(SimpleDataListField reciprocalTypeCodeID, bool isLoading)
        {
            if (reciprocalTypeCodeID == null)
                return;
            reciprocalTypeCodeID.ResetDataSource();
            SimpleDataListItemCollection dataSource = reciprocalTypeCodeID.DataSource;
            if (isLoading)
                return;
            SimpleDataListItem simpleDataListItem = (SimpleDataListItem)null;
            if (dataSource != null && dataSource.Count > 0)
                simpleDataListItem = dataSource[0];
            if (simpleDataListItem != null && Operators.CompareString(simpleDataListItem.Description, "1", false) == 0)
                reciprocalTypeCodeID.ValueObject = RuntimeHelpers.GetObjectValue(simpleDataListItem.Value);
            else
                reciprocalTypeCodeID.ValueObject = (object)Guid.Empty;
        }

        public static void RefreshReciprocalRelationshipTypeID(SimpleDataListField reciprocalTypeCodeID)
        {
            RefreshReciprocalRelationshipTypeID(reciprocalTypeCodeID, false);
        }

        private void _relationshiptypecodeid_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            RefreshReciprocalRelationshipTypeID((SimpleDataListField)this.RECIPROCALTYPECODEID);
        }
        private void _reciprocaltypecodeid_ValueChanged(object sender, ValueChangedEventArgs e)
        {

        }
        private void RefreshAdditionalRelationships()
        {
            //this.RELATIONSHIPS.Value.Clear();
            DataListLoadRequest req = new DataListLoadRequest();
            req.DataListID = new Guid("9FB22DDF-1B9E-4FBF-875A-CB7E18CBB755");
            req.Parameters = new DataFormItem();
           // Guid guid1;
         //   Guid guid2;
            //if (this._constitHasHousehold)
            // {
            //    guid1 = new Guid(this.ContextRecordId);
            //     guid2 = this.RECIPROCALUMHSID.Value;
            // }
            //else
            //{
            //    guid1 = this.RELATEDINDIVIDUAL.Value;
            //    guid2 = new Guid(this.ContextRecordId);
            //}
            //req.Parameters.SetValue("HOUSEHOLDCONSTITUENTID", (object)guid1);
            //req.Parameters.SetValue("ADDITIONALCONSTITUENTID", (object)guid2);
            req.SecurityContext = this.GetRequestSecurityContext();
            DataListLoadReply dataListLoadReply = new AppFxWebService(this.GetRequestContext()).DataListLoad(req);
            if (dataListLoadReply == null || Enumerable.Count<DataListResultRow>((IEnumerable<DataListResultRow>)dataListLoadReply.Rows) <= 0)
                return;
            DataListResultRow[] dataListResultRowArray = dataListLoadReply.Rows;
            int index = 0;
            while (index < dataListResultRowArray.Length)
            {
                DataListResultRow dataListResultRow = dataListResultRowArray[index];
                UMHSRelationshipAddFormUIModel relationshipsuiModel = new UMHSRelationshipAddFormUIModel();
                Guid guid3 = new Guid(dataListResultRow.Values[0]);
                int num = Conversions.ToInteger(dataListResultRow.Values[1]);
                string str1 = dataListResultRow.Values[2];
                string str2 = dataListResultRow.Values[3];
                string str3 = dataListResultRow.Values[4];
                string str4 = dataListResultRow.Values[5];
                Guid relationshipTypeCodeID = dataListResultRow.Values[6] == null ? Guid.Empty : new Guid(dataListResultRow.Values[6]);
                Guid reciprocalTypeCodeID = dataListResultRow.Values[7] == null ? Guid.Empty : new Guid(dataListResultRow.Values[7]);
                //relationshipsuiModel.RECIPROCALCONSTITUENTID.Value = guid3;
                //relationshipsuiModel.RECIPROCALCONSTITUENTID.Value = guid2;
                relationshipsuiModel.RECIPROCALUMHSID.Value = guid3;
                //relationshipsuiModel.RECIPROCALUMHSID.Value = guid2;
                //relationshipsuiModel.UMHSTYPE.Value = num;
                if (string.IsNullOrEmpty(str4)) //**It shoudl be RECIPROCALPATIENTNAME
                    relationshipsuiModel.PATIENTNAME.Value = string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Individual is the", new object[2]
                    {
            (object) str1,
            (object) ConstituentHelper.MakeStringPossessive(str3)
                    });
                else
                    //shoudl be RECIPROCALPATIENTNAME instead of PATIENTNAME here
                    relationshipsuiModel.PATIENTNAME.Value = string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Individual is the", (object)str1, (object)ConstituentHelper.MakeStringPossessive(str2), (object)str4, (object)ConstituentHelper.MakeStringPossessive(str3));
                relationshipsuiModel.PATIENTNAME.Value = string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Individual is the", new object[1]
                {
          (object) str3
                });
                relationshipsuiModel.PATIENTNAME.Enabled = false;
                relationshipsuiModel.PATIENTNAME.Enabled = false; //should be RECIPROCALPATIENTNAME
                relationshipsuiModel.SetExistingRelationship(relationshipTypeCodeID, reciprocalTypeCodeID);
                // this.RELATIONSHIPS.Value.Add(relationshipsuiModel);
                checked { ++index; }
            }
        }
        private bool _isExistingRelationship;
        public void SetExistingRelationship(Guid relationshipTypeCodeID, Guid reciprocalTypeCodeID)
        {
            Guid guid = new Guid();
            if (relationshipTypeCodeID != guid && relationshipTypeCodeID != Guid.Empty || reciprocalTypeCodeID != guid && reciprocalTypeCodeID != Guid.Empty)
            {
                this._isExistingRelationship = true;
                this.RELATIONSHIPTYPECODEID.Value = relationshipTypeCodeID;
                this.RECIPROCALTYPECODEID.Value = reciprocalTypeCodeID;
            }
            this.RELATIONSHIPTYPECODEID.Enabled = !this._isExistingRelationship;
            this.RECIPROCALTYPECODEID.Enabled = !this._isExistingRelationship;
        }
        #region "Event handlers"

        partial void OnCreated()
		{
			this.Loaded += UMHSRelationshipEditFormUIModel_Loaded;
		}

#endregion

	}

}