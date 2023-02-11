using DeckTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DeckTracker.Services
{
    public class Database
    {

        Root boardState;
        public bool CardSeen = false;
        private const string URL = "http://127.0.0.1:21337/positional-rectangles";

        public Root BoardState
        {
            get => boardState;
            private set => boardState = value;
        }

        private Database(Root BoardState)
        {
            this.BoardState = BoardState;
        }

        async public static Task<Database> PingClientAsync(HttpClient client)
        {
            var response = await client.GetAsync(URL).ConfigureAwait(false);
            var stringResponse = await response.Content.ReadAsStringAsync();

            var tmpRoot = JsonConvert.DeserializeObject<Root>(stringResponse);
            return new Database(tmpRoot);
        }
        public IEnumerable<string> GetItems()
        {
            List<string> items = new List<string>();
            IEnumerable<string> cards;

            foreach(Rectangle rec in BoardState.Rectangles)
            {
                if (!rec.LocalPlayer)
                {
                    if (rec.CardCode == "face")
                    {
                        continue;
                    }
                    CardSeen = true;
                    items.Add(rec.CardCode);
                }
            }

            cards = items.ToArray();
            return cards;
        }

        


    }
    
}
