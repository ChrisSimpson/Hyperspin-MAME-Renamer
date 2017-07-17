using System;
using System.Windows.Input;

namespace Renamer.Commands
{
    /// <summary>
    /// Relay command implementation
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Private Fields

        private readonly Func<bool> _canExecute;
        private readonly Action _execute;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">The action to be executed by the command</param>
        public RelayCommand(Action execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">The action to be executed by the command</param>
        /// <param name="canExecute">Function to determine if the command can be executed or not</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion Public Constructors

        #region Public Events

        /// <summary>
        /// Event fired when the can execute flag changes
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion Public Events

        #region Public Methods

        /// <summary>
        /// Check whether the command can be executed or not
        /// </summary>
        /// <param name="parameter">The parameter to be passed to the command</param>
        /// <returns>True if the command can be executed, false if not</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        /// <param name="parameter">The parameter to be passed to the command</param>
        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _execute();
            }
        }

        #endregion Public Methods
    }
}