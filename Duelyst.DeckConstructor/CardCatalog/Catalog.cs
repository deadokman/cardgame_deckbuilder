using System.Collections.Generic;
using System.Linq;
using Duelyst.DeckConstructor.CardCatalog.Provider;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;
using GalaSoft.MvvmLight.Ioc;

namespace Duelyst.DeckConstructor.CardCatalog
{
    /// <summary>
    /// Каталог с картами (SingleInstance)
    /// </summary>
    public class Catalog
    {
        #region Основные свойства

        /// <summary>
        /// Экземпляр провайдера карт
        /// </summary>
        private ICardProvider _cardProvider;

        /// <summary>
        /// Фабрика карт
        /// </summary>
        private CardViewModelFactory _cardViewModelFactory;

        /// <summary>
        /// Получение инстанса  класса
        /// </summary>
        public static Catalog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Catalog();
                    _instance.InitCat();
                }

                return _instance;
            }
            
        }
        private static Catalog _instance;

        #endregion

        public CardGeneral[] Generals { get { return _viewModelGeneralsDict.Select(i => i.Value).ToArray(); } }

        /// <summary>
        /// Список доступных генералов
        /// </summary>
        private readonly Dictionary<string, CardGeneral> _viewModelGeneralsDict;

        /// <summary>
        /// Плоский список с картами
        /// </summary>
        private readonly SortedList<string, CardItemViewModelBase> _flatCardList; 

        /// <summary>
        /// Инициализация каталога с картами
        /// </summary>
        public void InitCat()
        {
            var images = _cardProvider.GetCardsImageData();
            _cardViewModelFactory = new CardViewModelFactory(images);
            var generals = _cardProvider.GetGenerals();
            foreach (var general in generals)
            {
                var root = _cardViewModelFactory.GetVimodelWithImage(() => new CardGeneral(general.Name),  general);
                _flatCardList.Add(root.CardId, root);
                //Установить картинку     
                _viewModelGeneralsDict.Add(root.CardId, root);
                foreach (var cardDtoItem in general.Cards)
                {
                    var minionCard = _cardViewModelFactory.GetVimodelWithImage(
                        () => new CardItemViewModelBase(cardDtoItem.Name), cardDtoItem);
                    root.AddCard(minionCard);
                    _flatCardList.Add(minionCard.CardId, minionCard);
                }
            }
        }

        /// <summary>
        /// Каталог с картами
        /// </summary>
        private Catalog()
        {
            _cardProvider = SimpleIoc.Default.GetInstance<ICardProvider>();
            _viewModelGeneralsDict = new Dictionary<string, CardGeneral>();
            _flatCardList= new SortedList<string, CardItemViewModelBase>();
        }


        /// <summary>
        /// Индексатор класса для получения генерала
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CardItemViewModelBase this[string id]
        {
            get
            {
                CardItemViewModelBase card;
                return _flatCardList.TryGetValue(id, out card) ? card : null;
            }
        }
    }
}
