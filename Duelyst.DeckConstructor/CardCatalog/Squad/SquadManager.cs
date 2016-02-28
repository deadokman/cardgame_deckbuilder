using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Duelyst.DeckConstructor.ViewModel.Communication;
using Duelyst.DeckConstructor.ViewModel.DeckCardItem;

namespace Duelyst.DeckConstructor.CardCatalog.Squad
{
    /// <summary>
    /// Менеджер отрядов
    /// </summary>
    public class SquadManager
    {
        //TODO: DefineGlobalConfig

        public static int MaxCardCount
        {
            get { return 40; }
        }

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

        /// <summary>
        /// Отряды
        /// </summary>
        public IEnumerable<Squad> Squads { get { return _squads.Select(s => s.Value); } } 

        public void InitializeSquads()
        {
            //Прикол с инициализацией данного варианта конструктора из-за IL инъекций кода внутри класса
            //что приводит к CLR ошибки не влияющей в конечном счете на работу экземпляра. Microsoft sucks!
            //http://stackoverflow.com/questions/3494886/filenotfoundexception-in-applicationsettingsbase
            try
            {
                _squadSeri = new XmlSerializer(typeof(SquadDto));
            }
            catch (Exception)
            {
                
            }

            var dir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SquadFolderName));
            if (!dir.Exists)
            {
                return;
            }

            var files = dir.GetFiles("*.squad");
            foreach (var fileInfo in files)
            {
                using (var stream = fileInfo.OpenRead())
                {
                    try
                    {
                        var squadDto = (SquadDto)_squadSeri.Deserialize(stream);
                        var squad = squadDto.GetSquad();
                        _squads.Add(squad.SquadName, squad);
                        squad.SquadOwner = _catalog.Generals.FirstOrDefault(g => g.CardId == squadDto.GeneralId);
                        CardAddResponse resp;
                        foreach (var card in squadDto.CardCountInfo)
                        {
                            squad.TryAddCard(_catalog[card.CardId], out resp);
                        }
                    }
                    catch (Exception ex)
                    {
                        //TODO: Придумать что-то для процессинга ошибок
                    }
                }
            }
        }

        public void StoreSquadToDefaultLocation(Squad squad)
        {
            var squadFolder = Path.Combine(Directory.GetCurrentDirectory(), SquadFolderName);
            if (!Directory.Exists(squadFolder))
            {
                Directory.CreateDirectory(squadFolder);
            }

            var file = Path.Combine(squadFolder, $"{squad.Name}_{squad.SquadOwner.Name}.squad");
            using (var fs = File.Open(file, FileMode.OpenOrCreate))
            {
                _squadSeri.Serialize(fs, squad.SquadDto);
            }

            _squads[squad.SquadName] = squad;
        }

        public Squad InitNewSquad(CardGeneral general)
        {
            var squad = new Squad();
            squad.CardGeneralId = general.CardId;
            squad.SquadOwner = general;
            return squad;
        }
    }
}
