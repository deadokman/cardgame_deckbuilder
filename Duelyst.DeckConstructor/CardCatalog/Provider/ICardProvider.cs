using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Duelyst.DeckConstructor.CardCatalog.Provider
{
    public interface ICardProvider
    {
        /// <summary>
        /// Получить информацию о картинках
        /// </summary>
        /// <returns></returns>
        Dictionary<string, BitmapImage> GetCardsImageData();

        /// <summary>
        /// Получить описания карт в виде DTO объектов
        /// </summary>
        /// <returns></returns>
        List<CardGeneralDto> GetGenerals();
    }
}
