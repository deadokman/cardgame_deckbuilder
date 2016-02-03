using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using Duelyst.DeckConstructor.CardCatalog.Provider;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Duelyst.DeckConstructor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<ICardProvider>(() => new LocalCardDataProvider());

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var res = MessageBox.Show(this.MainWindow, unhandledExceptionEventArgs.ExceptionObject.ToString(), "Необработанное исключение",
                MessageBoxButton.OK, MessageBoxImage.Error);
            if (res == MessageBoxResult.OK)
            {
                this.Shutdown(220);
            }
        }
    }
}
