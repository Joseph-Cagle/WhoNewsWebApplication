using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Project_WhoNews.Models.Entities
{
    public partial class Content
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string BodyContent { get; set; }
        public DateTime DatePublished { get; set; }
        public string HeaderContent { get; set; }

        [Display(Name = "Choose photo for article")]
        public string Image { get; set; }

        [NotMapped]
        public IFormFile ImagePath { get; set; }

        public bool? IsArchived { get; set; }
        public int CatItemId { get; set; }
    }
}
