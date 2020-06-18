using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UM.CustomFx.GratefulPatient.Models
{
    public class CRMDataRepo : ICRMData
    {

        CRMEntities _crmEntities = new CRMEntities(ConnectionStringHelper.GetConnectionString(true));

        public Guid GetRecordTypeID(string recordTypeName)
        {

            Guid id = (from r in _crmEntities.RECORDTYPEs
                       where r.NAME == recordTypeName
                       select r.ID).First();

            return id;
        }
    }
}
