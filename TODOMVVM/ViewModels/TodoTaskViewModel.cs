using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TODOMVVM.Infrastructure;
using System.Windows.Input;
using System.Diagnostics;

namespace TODOMVVM.ViewModels {
	public class TodoTaskViewModel : PropertyChangedBase {
		private readonly IEventAggregator _eventAggregator;

		public bool IsCompleted {
			get { return _isCompleted; }
			set {
				_isCompleted = value;
				NotifyOfPropertyChange("IsCompleted");
			}
		} bool _isCompleted;

		public bool IsInEditMode {
			get { return _isInEditMode; }
			set { _isInEditMode = value; NotifyOfPropertyChange("IsInEditMode"); }
		}
		bool _isInEditMode;

		public string TaskText {
			get { return _taskText; }
			set { _taskText = value; NotifyOfPropertyChange("TaskText"); }
		} string _taskText;

		public string NewTaskText {
			get { return _newTaskText; }
			set { _newTaskText = value; NotifyOfPropertyChange("NewTaskText"); }
		} string _newTaskText;

		public TodoTaskViewModel(IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;

			var currentParser = Parser.CreateTrigger;
			Parser.CreateTrigger = (target, triggerText) => ShortcutParser.CanParse(triggerText) ? ShortcutParser.CreateTrigger(triggerText) : currentParser(target, triggerText);
		}

        public void InformTaskCompleted() {
            _eventAggregator.PublishOnUIThread(new TaskCompletedChangedMessage ());
        }

		public void DeleteTask() {
			_eventAggregator.PublishOnUIThread(new TaskDeleteMessage { task = this });
        }

		public void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
			if(e.ClickCount == 2) {
				IsInEditMode = true;
				NewTaskText = TaskText;
			}
		}

		public void SaveChanges() {
			NewTaskText = NewTaskText.Trim();
			if (string.IsNullOrEmpty(NewTaskText))
				return;

			TaskText = NewTaskText;
			IsInEditMode = false;
		}

		public void DiscardChanges() {
			NewTaskText = string.Empty;
			IsInEditMode = false;
        }
    }
}
