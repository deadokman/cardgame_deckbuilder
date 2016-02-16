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
using Duelyst.DeckConstructor.CardCatalog.Provider;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            var initial = Catalog.Instance;
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AppBackgroundPageViewModel>();
            SimpleIoc.Default.Register<MenuViewModel>();
            SimpleIoc.Default.Register<DeckConstructorViewModel>();
            SimpleIoc.Default.Register<SquadManagerViewModel>();
            SimpleIoc.Default.Register<GeneratedImagePreviewViewModel>();
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

        public GeneratedImagePreviewViewModel ImagePreviewViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GeneratedImagePreviewViewModel>();
            }
        }

        public static void Cleanup()
        {

        }
    }
}