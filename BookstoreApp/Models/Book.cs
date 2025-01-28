using System;

namespace BookstoreApp.Models
{
    public class Book
    {
        public int Index { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int Likes { get; set; }
        public int Reviews { get; set; }
        public string CoverImageUrl { get; set; }
        public int Pages { get; set; }
        public List<string> Comments { get; set; }
    }
}
