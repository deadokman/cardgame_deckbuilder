using System.ComponentModel;
using System.Runtime.CompilerServices;
using Duelyst.DeckConstructor.Annotations;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class ChartBarCardInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double ManaCost { get; set; }

        private int _number = 0;
        public int Count
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Count"));
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
