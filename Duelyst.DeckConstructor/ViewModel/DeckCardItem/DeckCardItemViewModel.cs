namespace Duelyst.DeckConstructor.ViewModel.DeckCardItem
{
    public class DeckCardItemViewModel
    {
        public DeckCardItemViewModel(int manaCost, string name)
        {
            ManaCost = manaCost;
            Name = name;    
        }

        public int ManaCost { get; set; }

        public string Name { get; set; }
    }
}
