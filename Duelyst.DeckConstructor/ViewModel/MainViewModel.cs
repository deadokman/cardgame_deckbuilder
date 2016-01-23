using System;
using System.Media;
using System.Reflection;
using System.Reflection.Emit;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Duelyst.DeckConstructor.Pages;
using Duelyst.DeckConstructor.ViewModel.Communication;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace Duelyst.DeckConstructor.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private Page _contentPage;
        private string _testProp;

        private Timer _tTimer;
        private SoundPlayer _player;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ContentPage = new SplashScreenPage();
            _tTimer = new Timer(1000);
            _tTimer.AutoReset = false;
            _tTimer.Elapsed += ElapsedEventHandler;
            _tTimer.Start();
            //Инициализировать звуковое оформление
            InitSoundMode();
        }

        private void InitSoundMode()
        {
            //var stream = Assembly.GetExecutingAssembly()
             //   .GetManifestResourceStream("Duelyst.DeckConstructor.AdditionalResources.mm_audio.wav");
            //_player = new SoundPlayer(stream);
            //_player.PlayLooping();
        }

        public void ElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {

                ContentPage = new MainAppPage();
            });

        }

        private void StartMusicPlaying()
        {
            
        }

        public Page ContentPage
        {
            get { return _contentPage; }
            set
            {
                _contentPage = value;
                RaisePropertyChanged(() => ContentPage);
            }
        }

        public string TestProp
        {
            get { return _testProp; }
            set
            {
                _testProp = value;
                RaisePropertyChanged(() => TestProp);
            }
        }
    }
}