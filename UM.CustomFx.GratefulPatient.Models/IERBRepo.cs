using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UM.CustomFx.GratefulPatient.Models
{
    public interface IERBRepo
    {
        BATCHREVENUECONSTITUENT Load(Guid constituentID);

        int? FindFriend(Guid constituentID, Guid batchID);
    }
}
