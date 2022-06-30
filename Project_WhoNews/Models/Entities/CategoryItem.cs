using System;

namespace Project_WhoNews.Models.Entities
{
    public partial class CategoryItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public DateTime ItemReleaseDate { get; set; }
        public string Description { get; set; }

  
    }
}
