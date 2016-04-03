using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Makao.Hub
{
    public static class PlayerNameCreator
    {
        static Random rand = new Random();
        static List<string> firstPart = new List<string>() { "Szalony", "Zaczarowany", "Dziwny", "Zużyty", "Śmieszny", "Tajemniczy", "Elegancki", "Piękny" };
        static List<string> secondPart = new List<string>() { "ołówek", "tamburyn", "krążek", "stolik", "klapek", "gwizdek", "grzebień", "kapelusz" };

        public static string GetRandomName()
        {
            return String.Format("{0} {1}", firstPart[rand.Next(firstPart.Count)], secondPart[rand.Next(secondPart.Count)]);
        }
    }
}