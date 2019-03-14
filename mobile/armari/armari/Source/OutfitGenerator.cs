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

        public void GenerateOutfit(CalendarOutfit outfit)
        {
            Random rand = new Random();

            int Lindex = rand.Next(Layers.Count);
            int Tindex = rand.Next(Layers.Count);
            int Bindex = rand.Next(Layers.Count);
            int Findex = rand.Next(Layers.Count);

            outfit.Layer = Layers[Lindex];
            outfit.Top = Layers[Lindex];
            outfit.Bottom = Layers[Lindex];
            outfit.Footwear = Layers[Lindex];

            Application.UniversalCalentarLogger.AddOutfit(outfit);
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
