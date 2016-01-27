using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Duelyst.DeckConstructor.CardCatalog
{
    /// <summary>
    /// Дессиреализуемый объект карты
    /// </summary>
    [Serializable]
    public class CardDtoItem
    {
        /// <summary>
        /// Название карты
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Тип карты (заклинение, артифакт, существо, генерал)
        /// </summary>
        [DataMember]
        public int CardType { get; set; }

        /// <summary>
        /// Максимальное количество в колоде
        /// </summary>
        [DataMember]
        public int MaxIndeckCount { get; set; }

        /// <summary>
        /// Цена карты
        /// </summary>
        [DataMember]
        public int ManaCost { get; set; }

        /// <summary>
        /// Атака
        /// </summary>
        [DataMember]
        public int? Attack { get; set; }

        /// <summary>
        /// Здоровье
        /// </summary>
        [DataMember]
        public int? Health { get; set; }

        /// <summary>
        /// Описание карты
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Способность (предсмертный хрип, боевой клич)
        /// </summary>
        [DataMember]
        public string AbilityName { get; set; }
    }
}
