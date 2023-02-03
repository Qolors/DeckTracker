using Avalonia.Platform;
using Avalonia;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DeckTracker.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class Asset
    {
        public string gameAbsolutePath { get; set; }
        public string fullAbsolutePath { get; set; }
    }

    public class CardDataBase
    {
        public List<Card> cards;
        private CardDataBase(List<Card> Cards)
        {
            this.Cards = Cards;
        }
        public List<Card> Cards 
        {
            get => cards;
            set => cards = value;
        }
        async public static Task<CardDataBase> LoadAllCards()
        {
            List<List<Card>> allCards = new List<List<Card>>();
            List<string> jSons = new List<string>
            {
                "set1-en_us.json",
                "set2-en_us.json",
                "set3-en_us.json",
                "set4-en_us.json",
                "set5-en_us.json",
                "set6-en_us.json"
            };

            foreach (var j in jSons)
            {
                var fs = new StreamReader($@"Assets/Riot Games Assets/{j}");
                var stringRead = await fs.ReadToEndAsync().ConfigureAwait(false);
                var cardData = JsonConvert.DeserializeObject<List<Card>>(stringRead);
                allCards.Add(cardData);
            }

            
            return new CardDataBase(allCards.SelectMany(x => x).ToList());
        }
    }



    public class Card
    {
        public List<object> associatedCards { get; set; }
        public List<string> associatedCardRefs { get; set; }
        public List<Asset> assets { get; set; }
        public List<string> regions { get; set; }
        public List<string> regionRefs { get; set; }
        public int attack { get; set; }
        public int cost { get; set; }
        public int health { get; set; }
        public string description { get; set; }
        public string descriptionRaw { get; set; }
        public string levelupDescription { get; set; }
        public string levelupDescriptionRaw { get; set; }
        public string flavorText { get; set; }
        public string artistName { get; set; }
        public string name { get; set; }
        public string cardCode { get; set; }
        public List<string> keywords { get; set; }
        public List<string> keywordRefs { get; set; }
        public string spellSpeed { get; set; }
        public string spellSpeedRef { get; set; }
        public string rarity { get; set; }
        public string rarityRef { get; set; }
        public List<string> subtypes { get; set; }
        public string supertype { get; set; }
        public string type { get; set; }
        public bool collectible { get; set; }
        public string set { get; set; }
    }

}
