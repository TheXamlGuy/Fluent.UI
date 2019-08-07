using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Fluent.UI.Controls
{
    public class CommandBar : AppBar
    {
        public static DependencyProperty PrimaryCommandsProperty =
            DependencyProperty.Register(nameof(PrimaryCommands),
                typeof(ObservableCollection<ICommandBarElement>), typeof(CommandBar),
                new PropertyMetadata(null));

        public static DependencyProperty SecondaryCommandsProperty =
            DependencyProperty.Register(nameof(SecondaryCommands),
                typeof(ObservableCollection<ICommandBarElement>), typeof(CommandBar),
                new PropertyMetadata(null));

        private ItemsControl _primaryItemsControl;

        public CommandBar()
        {
            DefaultStyleKey = typeof(CommandBar);
            PrimaryCommands = new ObservableCollection<ICommandBarElement>();
            SecondaryCommands = new ObservableCollection<ICommandBarElement>();
        }

        public ObservableCollection<ICommandBarElement> PrimaryCommands
        {
            get => (ObservableCollection<ICommandBarElement>)GetValue(PrimaryCommandsProperty);
            set => SetValue(PrimaryCommandsProperty, value);
        }

        public ObservableCollection<ICommandBarElement> SecondaryCommands
        {
            get => (ObservableCollection<ICommandBarElement>)GetValue(SecondaryCommandsProperty);
            set => SetValue(SecondaryCommandsProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PreparePrimaryCommands();
        }

        private void PreparePrimaryCommands()
        {
            _primaryItemsControl = GetTemplateChild("PrimaryItemsControl") as ItemsControl;
            if (_primaryItemsControl != null)
            {
                var primaryCommandBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath(PrimaryCommandsProperty),
                    Mode = BindingMode.TwoWay
                };

                BindingOperations.SetBinding(_primaryItemsControl, ItemsControl.ItemsSourceProperty, primaryCommandBinding);
            }
        }
    }
}
