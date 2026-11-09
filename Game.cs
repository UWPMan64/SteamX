using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

namespace SteamX.Models
{
    public class Game
    {
        public Game()
        {
            Screenshots = new ObservableCollection<BitmapImage>();
        }

        public int AppId { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string ImageUrl { get; set; }
        public BitmapImage Artwork { get; set; }

        public string Description { get; set; }
        public string Developers { get; set; }
        public string Publishers { get; set; }
        public string ReleaseDate { get; set; }

        public string HeaderImageUrl { get; set; }
        public BitmapImage HeaderArtwork { get; set; }

        public ObservableCollection<BitmapImage> Screenshots
        {
            get;
            set;
        }
    }
}