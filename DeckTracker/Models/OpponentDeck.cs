using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckTracker.Models
{
    public class OpponentDeck
    {
        HashSet<string> deckList;
        public OpponentDeck()
        {
            DeckList = new();
        }

        public bool FoundACard(string cardCode)
        {
            if (!DeckList.Contains(cardCode))
            {
                DeckList.Add(cardCode);
                return true;
            }
            return false;
        }

        public IEnumerable<string> ShowHand()
        {
            return DeckList;
        }

        public HashSet<string> DeckList
        {
            get => deckList;
            set => deckList = value;
        }
        
    }
}
