using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UM.CustomFx.GratefulPatient.Models
{
    public interface IUMHSDataRepo
    {
        string GetPatientName(Guid constituentID);
        string GetIncomingConstituentName(Guid batchRevenueID);

        Guid? GetDepartment(Guid referrerID);
        Guid? GetSubDepartment(Guid referrerID, Guid departmentID);

    }
}
