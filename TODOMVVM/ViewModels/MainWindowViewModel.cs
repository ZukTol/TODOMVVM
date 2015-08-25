using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace TODOMVVM.ViewModels {
    public class MainWindowViewModel : Screen {
		private readonly IEventAggregator _eventAggregator;
		public override string DisplayName { get { return "TODOMVVM"; } set { } }

        #region Public properties

        public string NewToDoText {
            get { return _newToDoText; }
            set {
                _newToDoText = value;
                NotifyOfPropertyChange("NewToDoText");
            }
        } private string _newToDoText;

		public bool IsMarkAllChecked {
			get { return _isMarkAllChecked; }
			set {
				_isMarkAllChecked = value;
				NotifyOfPropertyChange("IsMarkAllChecked");
				foreach(var task in TodoList) {
					task.IsCompleted = _isMarkAllChecked;
				}
			}
		} bool _isMarkAllChecked;

		public BindableCollection<TodoTaskViewModel> TodoList {
			get { return _todoList; }
			set {
				_todoList = value;
				NotifyOfPropertyChange("TodoList");
			}
		} private BindableCollection<TodoTaskViewModel> _todoList = new BindableCollection<TodoTaskViewModel>();

		#endregion Public properties

		public MainWindowViewModel(IEventAggregator eventAggregator) {
			_eventAggregator = eventAggregator;
			_eventAggregator.Subscribe(this);
		}

		public void AddTaskToList() {
			NewToDoText = NewToDoText.Trim();
            if (string.IsNullOrEmpty(NewToDoText))
				return;

			var task = new TodoTaskViewModel(_eventAggregator) { TaskText = NewToDoText, IsCompleted = false, IsInEditMode = false };

            TodoList.Add(task);

		    NewToDoText = string.Empty;
		}

        public void DoClearSelected() {
            for (int i = TodoList.Count - 1; i >= 0; i--) {
                if(TodoList[i].IsCompleted)
                    TodoList.RemoveAt(i);
            }
        }
    }
}
