using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using IfcDb.Models;
using IfcDb.Models.Entities;

namespace IfcDb
{

    public class IfcDbContext : DbContext
    {
        public DbSet<IfcAttributeTypeEntity> AttriubteTypes { get; set; }
        public DbSet<IfcAttributeEntity> Attributes { get; set; }
        public DbSet<IfcValueEntity> Values { get; set; }
        public DbSet<IfcObjTypeEntity> ObjectTypes { get; set; }
        public DbSet<IfcObjDestinationEntity> ObjectDestinations { get; set; }
        public DbSet<IfcObjEntity> Objects { get; set; }
        public DbSet<IfcFileEntity> Files { get; set; }

        public IfcDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IfcObjEntity>().HasMany(e => e.Attributes).WithMany();
            modelBuilder.Entity<IfcObjEntity>().HasOne(e => e.Type).WithMany().HasForeignKey(e => e.TypeId);
            modelBuilder.Entity<IfcObjEntity>().HasOne(e => e.Destination).WithMany().HasForeignKey(e => e.DestinationId);
            modelBuilder.Entity<IfcObjEntity>().Property(e => e.DestinationId).HasConversion<int>();

            modelBuilder.Entity<IfcObjDestinationEntity>().HasIndex(e => e.Destination).IsUnique();
            modelBuilder.Entity<IfcObjDestinationEntity>().Property(e => e.Id).HasConversion<int>();

            modelBuilder.Entity<IfcAttributeEntity>().HasOne(e => e.Obj).WithOne().HasForeignKey<IfcAttributeEntity>(e => e.ObjId);
            modelBuilder.Entity<IfcAttributeEntity>().HasOne(e => e.Value).WithOne().HasForeignKey<IfcAttributeEntity>(e => e.ValueId);
            modelBuilder.Entity<IfcAttributeEntity>().Property(e => e.TypeId).HasConversion<int>();

            modelBuilder.Entity<IfcAttributeTypeEntity>().Property(e => e.Id).HasConversion<int>();
            modelBuilder.Entity<IfcAttributeTypeEntity>().HasIndex(e => e.Name).IsUnique();

            modelBuilder.Entity<IfcFileEntity>().HasIndex(e => e.Name).IsUnique();

            // Data initialization
            var valueTypes = new List<IfcAttributeTypeEntity>((int)IfcAttributeType.Count);
            for (IfcAttributeType valueType = (IfcAttributeType)1; valueType < IfcAttributeType.Count; ++valueType)
            {
                valueTypes.Add(new IfcAttributeTypeEntity { Id = valueType, Name = valueType.ToString() });
            }

            var objDestinations = new List<IfcObjDestinationEntity>((int)IfcObjDestination.Count);
            for (IfcObjDestination destination = (IfcObjDestination)1; destination < IfcObjDestination.Count; ++destination)
            {
                objDestinations.Add(new IfcObjDestinationEntity { Id = destination, Destination = destination.ToString() });
            }

            modelBuilder.Entity<IfcAttributeTypeEntity>().HasData(valueTypes);
            modelBuilder.Entity<IfcObjDestinationEntity>().HasData(objDestinations);
        }
    }
}