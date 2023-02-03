using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeckTracker.Models
{
    public class TodoItem
    {
        private Bitmap cover;
        public string Description { get; set; }
        public string Set { get; set; }
        public string Url { get; set; }
        public Bitmap? Cover
        {
            get => cover;
            private set => cover = value;
        }
        public async Task LoadImage()
        {
            await using (var imageStream = await LoadImageAsync())
            {
                Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 300));
            }
        }

        public async Task<Stream> LoadImageAsync()
        {
            var client = new HttpClient();

            var data = await client.GetByteArrayAsync(this.Url);
            return new MemoryStream(data);
        }
    }
}
