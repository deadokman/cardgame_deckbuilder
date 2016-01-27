using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

using Ionic.Zip;

namespace Duelyst.DeckConstructor.CardCatalog
{
    /// <summary>
    /// Каталог с картами (SingleInstance)
    /// </summary>
    public class CardsCatalog
    {
        /// <summary>
        /// Путь к файлу с картинками и их описанием
        /// </summary>
        private static string CardImageFilePatch = "CardImageData\\CardData.arch";

        /// <summary>
        /// Пароль от архива
        /// </summary>
        private const string ArchPwd = "qazWSX123";

        /// <summary>
        /// Дессириализованный список карт
        /// </summary>
        public List<CardDtoItem> CardItems { get; set; }

        /// <summary>
        /// Коллекция картинок
        /// </summary>
        private Dictionary<string, Image> ImageCollection; 

        /// <summary>
        /// Получение инстанса  класса
        /// </summary>
        public static CardsCatalog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CardsCatalog();;
                }

                return _instance;
            }
            
        }

        private static CardsCatalog _instance;

        /// <summary>
        /// Инициализация каталога с картами
        /// </summary>
        private void InitCat()
        {
            using (var file = File.OpenRead(CardImageFilePatch))
            {
                using (var zFile = ZipFile.Read(file))
                {
                    zFile.Password = ArchPwd;
                    ImageCollection = zFile.Entries.Select(
                        e =>
                            {
                                var memStraeam = new MemoryStream();
                                e.Extract(memStraeam);
                                memStraeam.Position = 0;
                                return new { key = e.FileName, Image = Image.FromStream(memStraeam) };
                            }).ToDictionary(i => i.key, i => i.Image);
                }
            }
        }

        private CardsCatalog()
        {
            InitCat();
        }


    }
}
