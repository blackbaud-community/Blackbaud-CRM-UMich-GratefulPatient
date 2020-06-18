using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UM.CustomFx.GratefulPatient.Models
{
    public interface IConstituentExtRepository
    {
        Guid ChangeAgentID { get; }
        Int64 NextSequence { get; }

        void Save(Guid constituentID, Int64 mSequence);
    }
}
