using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Makao.Game.Converters
{
    public class NumberOfPlayersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var gameroom = value as GameRoom;
            return string.Format("{0}/{1} players", gameroom.Players != null ? gameroom.Players.Count : 0, gameroom.NumberOfPlayers);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}