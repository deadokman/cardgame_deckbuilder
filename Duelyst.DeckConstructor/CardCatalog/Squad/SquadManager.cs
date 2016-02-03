using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;

namespace Duelyst.DeckConstructor.CardCatalog.Squad
{
    /// <summary>
    /// Менеджер отрядов
    /// </summary>
    public class SquadManager
    {
        private const string SquadFolderName = "Squads";

        /// <summary>
        /// Список имеющихся отрядов
        /// </summary>
        private Dictionary<string, Squad> _squads; 

        /// <summary>
        /// Серриализатор отряда
        /// </summary>
        private XmlSerializer _squadSeri;

        /// <summary>
        /// Экземпляр каталога для класа
        /// </summary>
        private Catalog _catalog;

        public static SquadManager Instance
        {
            get { return _instance == null ? _instance = new SquadManager() : _instance; }
        }

        private static SquadManager _instance;

        private SquadManager()
        {
            _squads = new Dictionary<string, Squad>();
            _catalog = Catalog.Instance;
            InitializeSquads();
        }

        public void InitializeSquads()
        {
            try
            {
                //Прикол с инициализацией данного варианта конструктора из-за IL инъекций кода внутри класса
                //что приводит к CLR ошибки не влияющей в конечном счете на работу экземпляра. Microsoft sucks!
                //http://stackoverflow.com/questions/3494886/filenotfoundexception-in-applicationsettingsbase
                _squadSeri = new XmlSerializer(typeof(Squad), String.Empty);
            }
            catch (Exception)
            {
            }

            var dir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SquadFolderName));
            if (!dir.Exists)
            {
                return;
            }

            var files = dir.GetFiles("*.xml");
            foreach (var fileInfo in files)
            {
                using (var stream = fileInfo.OpenRead())
                {
                    try
                    {
                        var squad = (Squad)_squadSeri.Deserialize(stream);
                        _squads.Add(squad.SquadName, squad);
                        foreach (var card in squad.CardSquadCount)
                        {
                            squad.TryAddCard(_catalog[card.Key]);
                        }

                    }
                    catch (Exception ex)
                    {
                        //TODO: Придумать что-то для процессинга ошибок
                    }
                }
            }
        }

        public Squad InitNewSquad(CardGeneral general)
        {
            var squad = new Squad();
            squad.SquadOwner = general;
            return squad;
        }
    }
}
