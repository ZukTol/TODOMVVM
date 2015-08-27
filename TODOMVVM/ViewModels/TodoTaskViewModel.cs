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

		public string OldTaskText {
			get { return _oldTaskText; }
			set { _oldTaskText = value; NotifyOfPropertyChange("OldTaskText"); }
		} string _oldTaskText;

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

	    public void OnMouseDoubleClick(MouseButtonEventArgs e) {
            IsInEditMode = true;
            OldTaskText = TaskText;
	    }

        public void SaveChanges() {
            TaskText = TaskText.Trim();
            if (string.IsNullOrEmpty(TaskText)) {
                DeleteTask();
                return;
            }
				

			OldTaskText = TaskText;
			IsInEditMode = false;
		}

		public void DiscardChanges() {
		    TaskText = OldTaskText;
			IsInEditMode = false;
        }
    }
}
