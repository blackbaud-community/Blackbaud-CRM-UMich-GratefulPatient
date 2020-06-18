using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UM.CustomFx.GratefulPatient.Models
{
    public class UMHSDataRepo : IUMHSDataRepo
    {
        //create a new class repo for our umhs data
        UMHSDataEntities _umhsData = new UMHSDataEntities(ConnectionStringHelper.GetConnectionString("UMHSData"));

        /// <summary>
        /// Gets the incoming name from the import from the batch constituent update table
        /// </summary>
        /// <param name="batchRevenueID"></param>
        /// <returns></returns>
        public string GetIncomingConstituentName(Guid batchRevenueID)
        {
            string name = string.Empty;
            try
            {
                var result = (from c in _umhsData.BATCHCONSTITUENTUPDATEs
                              where c.ID == batchRevenueID
                              select c.FIRSTNAME + " " + c.KEYNAME);
                if (result.Any())
                {
                    name = result.First().ToString();
                }
                else
                {
                    result = (from c in _umhsData.BATCHREVENUECONSTITUENTs
                              where c.ID == batchRevenueID
                              select c.NAME);

                    if (result.Any())
                    {
                        name = result.First().ToString();
                    }
                }

            }
            catch(Exception x)
            {
                throw new Exception($"Error: {x.Message}{Environment.NewLine}Details:{x.InnerException}");
            }
            return name;
        }
        
        /// <summary>
        /// Pulls the patient name if it exits
        /// </summary>
        /// <param name="constituentID"></param>
        /// <returns></returns>
        public string GetPatientName(Guid constituentID)
        {
            string patientName = string.Empty;
            try
            {
                var result = (from p in _umhsData.USR_UMHS_DATA
                              where p.CONSTITUENTID == constituentID
                              select p.FIRSTNAME + " " + p.KEYNAME);
                if (result.Any())
                {
                    patientName = result.First().ToString();
                }
            }
            catch (Exception x)
            {
                throw new Exception($"Error: {x.Message}{Environment.NewLine}Details:{x.InnerException}");
            }
            return patientName;
        }

        public Guid? GetDepartment(Guid referrerID)
        {
            Guid? department = Guid.Empty;
            try
            {
                var result = (from r in _umhsData.USR_UMHS_PROVIDERS
                              where r.CONSTITUENTID == referrerID
                              select r.DEPARTMENTID);
                if (result.Any())
                {
                    department = result.First();
                }
            }
            catch (Exception x)
            {
                throw new Exception($"Error: {x.Message}{Environment.NewLine}Details:{x.InnerException}");
            }
            return department;

        }
        public Guid? GetSubDepartment(Guid referrerID, Guid departmentID)
        {
            Guid? subDepartment = Guid.Empty;
            try
            {
                var result = (from d in _umhsData.USR_UMHS_PROVIDERS
                              where d.DEPARTMENTID == departmentID && d.CONSTITUENTID == referrerID
                              select d.SUBDEPARTMENTID);
                if (result.Any())
                {
                    subDepartment = result.First();
                }
            }
            catch (Exception x)
            {
                throw new Exception($"Error: {x.Message}{Environment.NewLine}Details:{x.InnerException}");
            }
            return subDepartment;
        }
    }
}
