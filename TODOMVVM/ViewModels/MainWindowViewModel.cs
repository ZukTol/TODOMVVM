﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TODOMVVM.Infrastructure;
using System.ComponentModel;
using System.Windows.Data;

namespace TODOMVVM.ViewModels {
    public class MainWindowViewModel : Screen, IHandle<TaskCompletedChangedMessage>, IHandle<TaskDeleteMessage> {
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

			TodoListView = CollectionViewSource.GetDefaultView(TodoList);

			ShowAllSelected = true;
		}

		public void AddTaskToList() {
			NewToDoText = NewToDoText.Trim();
            if (string.IsNullOrEmpty(NewToDoText))
				return;

			var task = new TodoTaskViewModel(_eventAggregator) { TaskText = NewToDoText, OldTaskText = NewToDoText, IsCompleted = false, IsInEditMode = false };

            TodoList.Add(task);

		    NewToDoText = string.Empty;
			UpdateLabels();
			IsMarkAllChecked = false;
        }

        public void DoClearCompleted() {
            for (int i = TodoList.Count - 1; i >= 0; i--) {
                if(TodoList[i].IsCompleted)
                    TodoList.RemoveAt(i);
            }
			UpdateLabels();
        }

        public void OnMarkAllClicked() {
            foreach (var task in TodoList) {
                task.IsCompleted = IsMarkAllChecked;
            }
			UpdateLabels();
        }

        public void Handle(TaskCompletedChangedMessage message) {
			UpdateLabels();

			IsMarkAllChecked = TodoList.All(t => t.IsCompleted);
        }

		#region Labels
		public string ItemsLeftText {
			get { return GetNumEnding(IncompletedCount, new[] { "item left", "items left", "items left" }); }
		}

		public int CompletedCount {
			get { return TodoList.Count(t => t.IsCompleted); }
		}

		public int IncompletedCount {
			get { return TodoList.Count(t => !t.IsCompleted); }
		}

		public bool HasTasks {
			get { return TodoList.Any(); }
		}

		public bool HasCompletedTasks {
			get { return TodoList.Any(t => t.IsCompleted); }
		}

		void UpdateLabels() {
			NotifyOfPropertyChange("CompletedCount");
			NotifyOfPropertyChange("IncompletedCount");
			NotifyOfPropertyChange("HasTasks");
			NotifyOfPropertyChange("HasCompletedTasks");
			NotifyOfPropertyChange("ItemsLeftText");

			TodoListView.Refresh();
		}
		#endregion Labels

		#region Filtering
		public bool ShowAllSelected {
			get { return _showAllSelected; }
			set {
				_showAllSelected = value;
				NotifyOfPropertyChange("ShowAllSelected");
				if (value) {
					TodoListView.Filter = null;
					TodoListView.Refresh();
				}
			}
		} bool _showAllSelected;

		public bool ShowActiveSelected {
			get { return _showActiveSelected; }
			set {
				_showActiveSelected = value;
				NotifyOfPropertyChange("ShowActiveSelected");
				if (value) {
					TodoListView.Filter = item => {
						var task = (TodoTaskViewModel)item;
						return !task.IsCompleted;
					};
					TodoListView.Refresh();
				}
			}
		}
		bool _showActiveSelected;

		public bool ShowCompletedSelected {
			get { return _showCompletedSelected; }
			set {
				_showCompletedSelected = value;
				NotifyOfPropertyChange("ShowCompletedSelected");
				if (value) {
					TodoListView.Filter = item => {
						var task = (TodoTaskViewModel)item;
						return task.IsCompleted;
					};
					TodoListView.Refresh();
				}
			}
		}
		bool _showCompletedSelected;

		public ICollectionView TodoListView {
			get { return _todoListView; }
			set { _todoListView = value; NotifyOfPropertyChange("TodoListView"); }
		} ICollectionView _todoListView;
		#endregion Filtering

		public void Handle(TaskDeleteMessage message) {
			if(message.task != null) {
				TodoList.Remove(message.task);
				UpdateLabels();
            }
		}

        public void EnterClicked() {
            var editedTask = TodoList.FirstOrDefault(t => t.IsInEditMode);
            if (editedTask == null) {
                AddTaskToList();
            }
            else {
                editedTask.SaveChanges();
            }
        }

        public void EscapeClicked() {
            var editedTask = TodoList.FirstOrDefault(t => t.IsInEditMode);
            if (null != editedTask) {
                editedTask.DiscardChanges();
            }
        }

        /// <summary>
        /// Функция возвращает окончание для множественного числа слова на основании числа и массива окончаний
        /// </summary>
        /// <param name="number">Число на основе которого нужно сформировать окончание</param>
        /// <param name="endings">Массив слов или окончаний для чисел (1, 4, 5), например['яблоко', 'яблока', 'яблок']</param>
        /// <returns></returns>
        public static string GetNumEnding(int number, string[] endings) {
            string strintEnding;
            number = number % 100;
            if (number >= 11 && number <= 19) {
                strintEnding = endings[2];
            }
            else {
                var i = number % 10;
                switch (i) {
                    case (1):
                        strintEnding = endings[0];
                        break;
                    case (2):
                    case (3):
                    case (4):
                        strintEnding = endings[1];
                        break;
                    default:
                        strintEnding = endings[2];
                        break;
                }
            }
            return strintEnding;
        }
    }
}
