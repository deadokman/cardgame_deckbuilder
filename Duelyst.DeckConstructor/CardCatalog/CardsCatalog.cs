using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;
using Ionic.Zip;

namespace Duelyst.DeckConstructor.CardCatalog
{
    /// <summary>
    /// Каталог с картами (SingleInstance)
    /// </summary>
    public class CardsCatalog
    {
        #region Основные свойства

        /// <summary>
        /// Путь к файлу с картинками и их описанием
        /// </summary>
        private static string CardImageFilePatch = "CardImageData\\CardData.arch";

        /// <summary>
        /// Пароль от архива
        /// </summary>
        private const string ArchPwd = "qazWSX123";

        /// <summary>
        /// Файл с описаниями карт
        /// </summary>
        private const string CardDescriptonFileName = "CardData.xml";

        /// <summary>
        /// Дессириализованный список карт
        /// </summary>
        private List<CardGeneralDto> GeneralsDto { get; set; }

        /// <summary>
        /// Коллекция картинок
        /// </summary>
        private Dictionary<string, BitmapImage> _imageCollection;

        /// <summary>
        /// Сериализатор описаний карт
        /// </summary>
        private XmlSerializer _seri;

        /// <summary>
        /// Получение инстанса  класса
        /// </summary>
        public static CardsCatalog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CardsCatalog();
                    _instance.InitData();
                }

                return _instance;
            }
            
        }
        private static CardsCatalog _instance;

        #endregion

        public List<CardGeneral> ViewModelGenerals; 

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
                    var entries =  zFile.Entries.ToList();
                    foreach (var entry in entries)
                    {
                        var stream = new MemoryStream();
                        entry.Extract(stream);
                        stream.Position = 0;
                        if (entry.FileName.Contains("png"))
                        {
                            var bi = new BitmapImage();
                            bi.BeginInit();
                            bi.StreamSource = new MemoryStream(stream.ToArray());
                            bi.EndInit();
                            _imageCollection.Add(entry.FileName, bi);
                        }
                        else if (entry.FileName.Contains(CardDescriptonFileName))
                        {
#if !DEBUG
                            GeneralsDto = (List<CardGeneralDto>)_seri.Deserialize(stream);
#endif
                        }
                    }
                }
            }
#if DEBUG
            using (var stream = File.OpenRead("CardImageData\\" + CardDescriptonFileName))
            {
                GeneralsDto = (List<CardGeneralDto>)_seri.Deserialize(stream);
            }

#endif
        }

        private CardsCatalog()
        {
            _imageCollection = new Dictionary<string, BitmapImage>();
            ViewModelGenerals = new List<CardGeneral>();
            try
            {
                //Прикол с инициализацией данного варианта конструктора из-за IL инъекций кода внутри класса
                //что приводит к CLR ошибки не влияющей в конечном счете на работу класса. Microsoft sucks!
                //http://stackoverflow.com/questions/3494886/filenotfoundexception-in-applicationsettingsbase
                _seri = new XmlSerializer(typeof (List<CardGeneralDto>), String.Empty);
            }
            catch (Exception ex)
            {
                
            }
        }

        public void InitData()
        {
            InitCat();
            BuildCatalog();
        }

        /// <summary>
        /// Генерация инстанса на основе шаблона объекта
        /// </summary>
        /// <typeparam name="T">Базовый класс карты</typeparam>
        /// <param name="vm">Делегат инициализации базового класса</param>
        /// <param name="dto">DTO объект дессириализованный из XML</param>
        /// <returns></returns>
        private T GetVimodelWithImage<T>(Func<T> vm, CardDtoItem dto)
            where T : CardItemViewModelBase
        {
            var producedType = vm.Invoke();
            if (!String.IsNullOrEmpty(dto.CardImageName) && _imageCollection.ContainsKey(dto.CardImageName))
            {
                producedType.SetImage(_imageCollection[dto.CardImageName]);
            }

            producedType.MaxInDeck = dto.MaxIndeckCount;
            producedType.ManaCost = dto.ManaCost;
            ECardType cardType;
            if (!Enum.TryParse(dto.CardType, out cardType))
            {
                cardType = ECardType.None;
            }
            producedType.CardType = cardType;

            return producedType;
        } 

        private void BuildCatalog()
        {
            foreach (var general in GeneralsDto)
            {
                var root = GetVimodelWithImage(() => new CardGeneral(general.Name), general);
                //Установить картинку     
                ViewModelGenerals.Add(root);
                foreach (var cardDtoItem in general.Cards)
                {
                    root.CardViewModels.Add(GetVimodelWithImage(
                        () => new CardItemViewModelBase(cardDtoItem.Name), cardDtoItem));
                }
            }
        }

    }
}
