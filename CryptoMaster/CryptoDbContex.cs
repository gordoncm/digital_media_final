namespace CryptoMaster
{
    using CryptoMaster.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;

    public class CryptoDbContex : DbContext
    {
        public DbSet<CryptoModel> Coins {  get; set; }
        public DbSet<UserModel> Users { get; set; }

        public DbSet<PortfolioModel> Portfolio { get; set; }

        public string DbPath {  get; set; }

        public CryptoDbContex()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "coins.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite($"Data Source={DbPath}");
    }
}
