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
    
    public partial class RECORDTYPE
    {
        public System.Guid ID { get; set; }
        public string NAME { get; set; }
        public string BASETABLENAME { get; set; }
        public bool HASRACSID { get; set; }
        public System.Guid ADDEDBYID { get; set; }
        public System.Guid CHANGEDBYID { get; set; }
        public System.DateTime DATEADDED { get; set; }
        public System.DateTime DATECHANGED { get; set; }
        public byte[] TS { get; set; }
        public Nullable<long> TSLONG { get; set; }
        public Nullable<System.Guid> DEFAULTSEARCHLISTID { get; set; }
    }
}
