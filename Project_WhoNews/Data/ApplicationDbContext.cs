using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Project_WhoNews.Models.Entities;

namespace Project_WhoNews.Data
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(200)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressName { get; set; }
        public string PostalCode { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

     
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<ArchiveDb1> archiveDb1 { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryItem> CategoryItems { get; set; }
        public DbSet<Content> Content { get; set; }
        
    }

    }

