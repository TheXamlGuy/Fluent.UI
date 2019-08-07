using System.Windows;

namespace Fluent.UI.Controls
{
    public abstract class StateTriggerBase : DependencyObject
    {
        protected void SetActive(bool isActive)
        {
            IsTriggerActive = isActive;
            Owner?.SetActive(IsTriggerActive);
        }

        internal bool IsTriggerActive { get; private set; }

        internal VisualStateTrigger Owner { get; set; }
    }
}