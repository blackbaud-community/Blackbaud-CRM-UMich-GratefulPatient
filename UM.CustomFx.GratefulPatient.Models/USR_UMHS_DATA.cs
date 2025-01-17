//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UM.CustomFx.GratefulPatient.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class USR_UMHS_DATA
    {
        public System.Guid ID { get; set; }
        public System.Guid CONSTITUENTID { get; set; }
        public string MRN { get; set; }
        public string CPISEQUENCE { get; set; }
        public string FIRSTNAME { get; set; }
        public string MIDDLENAME { get; set; }
        public string NICKNAME { get; set; }
        public string KEYNAME { get; set; }
        public string BIRTHDATE { get; set; }
        public Nullable<int> AGE { get; set; }
        public Nullable<System.Guid> TITLECODEID { get; set; }
        public Nullable<System.Guid> TITLE2CODEID { get; set; }
        public Nullable<System.Guid> SUFFIXCODEID { get; set; }
        public Nullable<System.Guid> SUFFIX2CODEID { get; set; }
        public Nullable<System.Guid> INFOSOURCECODEID { get; set; }
        public byte GENDERCODE { get; set; }
        public string GENDER { get; set; }
        public System.Guid ADDEDBYID { get; set; }
        public System.Guid CHANGEDBYID { get; set; }
        public System.DateTime DATEADDED { get; set; }
        public System.DateTime DATECHANGED { get; set; }
        public byte[] TS { get; set; }
        public Nullable<long> TSLONG { get; set; }
        public bool ISDMINELIGIBLE { get; set; }
        public Nullable<System.Guid> DMINELIGIBLEREASONCODEID { get; set; }
        public Nullable<System.DateTime> UMHSDATA_LASTPROCDATE { get; set; }
    }
}
