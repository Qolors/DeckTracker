using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckTracker.Models
{
    public class MetaDecks
    {
        public List<string> DeckCodes;
        public List<Deck> Decks;
        public MetaDecks() 
        {
            DeckCodes = new List<string>();
            Decks = new List<Deck>();
        }
        async public Task LoadDecks()
        {
            var codeFile = await File.ReadAllLinesAsync($@"Assets/codes.txt");
            this.DeckCodes = new List<string>(codeFile);
        }
    }

    public class Deck
    {
        public HashSet<string> Cards { get; set; }
        public HashSet<string> Regions { get; set; }

        public Deck()
        {
            Cards = new HashSet<string>();
            Regions = new HashSet<string>();
        }

    }



    
}
