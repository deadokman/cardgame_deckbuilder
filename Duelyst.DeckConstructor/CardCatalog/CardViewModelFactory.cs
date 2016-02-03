using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Media.Imaging;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;

namespace Duelyst.DeckConstructor.CardCatalog
{
    public class CardViewModelFactory
    {
        private Dictionary<string, BitmapImage> _images; 

        public CardViewModelFactory(Dictionary<string, BitmapImage> images)
        {
            _images = images;
        }

        /// <summary>
        /// Генерация инстанса на основе шаблона объекта
        /// </summary>
        /// <typeparam name="T">Базовый класс карты</typeparam>
        /// <param name="vm">Делегат инициализации базового класса</param>
        /// <param name="dto">DTO объект дессириализованный из XML</param>
        /// <returns></returns>
        public T GetVimodelWithImage<T>(Func<T> vm, CardDtoItem dto)
            where T : CardItemViewModelBase
        {
            var producedType = vm.Invoke();
            if (!String.IsNullOrEmpty(dto.CardImageName) && _images.ContainsKey(dto.CardImageName))
            {
                producedType.SetImage(_images[dto.CardImageName]);
            }

            producedType.MaxInDeck = dto.MaxIndeckCount;
            producedType.ManaCost = dto.ManaCost;
            ECardType cardType;
            if (!Enum.TryParse(dto.CardType, out cardType))
            {
                cardType = ECardType.None;
            }

            producedType.CardType = cardType;
            if (string.IsNullOrEmpty(dto.Id))
            {
                producedType.CardId = (dto.Name.GetHashCode() ^ dto.ManaCost.GetHashCode()).ToString();
            }
            else
            {
                producedType.CardId = dto.Id;
            }

            return producedType;
        }
    }
}
