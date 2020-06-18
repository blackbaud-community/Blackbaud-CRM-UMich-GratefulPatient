using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UM.CustomFx.GratefulPatient.UIModel.Helpers
{
    /// <summary>
    /// This class is used to do common functions shared by UIModels and classes
    /// </summary>
    public class Common
    {
        public static int CalculateAge(Blackbaud.AppFx.FuzzyDate birthDay)
        {
            //fuzzy date makes this harder than it should be, consider using Date for UI Field
            DateTime today = DateTime.Now;

            int age = today.Year - birthDay.Year;
            if (birthDay.Month > today.Month)
            {
                age--;
            }
            else if ((birthDay.Month == today.Month) && (birthDay.Day > today.Day))
            {
                age--;
            }
            return age;
        }

    }



}
