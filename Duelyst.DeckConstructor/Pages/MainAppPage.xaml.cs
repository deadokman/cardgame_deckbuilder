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
using Duelyst.DeckConstructor.ViewModel.Ifaces;

namespace Duelyst.DeckConstructor.Pages
{
    /// <summary>
    /// Interaction logic for MainAppPage.xaml
    /// </summary>
    public partial class MainAppPage : Page
    {

        private IResizeableViewModel _context;

        public MainAppPage()
        {
            InitializeComponent();
            _context = DataContext as IResizeableViewModel;
        }

        private void MainAppPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_context != null)
            {
                _context.PageWidth = e.NewSize.Width;
                _context.PageHeight = e.NewSize.Height;
            }
        }

        private void MainAppPage_OnMouseMove(object sender, MouseEventArgs e)
        {
            var pos = e. GetPosition(this);
            var wCenterCoordsY = this.ActualHeight/ 2;
            var wCenterCoordsX = this.ActualWidth/ 2;
            var cPosY = pos.Y - wCenterCoordsY;
            var cPosX = pos.X - wCenterCoordsX;
            _context.MouseMove(cPosX, cPosY, wCenterCoordsX, wCenterCoordsY);
        }
    }
}
