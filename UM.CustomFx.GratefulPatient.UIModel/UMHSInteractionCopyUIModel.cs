using Blackbaud.AppFx.UIModeling.Core;
using System;

namespace UM.CustomFx.GratefulPatient.UIModel
{

    public partial class UMHSInteractionCopyUIModel
    {

        private void UMHSInteractionCopyUIModel_Loaded(object sender, Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs e)
        {
            this.SaveButtonCaption = "Copy";
            this.INTERACTIONS.AllowAdd = false;
        }

        #region "Event handlers"

        partial void OnCreated()
        {
            this.Loaded += UMHSInteractionCopyUIModel_Loaded;
            this.Saving += UMHSInteractionCopyUIModel_Saving;
            this.SELECTALL.InvokeAction += SELECTALL_InvokeAction;
            this.Validating += UMHSInteractionCopyUIModel_Validating;
        }

        private void UMHSInteractionCopyUIModel_Validating(object sender, ValidatingEventArgs e)
        {
            if (this.INTERACTIONS.Value.Count > 0)
            {
                var prompt = new UIPrompt();
                prompt.Text = "Copy iteraction complete";
                prompt.ButtonStyle = UIPromptButtonStyle.Ok;
                prompt.ImageStyle = UIPromptImageStyle.Information;
                this.Prompts.Add(prompt);
            }

        }

        private void SELECTALL_InvokeAction(object sender, Blackbaud.AppFx.UIModeling.Core.InvokeActionEventArgs e)
        {
            foreach(var item in this.INTERACTIONS.Value)
            {
                item.SELECTED.Value = true;
            }
        }

        private void UMHSInteractionCopyUIModel_Saving(object sender, Blackbaud.AppFx.UIModeling.Core.SavingEventArgs e)
        {

            for (int i = this.INTERACTIONS.Value.Count - 1; i >= 0; i--)
            {
                var x = this.INTERACTIONS.Value[i];

                if (x.SELECTED.Value == false)
                {
                    this.INTERACTIONS.Value.Remove(x);
                }
            }
        }
        #endregion
    }

    public partial class UMHSInteractionCopyINTERACTIONSUIModel
    {
        partial void OnCreated()
        {
            this.DATE.DBReadOnly = true;
        }
    }

}