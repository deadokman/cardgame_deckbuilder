using System;
using System.Collections.Generic;

namespace Duelyst.DeckConstructor.ViewModel.Ifaces.CardDisplayObjects
{
    public interface IDisplayableFilter : IDisplayadble
    {
        IList<IDisplayadble> ChildData { get;  }
        bool IsSelected { get; set; }
        event FilterSelectionChanged Selected;
        bool IsAvailebleToSelect { get; }
    }
}
