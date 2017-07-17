using System;

namespace Renamer.Services
{
    /// <summary>
    /// Dialog service interface
    /// </summary>
    public interface IDialogService
    {
        #region Public Methods

        /// <summary>
        /// Report progress
        /// </summary>
        /// <param name="percentage">The percentage of work that has been completed</param>
        /// <param name="text">The text to be displayed on the progress dialog</param>
        void ReportProgress(int percentage, string text);

        /// <summary>
        /// Show a confirmation dialog
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="text">The text to be displayed on the dialog</param>
        /// <returns>True if Yes was selected, false if not</returns>
        bool ShowConfirmationDialog(string title, string text);

        /// <summary>
        /// Show an error message dialog
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="text">The text to be displayed on the dialog</param>
        void ShowErrorMessageDialog(string title, string text);

        /// <summary>
        /// Show an exception dialog
        /// </summary>
        /// <param name="exception">The exception that occurred</param>
        void ShowExceptionDialog(Exception exception);

        /// <summary>
        /// Show the folder browser dialog
        /// </summary>
        /// <returns></returns>
        string ShowFolderBrowserDialog();

        /// <summary>
        /// Show an error message dialog
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="text">The text to be displayed on the dialog</param>
        void ShowInformationDialog(string title, string text);

        /// <summary>
        /// Show the open file dialog
        /// </summary>
        /// <param name="filter">The filter to be applied to the dialog</param>
        /// <returns>The name of the selected file</returns>
        string ShowOpenFileDialog(string filter);

        /// <summary>
        /// Show the progress dialog
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="text">The text to be displayed on the dialog</param>
        /// <param name="action">The action to be performed</param>
        void ShowProgressDialog(string title, string text, Action action);

        #endregion Public Methods
    }
}