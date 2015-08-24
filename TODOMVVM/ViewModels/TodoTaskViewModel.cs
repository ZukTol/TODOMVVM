using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace TODOMVVM.ViewModels {
	public class TodoTaskViewModel : PropertyChangedBase {
		private readonly IEventAggregator _eventAggregator;

		public bool IsCompleted {
			get { return _isCompleted; }
			set { _isCompleted = value; NotifyOfPropertyChange("IsCompleted"); }
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

		public TodoTaskViewModel(IEventAggregator eventAggregator) {
			_eventAggregator = eventAggregator;
		}
	}
}
