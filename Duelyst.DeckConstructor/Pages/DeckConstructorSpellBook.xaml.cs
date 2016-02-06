using System.Windows;
using System.Windows.Controls;
using Duelyst.DeckConstructor.ViewModel.Ifaces;

namespace Duelyst.DeckConstructor.Pages
{
    /// <summary>
    /// Interaction logic for DeckConstructorSpellBook.xaml
    /// </summary>
    public partial class DeckConstructorSpellBook : Page
    {
        private IResizeableViewModel _context;

        public DeckConstructorSpellBook()
        {
            InitializeComponent();

            _context = DataContext as IResizeableViewModel;
        }


        private void DeckConstructorSpellBook_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_context != null)
            {
                _context.PageWidth = e.NewSize.Width;
                _context.PageHeight = e.NewSize.Height;
            }
        }
    }
}
