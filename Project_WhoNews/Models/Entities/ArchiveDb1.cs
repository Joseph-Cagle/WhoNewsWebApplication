using System;

namespace Project_WhoNews.Models.Entities
{
    public partial class ArchiveDb1
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ArchiveDate { get; set; }
        public string Author { get; set; }
        public string BodyContent { get; set; }
        public DateTime DatePublished { get; set; }

        public Content content { get; set; }
        public string HeaderContent { get; set; }
        public string Image { get; set; }
    }
}
