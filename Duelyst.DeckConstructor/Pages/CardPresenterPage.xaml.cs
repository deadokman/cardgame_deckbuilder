using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// Список текущих отображаемых карт
        /// </summary>
        private IList<CardItemViewModelBase> _currentDisplay;

        /// <summary>
        /// Регистрация свойства для биндинга списка отображаемых карт
        /// </summary>
        private static DependencyProperty cardDeckcountProperty = DependencyProperty.Register(
        "CardSource",
        typeof(IList<CardItemViewModelBase>), typeof(CardPresenterPage), new PropertyMetadata(new List<CardItemViewModelBase>(), InitCardData));


        private static DependencyProperty cardClickCommand = DependencyProperty.Register(
        "Command",
        typeof(ICommand), typeof(CardPresenterPage), new PropertyMetadata(new RelayCommand(() => { return; }), SetNewClickCommand));


        /// <summary>
        /// Ссылка на хендлер для обработчика клика на карту
        /// </summary>
        private static ICommand _clickCommand;

        /// <summary>
        /// Список представлений для карт
        /// </summary>
        private static List<SinglecardView> _cardViews;

        /// <summary>
        /// Общее количество представлений для крат
        /// </summary>
        private const int CardviewsPresenters = 8;

        public CardPresenterPage()
        {
            InitializeComponent();
            _currentDisplay = new List<CardItemViewModelBase>();
            _cardViews = new List<SinglecardView>(CardviewsPresenters);
            InitViewPresenters();
        }

        private void SetCurrentDisplay(IList<CardItemViewModelBase>  data)
        {
            _currentDisplay = data;
        }

        private void InitViewPresenters()
        {
            _cardViews.Add(V00);
            _cardViews.Add(V01);
            _cardViews.Add(V02);
            _cardViews.Add(V03);
            _cardViews.Add(V10);
            _cardViews.Add(V11);
            _cardViews.Add(V12);
            _cardViews.Add(V13);
        }

        private static void InitCardData(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as CardPresenterPage;
            var items = (IList<CardItemViewModelBase>)e.NewValue;
            if (items == null || ctrl == null)
            {
                return;
            }

            ctrl.SetCurrentDisplay(items);
            var itemsCount = items.Count();
            if (itemsCount == 0)
            {
                return;
            }

            for (int v = 0; v < CardviewsPresenters; v++)
            {
                var item = _cardViews[v];
                if (v < itemsCount)
                {
                    item.CardImage.Source = items[v].Image;
                    item.IsEnabled = true;
                }
                else
                {
                    item.IsEnabled = false;
                }
            }
        }

        public event CardSelectedEventHandler CardClicked;

        private void RaiseCardClicked(CardItemViewModelBase i)
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

        public IList<CardItemViewModelBase> CardSource
        {
            get
            {
                return (IList<CardItemViewModelBase>)GetValue(cardDeckcountProperty);
            }
            set
            {
                SetValue(cardDeckcountProperty, value);
            }
        }

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

        public delegate void CardSelectedEventHandler(CardItemViewModelBase e);

        private void CaldViewClickedCallback(object sender)
        {
            var view = sender as SinglecardView;
            if (view != null)
            {
                var idx=  _cardViews.IndexOf(view);
                if (idx != -1 && _currentDisplay.Count != 0 && idx < _currentDisplay.Count)
                {
                    RaiseCardClicked(_currentDisplay[idx]);
                }
            }
        }

        private void OnCardClickedHandler(object sender, EventArgs e)
        {
            CaldViewClickedCallback(sender);
        }
    }
}
