using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace TODOMVVM.Infrastructure {
    public class InputBindingTrigger : TriggerBase<FrameworkElement>, ICommand {
        public static readonly DependencyProperty InputBindingProperty =
          DependencyProperty.Register("InputBinding", typeof(InputBinding)
            , typeof(InputBindingTrigger)
            , new UIPropertyMetadata(null));

        public InputBinding InputBinding {
            get { return (InputBinding)GetValue(InputBindingProperty); }
            set { SetValue(InputBindingProperty, value); }
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter) {
            // action is anyway blocked by Caliburn at the invoke level
            return true;
        }

        public void Execute(object parameter) {
            InvokeActions(parameter);
        }

        protected override void OnAttached() {
            if (InputBinding != null) {
                InputBinding.Command = this;
                AssociatedObject.Loaded += delegate {
                    var window = GetWindow(AssociatedObject);
                    if (window != null) window.InputBindings.Add(InputBinding);
                };
            }
            base.OnAttached();
        }

        private Window GetWindow(FrameworkElement frameworkElement) {
            if (frameworkElement != null) {
                if (frameworkElement is Window)
                    return frameworkElement as Window;

                var parent = frameworkElement.Parent as FrameworkElement;
                System.Diagnostics.Debug.Assert(parent != null);

                return GetWindow(parent);
            }
            else return null;
        }

    }
}
