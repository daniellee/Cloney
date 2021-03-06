﻿using System.Windows;
using System.Windows.Threading;
using Cloney.Wizard.Resources;

namespace Cloney.Wizard
{
    /// <summary>
    /// The main Cloney Wizard application class.
    /// </summary>
    /// <remarks>
    /// Author:     Daniel Saidi [daniel.saidi@gmail.com]
    /// Link:       http://danielsaidi.github.com/Cloney
    /// </remarks>
    public partial class App
    {
        public App()
        {
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        static void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var errorMessage = string.Format(Language.UnhandledExceptionPattern, e.Exception.Message);
            MessageBox.Show(errorMessage, Language.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
