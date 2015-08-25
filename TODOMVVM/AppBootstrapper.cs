using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using TODOMVVM.Infrastructure;
using TODOMVVM.ViewModels;

namespace TODOMVVM {
    public class AppBootstrapper : BootstrapperBase {
        SimpleContainer _container;

        public AppBootstrapper() {
            Initialize();
        }

        protected override void Configure() {
            var currentParser = Parser.CreateTrigger;
            Parser.CreateTrigger = (target, triggerText) => ShortcutParser.CanParse(triggerText) ? ShortcutParser.CreateTrigger(triggerText) : currentParser(target, triggerText);

            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();

            _container.Singleton<MainWindowViewModel>();
        }

        protected override object GetInstance(Type service, string key) {
            var instance = _container.GetInstance(service, key);
            if (instance != null)
                return instance;

            throw new InvalidOperationException("Could not locate any instances.");
        }
        protected override IEnumerable<object> GetAllInstances(Type service) {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance) {
            _container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e) {
            DisplayRootViewFor<MainWindowViewModel>();
        }
    }
}
