/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Duelyst.DeckConstructor"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Duelyst.DeckConstructor.CardCatalog;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Duelyst.DeckConstructor.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
            var initial = Catalog.Instance;
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AppBackgroundPageViewModel>();
            SimpleIoc.Default.Register<MenuViewModel>();
            SimpleIoc.Default.Register<DeckConstructorViewModel>();
            SimpleIoc.Default.Register<SquadManagerViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public AppBackgroundPageViewModel AppBackgroundPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AppBackgroundPageViewModel>();
            }
        }

        public DeckConstructorViewModel DeckConstructorViewModel
        {
            get { return ServiceLocator.Current.GetInstance<DeckConstructorViewModel>(); }
        }

        public MenuViewModel MenuMain
        {
            get { return ServiceLocator.Current.GetInstance<MenuViewModel>(); }
        }

        public SquadManagerViewModel SquadBuilderListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SquadManagerViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}