using System;
using System.Windows.Input;

namespace gps_time_viewer
{
    internal class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action<object> execute) : base(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute) : base(execute, canExecute)
        {
        }
    }

    /// <summary>
    /// Relay a function with up to a single parameter from a Command to a function
    /// </summary>
    /// <typeparam name="T">Parameter Type</typeparam>
    internal class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        /// <summary>
        /// Create a new command that will always execute
        /// </summary>
        /// <param name="execute">Action to execute</param>
        public RelayCommand(Action<T> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Create a new command with conditional execution
        /// </summary>
        /// <param name="execute">Action to execute</param>
        /// <param name="canExecute">Predicate function that is evaluated before execution</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Determines if the command can be executed
        /// </summary>
        /// <param name="parameter">parameter for the predicate function</param>
        /// <returns>True if the command can execute; otherwise false</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke((T) parameter) ?? true;
        }

        /// <summary>
        /// Invoke the action
        /// </summary>
        /// <param name="parameter">optional parameter; can be <see langword="null" /></param>
        public void Execute(object parameter)
        {
            _execute((T) parameter);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}