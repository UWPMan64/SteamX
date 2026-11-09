using Windows.UI.Xaml.Media.Imaging;

namespace SteamX.Models
{
    public class Game
    {
        public int AppId { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string ImageUrl { get; set; }
        public BitmapImage Artwork { get; set; }
    }
}
