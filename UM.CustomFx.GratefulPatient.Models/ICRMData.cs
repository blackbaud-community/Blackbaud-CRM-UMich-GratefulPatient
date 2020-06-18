using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UM.CustomFx.GratefulPatient.Models
{
    public interface ICRMData
    {
        Guid GetRecordTypeID(string recordTypeName);
    }
}
