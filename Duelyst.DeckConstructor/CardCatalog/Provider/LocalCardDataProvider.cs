using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using Ionic.Zip;

namespace Duelyst.DeckConstructor.CardCatalog.Provider
{
    public class LocalCardDataProvider : ICardProvider
    {
        private XmlSerializer _seri;

        public LocalCardDataProvider()
        {
            try
            {
                //Прикол с инициализацией данного варианта конструктора из-за IL инъекций кода внутри класса
                //что приводит к CLR ошибки не влияющей в конечном счете на работу экземпляра. Microsoft sucks!
                //http://stackoverflow.com/questions/3494886/filenotfoundexception-in-applicationsettingsbase
                _seri = new XmlSerializer(typeof(List<CardGeneralDto>), String.Empty);
            }
            catch (Exception ex)
            {

            }
        }

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
        /// Получить информацию об артах карт
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, BitmapImage> GetCardsImageData()
        {
            var dict = new Dictionary<string, BitmapImage>();
            using (var file = File.OpenRead(CardImageFilePatch))
            {
                using (var zFile = ZipFile.Read(file))
                {
                    zFile.Password = ArchPwd;
                    var entries = zFile.Entries.ToList();
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
                            dict.Add(entry.FileName, bi);
                        }
                    }
                }
            }

            return dict;
        } 

        /// <summary>
        /// Получить DTO классы с информацией о картах
        /// </summary>
        /// <returns></returns>
        public List<CardGeneralDto> GetGenerals()
        {
#if DEBUG
            using (var stream = File.OpenRead("CardImageData\\" + CardDescriptonFileName))
            {
                return (List<CardGeneralDto>)_seri.Deserialize(stream);
            }
#else
            throw new NotImplementedException();
#endif

        }
    }
}
