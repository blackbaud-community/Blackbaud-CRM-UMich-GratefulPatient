using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UM.CustomFx.GratefulPatient.Models
{
    public class ConstituentExtRepository : IConstituentExtRepository
    {

        CRMEntities _crmEntities = new CRMEntities(ConnectionStringHelper.GetConnectionString(true));

        public ConstituentExtRepository() { }

        public Guid ChangeAgentID
        {
            get
            {
                ////get the change agent
                ObjectParameter ID = new ObjectParameter("ID", typeof(Guid));
                _crmEntities.USP_CHANGEAGENT_GETORCREATECHANGEAGENT(ID, "", "", "");

                var changeAgentID = System.Guid.Parse(ID.Value.ToString());

                return changeAgentID;
            }
        }

        public Int64 NextSequence
        {
            get
            {
                //long? mSequence = _crmEntities.USR_UMHS_CONSTITUENT_EXT.DefaultIfEmpty().Max(m => m == null ? 1 : m.MSEQUENCE + 1);
                Int64? mSequence = _crmEntities.USR_UMHS_FRIENDNUMBERS.DefaultIfEmpty().Max(m => m == null ? 1 : m.UMHSFRIENDNUMBER + 1);

                 //return " M" + mSequence.Value.ToString("D10");
                return mSequence.Value;
            }
        }

        public void Save(Guid constituentID, Int64 mSequence)
        {
            //USR_UMHS_CONSTITUENT_EXT c = new USR_UMHS_CONSTITUENT_EXT();
            USR_UMHS_FRIENDNUMBERS c = new USR_UMHS_FRIENDNUMBERS();
            c.ID = Guid.NewGuid();
            c.CONSTITUENTID=constituentID;
            c.UMHSFRIENDNUMBER = mSequence; 
            c.ADDEDBYID = ChangeAgentID;
            c.CHANGEDBYID = ChangeAgentID;
            c.DATEADDED = DateTime.Now;
            c.DATECHANGED = DateTime.Now;

            //_crmEntities.USR_UMHS_CONSTITUENT_EXT.Add(c);
            _crmEntities.USR_UMHS_FRIENDNUMBERS.Add(c);
            _crmEntities.SaveChangesAsync();
        }
    }
}
