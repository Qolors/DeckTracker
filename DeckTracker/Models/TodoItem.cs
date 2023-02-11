using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DeckTracker.Models
{
    public class TodoItem : ReactiveObject
    {
        private Bitmap cover;
        private bool displayImage = false;
        public string Description { get; set; }
        public string Set { get; set; }
        public string Url { get; set; }
        public Bitmap? Cover
        {
            get => cover;
            private set => cover = value;
        }

        public async Task LoadImage(HttpClient client)
        {
            await using (var imageStream = await LoadImageAsync(client))
            {
                Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 300));
            }
        }

        public async Task<Stream> LoadImageAsync(HttpClient client)
        {
            var data = await client.GetByteArrayAsync(this.Url);
            return new MemoryStream(data);
        }

        public bool DisplayImage
        {
            get => displayImage;
            set => this.RaiseAndSetIfChanged(ref displayImage, value);
        }
    }
}
