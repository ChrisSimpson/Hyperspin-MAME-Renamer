using Renamer.Services;
using System;
using System.Windows;
using System.Windows.Threading;

namespace Renamer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Protected Methods

        /// <summary>
        /// Called when the application is started.
        /// </summary>
        /// <param name="e">The startup event arguments</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            RegisterServices();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            Application.Current.DispatcherUnhandledException += Dispatcher_UnhandledException;

            base.OnStartup(e);
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Register services
        /// </summary>
        private static void RegisterServices()
        {
            ServiceFactory.RegisterService<IDialogService, DialogService>();
        }

        /// <summary>
        /// AppDomain unhandled exception handler
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ServiceFactory.Get<IDialogService>().ShowExceptionDialog(e.ExceptionObject as Exception);
        }

        /// <summary>
        /// Dispatcher unhandled exception handler
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ServiceFactory.Get<IDialogService>().ShowExceptionDialog(e.Exception as Exception);
        }

        #endregion Private Methods
    }
}