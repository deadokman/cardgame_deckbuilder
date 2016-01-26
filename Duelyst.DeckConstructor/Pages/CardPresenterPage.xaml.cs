using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;
using GalaSoft.MvvmLight.Command;

namespace Duelyst.DeckConstructor.Pages
{
    /// <summary>
    /// Interaction logic for CardPresenterPage.xaml
    /// </summary>
    public partial class CardPresenterPage : UserControl
    {

        private IList<DeckCardItemViewModel> _currentDisplay;

        private static DependencyProperty cardDeckcountProperty = DependencyProperty.Register(
        "CardSource",
        typeof(IList<DeckCardItemViewModel>), typeof(CardPresenterPage), new PropertyMetadata(new List<DeckCardItemViewModel>(), InitCardData));

        private static ICommand _clickCommand;

        public CardPresenterPage()
        {
            InitializeComponent();
            _currentDisplay = new List<DeckCardItemViewModel>();
        }

        private void SetCurrentDisplay(IList<DeckCardItemViewModel>  data)
        {
            _currentDisplay = data;
        }

        private static void InitCardData(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as CardPresenterPage;
            var items = (IList<DeckCardItemViewModel>)e.NewValue;
            ctrl.SetCurrentDisplay(items);
            for (int v = 0; v < items.Count; v++)

            {
                switch (v)
                {
                    case 0:
                        ctrl.V00.CardImage.Source = items[v].Image;
                        break;
                    case 1:
                        ctrl.V01.CardImage.Source = items[v].Image;
                        break;
                    case 2:
                        ctrl.V02.CardImage.Source = items[v].Image;
                        break;
                    case 3:
                        ctrl.V03.CardImage.Source = items[v].Image;
                        break;
                    case 4:
                        ctrl.V10.CardImage.Source = items[v].Image;
                        break;
                    case 5:
                        ctrl.V11.CardImage.Source = items[v].Image;
                        break;
                    case 6:
                        ctrl.V12.CardImage.Source = items[v].Image;
                        break;
                    case 7:
                        ctrl.V13.CardImage.Source = items[v].Image;
                        break;
                    default: break;
                }
            }
        }

        public event CardSelectedEventHandler CardClicked;

        private void RaiseCardClicked(DeckCardItemViewModel i)
        {
            if (_clickCommand != null)
            {
                _clickCommand.Execute(i);
            }
            if (CardClicked != null)
            {
                CardClicked(i);
            }
        }

        public IList<DeckCardItemViewModel> CardSource
        {
            get { return (IList<DeckCardItemViewModel>)GetValue(cardDeckcountProperty); }
            set
            {
                SetValue(cardDeckcountProperty, value);
            }
        }


        private static DependencyProperty cardClickCommand = DependencyProperty.Register(
        "Command",
        typeof(ICommand), typeof(CardPresenterPage), new PropertyMetadata(new RelayCommand(() => {return;}), SetNewClickCommand));

        private static void SetNewClickCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var items = (ICommand)e.NewValue;
            _clickCommand = items;
        }

        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(cardClickCommand);
            }
            set
            {
                SetValue(cardDeckcountProperty, value);
            }
        }

        public delegate void CardSelectedEventHandler(DeckCardItemViewModel e);

        private void V00_OnCardClickedHandler(object sender, EventArgs e)
        {
            RaiseCardClicked(_currentDisplay[0]);
        }

        private void V01_OnCardClickedHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
