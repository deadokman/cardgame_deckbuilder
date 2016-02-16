using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Duelyst.DeckConstructor.CardCatalog.Squad;
using Duelyst.DeckConstructor.ViewModel.Communication;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class GeneratedImagePreviewViewModel : ViewModelBase
    {
        private bool _redrawNeeded;

        private object _workerLock = new object();

        public GeneratedImagePreviewViewModel()
        {
            Messenger.Default.Register<NavigationMessage>(this, (e) => OnShowMsgRecive(e));
            _redrawWorkerbackground = new BackgroundWorker();
            _redrawWorkerbackground.WorkerSupportsCancellation = false;
            _redrawWorkerbackground.DoWork += RedrawWorkerbackgroundOnDoWork;
            _redrawWorkerbackground.RunWorkerCompleted += RedrawWorkerbackgroundOnRunWorkerCompleted;
            ZoomController = 100;
        }

        private Bitmap _imageToDisplay;

        private void RedrawWorkerbackgroundOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            var result = runWorkerCompletedEventArgs.Result as Bitmap;
            if (result != null)
            {
                DisplayableImage = LoadFromBitmap(result);
                _imageToDisplay = result;
            }

            lock (_workerLock)
            {
                if (_redrawNeeded)
                {
                    _redrawNeeded = false;
                    _redrawWorkerbackground.RunWorkerAsync(_lastSquadtoDisplay);
                }
            }
        }

        /// <summary>
        /// Конвертировать картинку в источник данных для отображения
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private BitmapImage LoadFromBitmap(Bitmap image)
        {
            using (var memory = new MemoryStream())
            {
                image.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        private void RedrawWorkerbackgroundOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var squad = doWorkEventArgs.Argument as Squad;
            if (squad != null)
            {
                doWorkEventArgs.Result = ToPictureProcessor.SquadToImage(_lastSquadtoDisplay);
            }
        }

        private Squad _lastSquadtoDisplay;

        private BackgroundWorker _redrawWorkerbackground;

        #region Bindable props

        public double ImageControllScale
        {
            get
            {
                return imageControllScale;
            }
            set
            {
                imageControllScale = value;
                RaisePropertyChanged(() => ImageControllScale);
            }
        }

        /// <summary>
        /// Зум изображения
        /// </summary>
        public int ZoomController
        {
            get
            {
                return zoomController;
            }
            set
            {
                zoomController = value;
                RescaleImageControll(value);
                RaisePropertyChanged(() => ZoomController);
            }
        }

        /// <summary>
        /// Интервал между строками
        /// </summary>
        public int RowInterval
        {
            get
            {
                return ToPictureProcessor.CardRowIntervalPx;
            }
            set
            {
                ToPictureProcessor.CardRowIntervalPx = value;
                RedrawImage();
            }
        }

        /// <summary>
        /// Интервал между столбцами
        /// </summary>
        public int ColInterval
        {
            get
            {
                return ToPictureProcessor.CardColIntervalPx;
            }
            set
            {
                ToPictureProcessor.CardColIntervalPx = value;
                RedrawImage();
            }
        }

        /// <summary>
        /// Карт в строке
        /// </summary>
        public int CardInRow
        {
            get
            {
                return ToPictureProcessor.CardsInrow;
            }
            set
            {
                ToPictureProcessor.CardsInrow = value;
                RedrawImage();
            }
        }

        /// <summary>
        /// Размер рамки по высоте
        /// </summary>
        public int BorderH
        {
            get
            {
                return ToPictureProcessor.BorderHLength;
            }
            set
            {
                ToPictureProcessor.BorderHLength = value;
                RedrawImage();
            }
        }

        /// <summary>
        /// Размер рамки по ширине
        /// </summary>
        public int BorderW
        {
            get
            {
                return ToPictureProcessor.BorderWLength;
            }
            set
            {
                ToPictureProcessor.BorderWLength = value;
                RedrawImage();
            }
        }

        /// <summary>
        /// Количество карт в слое подложки
        /// </summary>
        public int CardDistSublayer
        {
            get
            {
                return ToPictureProcessor.CardDispLayer;
            }
            set
            {
                ToPictureProcessor.CardDispLayer = value;
                RedrawImage();
            }
        }

        /// <summary>
        /// Свойство для биндинга изображения
        /// </summary>
        public ImageSource DisplayableImage
        {
            get
            {
                return displayableImage;
            }
            set
            {
                displayableImage = value;
                RaisePropertyChanged(() => DisplayableImage);
            }
        }

        /// <summary>
        /// Картинка к отрисовке
        /// </summary>
        private ImageSource displayableImage;

        private int zoomController;

        private double imageControllScale;

        #endregion

        private void RescaleImageControll(double percentScale)
        {
            ImageControllScale = percentScale / 100;
        }

        private void RedrawImage()
        {
            lock (_workerLock)
            {
                if (!_redrawWorkerbackground.IsBusy)
                {
                    _redrawWorkerbackground.RunWorkerAsync(_lastSquadtoDisplay);
                }
                else
                {
                    _redrawNeeded = true;
                }
            }
        }

        private void OnShowMsgRecive(NavigationMessage msg)
        {
            if (msg.CommMsgType == CommEventType.WindowPreview)
            {
                if (msg.SquadToDisplay != null)
                {
                    _lastSquadtoDisplay = msg.SquadToDisplay;
                    RedrawImage();
                }
            }
        }
    }
}
