using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TxtInterfaces.Framework
{
    class AsyncCommand<TIn, TOut> : ICommand, INotifyPropertyChanged
    {
        private readonly BackgroundWorker _backgroundWorker;
        private readonly Func<TIn, TOut> _execute;
        private readonly Action<TOut> _executed;
        private readonly Func<bool> _canExecute;

        private bool _isWorking;
        public bool IsWorking
        {
            get { return _isWorking; }
            set
            {
                if (value == _isWorking) return;
                _isWorking = value;
                RaisePropertyChanged("IsWorking");
            }
        }

        public AsyncCommand(Func<TIn, TOut> execute) : this(execute, null, null) { }
        public AsyncCommand(Func<TIn, TOut> execute, Action<TOut> executed) : this(execute, executed, null) { }
        public AsyncCommand(Func<TIn, TOut> execute, Action<TOut> executed, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
            _executed = executed;

            _backgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
            _backgroundWorker.DoWork += BackGroundWorkerDoWork;
            _backgroundWorker.RunWorkerCompleted += BackGroundWorkerRunWorkerCompleted;
        }

        public bool HasErrors { get; private set; }
        public string ErrorMessage { get; private set; }
        public object ErrorParameter { get; private set; }

        public object ExecuteParameter { get; private set; }

        #region ICommand Members
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            ExecuteParameter = parameter;

            HasErrors = false;
            ErrorMessage = "";

            if (_backgroundWorker.IsBusy) return;

            IsWorking = true;
            _backgroundWorker.RunWorkerAsync(parameter);
        }

        public void Cancel()
        {
            if (IsWorking == false) return;
            _backgroundWorker.CancelAsync();
            IsWorking = false;
        }

        public void SetError(string errorMessage, object extraParam = null)
        {
            this.HasErrors = true;
            this.ErrorMessage = errorMessage;
            this.ErrorParameter = extraParam;
            this.Cancel();
        }
        #endregion

        private void BackGroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = _execute((TIn)e.Argument);
        }

        private void BackGroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsWorking = false;

            if (HasErrors)
            {
                _executed?.Invoke(default(TOut));
                return;
            }
            if (e.Error == null)
            {
                HasErrors = false;
                ErrorMessage = string.Empty;
                _executed?.Invoke((TOut)e.Result);
            }
            else
            {
                HasErrors = true;
                ErrorMessage = e.Error.Message;
                _executed?.Invoke(default(TOut));
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler == null) return;

            handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
