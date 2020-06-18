using UM.CustomFx.GratefulPatient.Models;
using System.Linq;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity.Core.Objects;

namespace UM.CustomFx.GratefulPatient.UIModel
{
    public sealed class UMHSConstituentAddIn : Blackbaud.AppFx.UIModeling.Core.RootModelAddIn<Blackbaud.AppFx.Constituent.UIModel.Individual.IndividualSpouseBusinessAddFormUIModel>
    {
     
        ConstituentExtRepository _constituentExtRepo = new ConstituentExtRepository();

        public override void Init()
        {
            //This method is called when the UI model is created to allow any initialization to be performed.
            this.HostModel.LASTNAME.ValueChanged += LASTNAME_ValueChanged;
            this.HostModel.Saved += HostModel_Saved;
        }

        private void HostModel_Saved(object sender, Blackbaud.AppFx.UIModeling.Core.SavedEventArgs e)
        {
            try { 
                if (this.HostModel.LASTNAME.Value.ToUpper().Contains("MIMED FRIEND")){
                    _constituentExtRepo.Save(Guid.Parse(this.HostModel.RecordId), _constituentExtRepo.NextSequence);
                }
            }
            catch(Exception x)
            {
                throw new Exception(x.InnerException.ToString());
            }
        }

        private void LASTNAME_ValueChanged(object sender, Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs e)
        {
            try
            {
                if (e.NewValue.ToString().ToUpper() == "MIMED FRIEND")
                {
                    String keyname = " M" + _constituentExtRepo.NextSequence.ToString("D10");
                    this.HostModel.LASTNAME.Value += keyname;//_constituentExtRepo.NextSequence;
                }
            }
            catch (Exception x)
            {
                throw new Exception(x.InnerException.ToString());
            }
        }
    }
}
