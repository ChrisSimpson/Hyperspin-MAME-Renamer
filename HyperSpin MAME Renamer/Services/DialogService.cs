using Ookii.Dialogs.Wpf;
using Renamer.Properties;
using System;
using System.ComponentModel;
using System.Windows;

namespace Renamer.Services
{
    /// <summary>
    /// Dialog service
    /// </summary>
    public class DialogService : IDialogService, IDisposable
    {
        #region Private Fields

        private ProgressDialog _progressDialog;

        private Action _progressDialogAction;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Dispose of resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Report progress
        /// </summary>
        /// <param name="percentage">The percentage of work that has been completed</param>
        /// <param name="text">The text to be displayed on the progress dialog</param>
        public void ReportProgress(int percentage, string text)
        {
            _progressDialog.ReportProgress(percentage, null, text);
        }

        /// <summary>
        /// Show a confirmation dialog
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="text">The text to be displayed on the dialog</param>
        /// <returns>True if Yes was selected, false if not</returns>
        public bool ShowConfirmationDialog(string title, string text)
        {
            return MessageBox.Show(text, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Show an error message dialog
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="text">The text to be displayed on the dialog</param>
        public void ShowErrorMessageDialog(string title, string text)
        {
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Show an exception dialog
        /// </summary>
        /// <param name="exception">The exception that occurred</param>
        public void ShowExceptionDialog(Exception exception)
        {
            if (exception != null)
            {
                using (var dialog = new TaskDialog())
                {
                    dialog.WindowTitle = Resources.UnhandledException;
                    dialog.MainInstruction = Resources.UnhandledExceptionInstruction;
                    dialog.Content = exception.Message;
                    dialog.ExpandedInformation = exception.StackTrace;
                    dialog.MainIcon = TaskDialogIcon.Error;
                    dialog.Buttons.Add(new TaskDialogButton(ButtonType.Ok));
                    dialog.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Show the folder browser dialog
        /// </summary>
        /// <returns></returns>
        public string ShowFolderBrowserDialog()
        {
            var dialog = new VistaFolderBrowserDialog();
            return dialog.ShowDialog() ?? false ? dialog.SelectedPath : null;
        }

        /// <summary>
        /// Show an error message dialog
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="text">The text to be displayed on the dialog</param>
        public void ShowInformationDialog(string title, string text)
        {
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Show the open file dialog
        /// </summary>
        /// <param name="filter">The filter to be applied to the dialog</param>
        /// <returns>The name of the selected file</returns>
        public string ShowOpenFileDialog(string filter)
        {
            var dialog = new VistaOpenFileDialog()
            {
                Filter = filter
            };

            return dialog.ShowDialog() ?? false ? dialog.FileName : null;
        }

        /// <summary>
        /// Show the progress dialog
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="text">The text to be displayed on the dialog</param>
        /// <param name="action">The action to be performed</param>
        public void ShowProgressDialog(string title, string text, Action action)
        {
            if (_progressDialog == null)
            {
                _progressDialog = new ProgressDialog();
                _progressDialog.ShowCancelButton = false;
                _progressDialog.ShowTimeRemaining = true;
                _progressDialog.MinimizeBox = false;
                _progressDialog.DoWork += ProgressDialog_DoWork;
            }

            _progressDialog.WindowTitle = title;
            _progressDialog.Text = text;
            _progressDialogAction = action;
            _progressDialog.ShowDialog();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Dispose of resources
        /// </summary>
        /// <param name="disposing">Whether to dispose of managed resources or not</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _progressDialog != null)
            {
                _progressDialog.Dispose();
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Perform the progress dialog's work
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void ProgressDialog_DoWork(object sender, DoWorkEventArgs e)
        {
            _progressDialogAction();
        }

        #endregion Private Methods
    }
}