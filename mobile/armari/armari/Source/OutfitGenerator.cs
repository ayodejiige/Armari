using System;
using System.Collections.Generic;

namespace armari
{
    public class OutfitGenerator
    {
        private List<string> Layers { get; set; }
        private List<string> Tops { get; set; }
        private List<string> Bottoms { get; set; }
        private List<string> Footwear { get; set; }

        public CalendarOutfit GenerateOutfit(CalendarOutfit outfit)
        {
            Random rand = new Random();

            int Lindex = rand.Next(Layers.Count);
            int Tindex = rand.Next(Tops.Count);
            int Bindex = rand.Next(Bottoms.Count);
            int Findex = rand.Next(Footwear.Count);

            outfit.Layer = Layers[Lindex];
            outfit.Top = Tops[Tindex];
            outfit.Bottom = Bottoms[Bindex];
            outfit.Footwear = Footwear[Findex];

            Application.UniversalCalentarLogger.AddOutfit(outfit);

            return outfit;
        }

        public OutfitGenerator(List<string> layers, List<string> tops, List<string> bottoms, List<string> footwear)
        {
            Layers = layers;
            Tops = tops;
            Bottoms = bottoms;
            Footwear = footwear;
        }
    }
}
