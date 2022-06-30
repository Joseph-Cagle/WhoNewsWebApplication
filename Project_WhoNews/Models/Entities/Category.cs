using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_WhoNews.Models.Entities
{
    public partial class Category
    {


        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        
      
    }
}
