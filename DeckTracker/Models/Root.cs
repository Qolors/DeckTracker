using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckTracker.Models
{
    public class Rectangle
    {
        public int? CardID;
        public string? CardCode;
        public int? TopLeftX;
        public int? TopLeftY;
        public int? Width;
        public int? Height;
        public bool LocalPlayer;
    }

    public class Root
    {
        public string? PlayerName;
        public string? OpponentName;
        public string? GameState;
        public Screen? Screen;
        public List<Rectangle>? Rectangles;
    }

    public class Screen
    {
        public int? ScreenWidth;
        public int? ScreenHeight;
    }
}
