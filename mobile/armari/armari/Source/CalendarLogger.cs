using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using Newtonsoft.Json;

namespace armari
{
    [Preserve]
    public class CalendarOutfit
    {
        private readonly DateTime CreatedDate;
        public string Layer { get; set; }
        public string Top { get; set; }
        public string Bottom { get; set; }
        public string Footwear { get; set; }
        public bool Worn { get; set; }

        public CalendarOutfit()
        {
            CreatedDate = DateTime.Today;
            Layer = "layer.png";
            Top = "top.png";
            Bottom = "bottom.png";
            Footwear = "footwear.png";
            Worn = false;
        }

        public DateTime getCreatedDate() 
        {
            return CreatedDate;
        }
    }

    [Preserve]
    public class CalendarOutfitContainer
    {
        public List<CalendarOutfit> Outfits { get; set; }

        public CalendarOutfitContainer()
        {
            Outfits = new List<CalendarOutfit>();
        }

        public int FindOutfit(CalendarOutfit outfit)
        {
            return Outfits.FindIndex(a => a.getCreatedDate() == outfit.getCreatedDate());
        }

        public void AddOutfit(CalendarOutfit outfit)
        {
            int index = FindOutfit(outfit);

            if (index != -1)
            {
                Outfits[index].Layer = outfit.Layer;
                Outfits[index].Top = outfit.Top;
                Outfits[index].Bottom = outfit.Bottom;
                Outfits[index].Footwear = outfit.Footwear;
            }
            else
            {
                Outfits.Add(outfit);
            }
        }
    }

    public class CalendarLogger
    {
        public CalendarOutfitContainer MyCalendarOutfitContainer { get; set; }

        public CalendarLogger()
        {
            MyCalendarOutfitContainer = new CalendarOutfitContainer();
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filename = Path.Combine(documents, "outfits.json");

            if (File.Exists(filename))
            {
                // Load json
                string savedContainer = File.ReadAllText(filename);
                MyCalendarOutfitContainer = JsonConvert.DeserializeObject<CalendarOutfitContainer>(savedContainer);
            }
        }

        public void AddOutfit(CalendarOutfit outfit)
        {
            MyCalendarOutfitContainer.AddOutfit(outfit);

            // Save changes to file
            var json = JsonConvert.SerializeObject(MyCalendarOutfitContainer, Newtonsoft.Json.Formatting.Indented);

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filename = Path.Combine(documents, "outfits.json");
            File.WriteAllText(filename, json);
        }

        public int NumOfOutfits()
        {
            return MyCalendarOutfitContainer.Outfits.Count;
        }

    }
}
