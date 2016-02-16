using Duelyst.DeckConstructor.VectorCalcs;
using Duelyst.DeckConstructor.ViewModel.Communication;
using GalaSoft.MvvmLight.Messaging;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class AppBackgroundPageViewModel : ResizableViewModelBase
    {
        private double _pageHeightFF;
        private double _pageWidthFF;

        private double _xscale;
        private double _yScaleFront;

        private const double ForeGroundFactor = 40;
        private const double MiddleFactor = 20;


        private double _scaleCenterX;
        private double _scaleCenterY;
        private double _xScaleMiddle;
        private double _yScaleMiddle;
        private int _selectedMenuTab;
        private bool _isBlurScene;

        public AppBackgroundPageViewModel()
        {
            ScaleCenterX = 0;
            ScaleCenterY = 0;
            XScaleMiddle = 1;
            YScaleMiddle = 1;
            XScaleFront = 1;
            YScaleFront = 1;
            SelectedMenuTab = 0;
            Messenger.Default.Register<NavigationMessage>(this, ResolveCommMessage);
        }

        private void ResolveCommMessage(NavigationMessage navMess)
        {
            switch (navMess.CommMsgType)
            {
                case CommEventType.NewDeck: SelectedMenuTab = 1;
                    IsBlurScene = true;
                    break;
                case CommEventType.MainMenu: SelectedMenuTab = 0;
                    IsBlurScene = false;
                    break;
                default: break;;
            }

        }

        private double CalcScaleFactor(double vLength, double scaleFactor, double centerPosX, double centerPosY)
        {
            return 1 + vLength / (centerPosX * 2 * centerPosY * 2) * scaleFactor;
        }

        public override void MouseMove(double newPosx, double newPosy, double centerPosX, double centerPosY)
        {
            var vectLength = VectorMath.VDotProductZero(newPosx, newPosy);
            var scaleFactorFront = CalcScaleFactor(vectLength, ForeGroundFactor, centerPosX, centerPosY);
            var scaleFactorMiddle = CalcScaleFactor(vectLength, MiddleFactor, centerPosX, centerPosY);
            XScaleFront = scaleFactorFront;
            YScaleFront = scaleFactorFront;
            XScaleMiddle = scaleFactorMiddle;
            YScaleMiddle = scaleFactorMiddle;
            //Изменить положение центра точки скейла
            ScaleCenterX = newPosx;
        }

        public bool IsBlurScene
        {
            get { return _isBlurScene; }
            set
            {
                _isBlurScene = value; 
                RaisePropertyChanged(() => IsBlurScene);
            }
        }

        public int SelectedMenuTab
        {
            get { return _selectedMenuTab; }
            set
            {
                _selectedMenuTab = value;
                RaisePropertyChanged(() => SelectedMenuTab);
            }
        }

        #region ScaleCenter

        public double ScaleCenterX
        {
            get { return _scaleCenterX; }
            set
            {
                _scaleCenterX = value;
                RaisePropertyChanged(() => ScaleCenterX);
            }
        }

        public double ScaleCenterY
        {
            get { return _scaleCenterY; }
            set
            {
                _scaleCenterY = value;
                RaisePropertyChanged(() => ScaleCenterY);
            }
        }

        #endregion

        #region MiddleTransform

        public double XScaleMiddle
        {
            get { return _xScaleMiddle; }
            set
            {
                _xScaleMiddle = value;
                RaisePropertyChanged(() => XScaleMiddle);
            }
        }

        public double YScaleMiddle
        {
            get { return _yScaleMiddle; }
            set
            {
                _yScaleMiddle = value; 
                RaisePropertyChanged(() => YScaleMiddle);
            }
        }

        #endregion

        #region ForegroundTransform

        public double XScaleFront
        {
            get { return _xscale; }
            set
            {
                _xscale = value;
                RaisePropertyChanged(() => XScaleFront);
            }
        }

        public double YScaleFront
        {
            get { return _yScaleFront; }
            set
            {
                _yScaleFront = value; 
                RaisePropertyChanged(() => YScaleFront);
            }
        }

        #endregion
    }
}
