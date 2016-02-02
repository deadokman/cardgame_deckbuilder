using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

        private Func<string, CardItemViewModelBase> _getCatdDelegate;

        private XmlSerializer _squadSeri;

        public SquadManager(Func<string, CardItemViewModelBase> getCardDelegate)
        {
            _getCatdDelegate = getCardDelegate;
            _squads = new Dictionary<string, Squad>();

        }

        public void InitializeSquads()
        {
            _squadSeri = new XmlSerializer(typeof(Squad), String.Empty);
            var dir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SquadFolderName));
            var files = dir.GetFiles("*.xml");
            foreach (var fileInfo in files)
            {
                using (var stream = fileInfo.OpenRead())
                {
                    try
                    {
                        var squad = (Squad)_squadSeri.Deserialize(stream);
                        _squads.Add(squad.SquadName, squad);
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
            //Инициализировать состав отряда
            foreach (var squad in _squads)
            {
                squad.Value.InitCards(_getCatdDelegate);
            }
        }

        public void InitNewSquad(CardGeneral general)
        {
            
        }
    }
}
