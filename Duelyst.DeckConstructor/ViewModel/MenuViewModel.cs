using System.Windows.Input;
using System.Windows.Navigation;
using Duelyst.DeckConstructor.Pages;
using Duelyst.DeckConstructor.ViewModel.Communication;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class MenuViewModel : ResizableViewModelBase
    {
        public MenuViewModel()
        {
            NewDeckCommand = new RelayCommand(NotfyNewDeck);
            NotfyCloseCommand = new RelayCommand(NotfyClose);
        }

        public ICommand NewDeckCommand { get; set; }

        public ICommand NotfyCloseCommand { get; set; }

        private void NotfyNewDeck()
        {
            Messenger.Default.Send(new NavigationMessage(CommEventType.NewDeck));
        }

        private void NotfyClose()
        {
            Messenger.Default.Send(new NavigationMessage(CommEventType.Exit));
        }
    }
}
