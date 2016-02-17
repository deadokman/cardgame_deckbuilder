using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Duelyst.DeckConstructor.CardCatalog.Squad;
using Duelyst.DeckConstructor.ViewModel.Communication;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Win32;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class GeneratedImagePreviewViewModel : ViewModelBase
    {
        private bool _redrawNeeded;

        private object _workerLock = new object();

        private const string UploadQryUrl = "http://shot.qip.ru/upload?chk_reduce=on&reduce=1024&file={0}&js={1}";

        public GeneratedImagePreviewViewModel()
        {
            Messenger.Default.Register<CommunicationMessage>(this, (e) => OnShowMsgRecive(e));
            _redrawWorkerbackground = new BackgroundWorker();
            _redrawWorkerbackground.WorkerSupportsCancellation = false;
            _redrawWorkerbackground.DoWork += RedrawWorkerbackgroundOnDoWork;
            _redrawWorkerbackground.RunWorkerCompleted += RedrawWorkerbackgroundOnRunWorkerCompleted;
            PersistImageCommand = new RelayCommand(ExecutePersist);
            UploadComamnd = new RelayCommand(ExecuteUpload);
            ZoomController = 100;
        }

        private void ExecuteUpload()
        {
            using (var webClient = new WebClient())
            {
                using (var ms = new MemoryStream())
                {
                    _imageToDisplay.Save(ms, ImageFormat.Png);
                    var str = String.Format("image_{0}_{1}", _lastSquadtoDisplay.SquadOwner.Name, _lastSquadtoDisplay.Name);
                    var url = string.Format(UploadQryUrl, str, 0.222);
                    var respArr = webClient.UploadData(url, ms.ToArray());
                    var resp = Encoding.ASCII.GetString(respArr);
                    try
                    {
                        var pars = resp.Split(',');
                        var par = pars.FirstOrDefault(p => p.Contains("real"));
                        if (par != null)
                        {
                            var addr = par.Split(':');
                            if (addr.Count() > 1)
                            {
                                var imgUrl = addr[2].Replace("\\", String.Empty).Replace("\"", String.Empty);
                                System.Diagnostics.Process.Start("http:" + imgUrl);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
        }

        private void ExecutePersist()
        {
            var dlg = new SaveFileDialog();
            dlg.FileName = String.Format("image_{0}_{1}", _lastSquadtoDisplay.SquadOwner.Name, _lastSquadtoDisplay.Name);
            dlg.DefaultExt = ".png";
            dlg.Filter = "Text documents (.png)|*.png";
            var res = dlg.ShowDialog();
            if (res != null && res == true)
            {
                using (var fw = File.OpenWrite(dlg.FileName))
                {
                    _imageToDisplay.Save(fw, ImageFormat.Png);
                }
            }
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

            if (_redrawNeeded)
            {
                _redrawNeeded = false;
                RedrawImage();
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

        public int LogoScale
        {
            get { return ToPictureProcessor.LogoScaleFactor; }
            set
            {
                ToPictureProcessor.LogoScaleFactor = value;
                RaisePropertyChanged(() => LogoScale);
                RedrawImage();
            }
        }

        public int SquadFontScaleFactor
        {
            get { return ToPictureProcessor.SquadFontScaleFactor; }
            set
            {
                ToPictureProcessor.SquadFontScaleFactor = value;
                RaisePropertyChanged(() => SquadFontScaleFactor);
                RedrawImage();
            }
        }

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
                RaisePropertyChanged(() => RowInterval);
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
                RaisePropertyChanged(() => ColInterval);
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
                RaisePropertyChanged(() => CardInRow);
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
                RaisePropertyChanged(() => BorderH);
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
                RaisePropertyChanged(() => BorderW);
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
                RaisePropertyChanged(() => CardDistSublayer);
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
            if (!_redrawWorkerbackground.IsBusy)
            {
                _redrawWorkerbackground.RunWorkerAsync(_lastSquadtoDisplay);
            }
            else
            {
                _redrawNeeded = true;
            }
        }

        public  ICommand UploadComamnd { get; set; }

        public ICommand PersistImageCommand { get; set; }

        private void OnShowMsgRecive(CommunicationMessage msg)
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
