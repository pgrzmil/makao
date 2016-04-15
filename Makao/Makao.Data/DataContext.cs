using Makao.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Data
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DataContext")
        {
        }

        public DbSet<ArchiveGame> ArchiveGames { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<GameRoom> GameRooms { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //one-to-one realation
            modelBuilder.Entity<Player>()
                .HasRequired(p => p.User)
                .WithRequiredPrincipal(u => u.Player);
        }
    }
}
