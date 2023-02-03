using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckTracker.Services
{
    public class ImageReader
    {


        public Task<Bitmap> LoadCardImage(string cardCode)
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var bitmap = new Bitmap(assets.Open(new Uri($"/Assets/Riot Games Assets/en_us/img/cards/{cardCode}.png")));

            return Task.FromResult(bitmap);
        }
    }
}
