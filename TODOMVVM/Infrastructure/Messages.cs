using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODOMVVM.ViewModels;

namespace TODOMVVM.Infrastructure {
    public class TaskCompletedChangedMessage {
    }

	public class TaskDeleteMessage {
		public TodoTaskViewModel task { get; set; }
	}
}
