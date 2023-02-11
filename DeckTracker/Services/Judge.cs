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
        private static MetaDecks metaDecks;
        private CardDataBase cardDataBase;
        private List<string> TheCurrentCards;
        private Dictionary<string, int> CurrentRegions;
        public bool IsChanged;
        public string? FirstRegion { get; set; } = null;
        public string? SecondRegion { get; set; } = null;
        public List<TodoItem> CurrentGuess { get; private set; }
        public Judge()
        {
            OpponentDeck= new OpponentDeck();
            DeckManager= new DeckManager();
            MetaDecks= new MetaDecks();
            TheCardDataBase = Task.Run(() => LoadCards()).Result;
            CurrentGuess= new List<TodoItem>();
            TheCurrentCards = new List<string>();
            CurrentRegions = new Dictionary<string, int>();
        }

        private static async Task<CardDataBase> LoadCards()
        {
            await MetaDecks.LoadDecks();
            var theCards = await CardDataBase.LoadAllCards();
            return theCards;
        }
        public void GetMetaDecks()
        {
            foreach (var codes in MetaDecks.DeckCodes)
            {

                Deck deck = new Deck();
                var cards = DeckManager.GetDeckFromCode(codes);

                foreach (var card in cards)
                {
                    deck.Cards.Add(card.CardCode);
                    var regions = TheCardDataBase.Cards.Where(c => c.cardCode == card.CardCode).Select(c => c.regions).FirstOrDefault();
                    if (regions != null)
                    {
                        foreach (var region in regions)
                        {
                            deck.Regions.Add(region);
                        }
                    }  
                }

                MetaDecks.Decks.Add(deck);
            }
        }

        private void UpdateRegions()
        {
            var sortedDict = from entry in CurrentRegions orderby entry.Value descending select entry.Key;
            FirstRegion = sortedDict.ElementAt(0);
            if (CurrentRegions.Count > 1)
            {
                SecondRegion = sortedDict.ElementAt(1);
            }
        }
        public bool CheckForNew(IEnumerable<string> codes)
        {
            if (codes.Count() != 0)
            {
                
                for (int i = 0; i < codes.Count(); i++)
                {
                    if (OpponentDeck.FoundACard(codes.ElementAt(i)))
                    {
                        var regions = TheCardDataBase.Cards.Where(c => c.cardCode == codes.ElementAt(i)).Select(c => c.regions).First();
                        foreach(var region in regions)
                        {
                            if (!CurrentRegions.ContainsKey(region))
                            {
                                CurrentRegions.Add(region, 1);
                            }
                            else
                            {
                                CurrentRegions[region]++;
                            }
                        }
                    }
                }

                if (CurrentRegions.Count > 0)
                {
                    UpdateRegions();
                }

                return GuessADeck();

                
            }
            return false;
        }

        private bool GuessADeck()
        {
            if (OpponentDeck.DeckList.Count < 2) 
            {
                return false; 
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
                
                if (matches > 0)
                {
                    Console.WriteLine("Matched: " + matches);
                    if (FirstRegion != null)
                    {
                        Console.WriteLine(FirstRegion);
                        if (!deck.Regions.Any(r => r == FirstRegion))
                        {
                            if (SecondRegion == null)
                            {
                                return false;
                            }
                            else
                            { 
                                Console.WriteLine(SecondRegion);
                                if (!deck.Regions.Any(r => r == SecondRegion))
                                {
                                    return false;
                                }
                            }
                            
                        }
                        dic.Add(deck.Cards, matches);

                        
                    }
                    else
                    {
                        return false;
                    }
                    
                }
            }

            if (dic.Count > 0)
            {
                
                var bestMatch = dic.OrderByDescending(deck => deck.Value).FirstOrDefault();

                var matchedDeck = bestMatch.Key.ToList();

                if (matchedDeck != null)
                {

                    IEnumerable<string> inMatchedDeck = matchedDeck.Except(TheCurrentCards);

                    IsChanged = (inMatchedDeck.Any());

                    Console.WriteLine(IsChanged);

                    if (!IsChanged) { return false; }

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

                    return true;
                }
            }
            return false;
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

        public static MetaDecks MetaDecks
        {
            get => metaDecks;
            set => metaDecks = value;
        }

    }
}
