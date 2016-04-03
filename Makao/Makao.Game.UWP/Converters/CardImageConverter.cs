using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Makao.Game.Converters
{
    public class CardImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return new BitmapImage();
            }

            var card = value as Card;
            var imagePath = string.Format("ms-appx:///Assets/Card/{0}{1}.png", card.Rank.ToString(), card.Suit.ToString());
            var image = new BitmapImage(new Uri(imagePath));
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}