using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Duelyst.DeckConstructor.ViewModel.Communication;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;

namespace Duelyst.DeckConstructor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<NavigationMessage>(this, HandleNavMessage);
        }

        private void HandleNavMessage(NavigationMessage navMessage)
        {
            switch (navMessage.CommMsgType)
            {
                case CommEventType.Exit:
                    this.Close();
                    break;
            }
        }
    }
}
