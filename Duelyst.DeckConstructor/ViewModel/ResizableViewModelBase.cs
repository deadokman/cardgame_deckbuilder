using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duelyst.DeckConstructor.ViewModel.Ifaces;
using GalaSoft.MvvmLight;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class ResizableViewModelBase : ViewModelBase, IResizeableViewModel
    {
        private double _pageHeight;
        private double _pageWidth;
        private double _pageHeightExtended;
        private const double ImageVertHeightExtendFactor = 1.5;

        public ResizableViewModelBase()
        {
            _pageHeight = 800;
            _pageWidth = 1200;
        }

        public double PageWidth
        {
            get { return _pageWidth; }
            set
            {
                _pageWidth = value;
                RaisePropertyChanged(() => PageWidth);
            }
        }

        public double PageHeight
        {
            get { return _pageHeight; }
            set
            {
                _pageHeight = value;
                PageHeightExtended = value;
                RaisePropertyChanged(() => PageHeight);
            }
        }

        public double PageHeightExtended
        {
            get { return _pageHeightExtended; }
            set
            {
                _pageHeightExtended = value * ImageVertHeightExtendFactor;
                RaisePropertyChanged(() => PageHeightExtended);
            }
        }

        public virtual void MouseMove(double newPosx, double newPosy, double centerPosX, double centerPosY)
        {
            return;
        }
    }
}
