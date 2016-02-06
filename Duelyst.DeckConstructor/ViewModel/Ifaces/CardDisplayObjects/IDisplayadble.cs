using System;
using System.Windows.Media.Imaging;

namespace Duelyst.DeckConstructor.ViewModel.Ifaces.CardDisplayObjects
{
    public interface IDisplayadble : IEquatable<IDisplayadble>
    {
        BitmapImage Image { get; }
        string Name { get; }
    }
}
