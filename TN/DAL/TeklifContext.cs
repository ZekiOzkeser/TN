using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TN.Models;

namespace TN.DAL
{
    public class TeklifContext:DbContext
    {
        public TeklifContext():base("cstr")
        {
                
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Urunler> Urunlers { get; set; }
       
        public DbSet<Kategoriler> Kategorilers { get; set; }
        public DbSet<Il> Il { get; set; }
        public DbSet<Iletisim> Iletisims { get; set; }
        public DbSet<Ilce> Ilce { get; set; }
        public DbSet<Durum> Durums { get; set; }
        public DbSet<Resimler> Resimlers { get; set; }
        public DbSet<Mesaj> Mesajs { get; set; }
        public DbSet<AnaGorsel> AnaGorsels { get; set; }

        public DbSet<Teklifler> Tekliflers { get; set; }

        public DbSet<AltKategoriler> AltKategorilers { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
        }


    }
}