using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UM.CustomFx.GratefulPatient.Models
{
    public class ERBRepo : IERBRepo
    {

        CRMEntities _crmEntities = new CRMEntities(ConnectionStringHelper.GetConnectionString(true));

        public BATCHREVENUECONSTITUENT Load(Guid constituentID)
        {

            BATCHREVENUECONSTITUENT result = (from c in _crmEntities.BATCHREVENUECONSTITUENTs
                                              where c.ID == constituentID
                                              select c).First();
            return result;
        }

        public int? FindFriend(Guid constituentID, Guid batchID)
        {
            return _crmEntities.USR_USP_UMHS_FINDFRIEND(constituentID, batchID).FirstOrDefault();
        }

    }
}
