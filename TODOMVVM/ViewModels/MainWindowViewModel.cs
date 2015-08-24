using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace TODOMVVM.ViewModels {
    public class MainWindowViewModel : Screen {
        public override string DisplayName { get { return "TODOMVVM"; } set { } }

        #region Public properties

        public string NewToDoText {
            get { return _newToDoText; }
            set {
                _newToDoText = value;
                NotifyOfPropertyChange("NewToDoText");
            }
        } private string _newToDoText;
        #endregion Public properties
    }
}
