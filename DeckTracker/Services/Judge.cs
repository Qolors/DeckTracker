using DeckTracker.DeckEncoder;
using DeckTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace DeckTracker.Services
{
    public class Judge
    {
        private OpponentDeck opponentDeck;
        private DeckManager deckManager;
        private MetaDecks metaDecks;
        private CardDataBase cardDataBase;
        private List<string> TheCurrentCards;
        public bool IsChanged;
        public List<TodoItem> CurrentGuess { get; private set; }
        public Judge()
        {
            OpponentDeck= new OpponentDeck();
            DeckManager= new DeckManager();
            MetaDecks= new MetaDecks();
            TheCardDataBase = Task.Run(() => LoadCards()).Result;
            CurrentGuess= new List<TodoItem>();
            TheCurrentCards = new List<string>();
        }

        private static async Task<CardDataBase> LoadCards()
        {
            Console.WriteLine("Loading Cards");
            var theCards = await CardDataBase.LoadAllCards();
            return theCards;
        }
        public void GetMetaDecks()
        {
            foreach (var code in MetaDecks.DeckCodes)
            {
                Deck deck = new Deck();
                var cards = DeckManager.GetDeckFromCode(code);
                Console.WriteLine("A Deck");
                foreach (var card in cards)
                {
                    deck.Cards.Add(card.CardCode);
                }
                MetaDecks.Decks.Add(deck);
            }
        }

        public void CheckForNew(IEnumerable<string> codes)
        {
            if (codes.Count() != 0)
            {
                for (int i = 0; i < codes.Count(); i++)
                {
                    OpponentDeck.FoundACard(codes.ElementAt(i));
                }
                GuessADeck();
            }
        }

        private void GuessADeck()
        {
            if (OpponentDeck.DeckList.Count < 2) 
            {
                //CurrentGuess = new List<TodoItem>(){ new TodoItem { Description = "Need more cards.." } }; 
                return; 
            }
            Dictionary<HashSet<string>, int> dic = new Dictionary<HashSet<string>, int>();
            foreach (var deck in MetaDecks.Decks)
            {
                int matches = 0;
                foreach (var card in OpponentDeck.DeckList)
                {
                    if (deck.Cards.Contains(card))
                    {
                        matches += 1;
                    }
                }
                Console.WriteLine("Matched: " + matches);
                if (matches > 1)
                {
                    dic.Add(deck.Cards, matches);
                }
            }
            if (dic.Count > 0)
            {
                
                var bestMatch = dic.OrderByDescending(deck => deck.Value).FirstOrDefault();
                var matchedDeck = bestMatch.Key.ToList();
                

                if (matchedDeck != null)
                {
                    IEnumerable<string> inCurrentCards = TheCurrentCards.Except(matchedDeck);
                    IEnumerable<string> inMatchedDeck = matchedDeck.Except(inCurrentCards);

                    IsChanged = (inCurrentCards.Any() && inMatchedDeck.Any());
                    Console.WriteLine(IsChanged);
                    if (!IsChanged) { return; }
                    TheCurrentCards = matchedDeck;
                    List<TodoItem> todoItems = new List<TodoItem>();
                    foreach (var card in matchedDeck)
                    {
                        foreach (var c in TheCardDataBase.Cards)
                        {
                            if (card == c.cardCode)
                            {
                                todoItems.Add(new TodoItem
                                {
                                    Description = c.name,
                                    Set = c.set.ToLower(),
                                    Url = $"https://dd.b.pvp.net/latest/{c.set.ToLower()}/en_us/img/cards/{c.cardCode}.png"

                                });
                            }
                        }
                    }
                    CurrentGuess = todoItems;
                }
            }
        }

        public OpponentDeck OpponentDeck
        {
            get => opponentDeck;
            set => opponentDeck = value;
        }

        public CardDataBase TheCardDataBase
        {
            get => cardDataBase;
            set => cardDataBase = value;
        }
        public DeckManager DeckManager
        {
            get => deckManager;
            set => deckManager = value;
        }

        public MetaDecks MetaDecks
        {
            get => metaDecks;
            set => metaDecks = value;
        }

    }
}
