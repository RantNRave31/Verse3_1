using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using GKYU.PresentationCoreLibrary.ViewModels;
using GKYU.PresentationLogicLibrary.ViewModels;

namespace GKYU.PresentationLogicLibrary.ViewModels
{
    public class TaskManagerViewModel
        : ViewModelBase
    {
        public abstract class Task
            : ViewModelBase
        {
            public class Immediate
                : Task
            {
                public Immediate(string displayName, ICommand command, object parameter)
                    : base(displayName, command, parameter)
                {

                }
                public override bool CanExecute()
                {
                    if (!_completed && Command.CanExecute(_parameter))
                        return true;
                    else
                        return false;
                }
            }
            public class Delay
            : Task
            {
                public readonly DateTime startTime;
                public TimeSpan TimeSpan { get; set; }
                public Delay(string displayName, ICommand command, object parameter, TimeSpan timeSpan)
                    : base(displayName, command, parameter)
                {
                    startTime = DateTime.Now;
                    TimeSpan = timeSpan;
                }
                public override bool CanExecute()
                {
                    if (!_completed && Command.CanExecute(_parameter) && TimeSpan <= DateTime.Now - startTime)
                        return true;
                    else
                        return false;
                }
            }
            public class Daily
                : Task
            {
                public TimeSpan ExecutionTime { get; set; }
                public Daily(string displayName, ICommand command, object parameter, TimeSpan executionTime)
                    : base(displayName, command, parameter)
                {
                    ExecutionTime = executionTime;
                }
                public override bool CanExecute()
                {
                    if (!_completed && Command.CanExecute(_parameter) && DateTime.Now > DateTime.Now.Date + ExecutionTime)
                        return true;
                    else
                        return false;
                }

            }
            protected bool _completed;
            public bool Completed
            {
                get
                {
                    return _completed;
                }
                set
                {
                    if (value == _completed) return;
                    _completed = value;
                    OnPropertyChanged("Completed");
                }
            }
            public ICommand Command { get; set; }
            protected object _parameter;
            public Task(string displayName, ICommand command, object parameter)
                : base(displayName)
            {
                Command = command;
                _parameter = parameter;
            }
            public virtual bool CanExecute()
            {
                return !_completed && Command.CanExecute(_parameter);
            }
            public virtual void Execute()
            {
                Command.Execute(_parameter);
                Completed = true;
            }
        }

        private List<Task> scheduledTasks = new List<Task>();
        private Queue<Task> queuedTasks = new Queue<Task>();
        private Timer timer1;
        protected bool _stop = false;
        public event Action<int, string> OnError;

        public TaskManagerViewModel(string displayName)
            : base(displayName)
        {

        }
        public void Schedule(Task task)
        {
            switch (task)
            {
                case Task.Immediate immediateTask:
                    queuedTasks.Enqueue(immediateTask);
                    break;
                case Task.Delay delayTask:
                    queuedTasks.Enqueue(delayTask);
                    break;
                default:
                    scheduledTasks.Add(task);
                    break;
            }
        }
        private void timer1_Tick(object state)
        {
            if (!_stop)
            {
                timer1.Change(Timeout.Infinite, Timeout.Infinite);
                try
                {
                    //Log("Tick");
                    if (queuedTasks.Count > 0)
                    {
                        Task task = queuedTasks.Peek();
                        if (task.CanExecute())
                        {
                            task.Execute();
                            queuedTasks.Dequeue();
                        }
                    }
                    else if (scheduledTasks.Count > 0)
                    {
                        foreach (Task task in scheduledTasks)
                        {
                            if (task.CanExecute())
                            {
                                task.Execute();
                            }
                        }
                    }
                }
                catch (Exception se)
                {
                    if (null != OnError)
                    {
                        OnError(-10, string.Format("ERROR:  Exception in TaskManager Timer {0}", se.ToString()));
                    }
                }
                finally
                {
                }
                timer1.Change(0, 100);
            }
        }
        public void Start()
        {
            timer1 = new Timer(timer1_Tick, null, 100, 100);
            _stop = false;
        }
        public void Stop()
        {
            _stop = true;
            timer1.Change(Timeout.Infinite, Timeout.Infinite);
            timer1.Dispose();
            timer1 = null;
        }
    }
}
