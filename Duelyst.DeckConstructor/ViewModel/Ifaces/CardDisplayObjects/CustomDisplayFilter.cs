using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Duelyst.DeckConstructor.ViewModel.Ifaces.CardDisplayObjects
{
    public class CustomDisplayFilter : IDisplayableFilter
    {
        private bool _isSelected;

        public CustomDisplayFilter(IList<IDisplayadble> childData)
        {
            ChildData = childData;
            IsAvailebleToSelect = true;
        }

        public bool Equals(IDisplayadble other)
        {
            return other != null && other.Name != null && Name != null && other.Name.Equals(this.Name);
        }

        public BitmapImage Image { get; }
        public string Name { get; }
        public IList<IDisplayadble> ChildData { get; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaiseSelectedChanged(value);
            }
        }

        private void RaiseSelectedChanged(bool value)
        {
            if (value && Selected != null)
            {
                Selected(this);
            }
        }

        public event FilterSelectionChanged Selected;
        public bool IsAvailebleToSelect { get; }
    }
}
