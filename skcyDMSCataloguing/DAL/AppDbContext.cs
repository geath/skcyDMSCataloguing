using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using skcyDMSCataloguing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.DAL
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Box> Boxes { get; set; }
        public DbSet<BoxCreator> BoxCreators { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<CustAccData> CustAccDataEntries { get; set; }
        public DbSet<CustData> CustDataEntries { get; set; }
        public DbSet<CustRelData> CustRelDataEntries { get; set; }
        public DbSet<PrjHelix1> PrjHelixes1 { get; set; }
        public DbSet<PrjVelocity1> PrjVelocitys1 { get; set; }
        public DbSet<PrjVelocity2> PrjVelocitys2 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Box>()
                .ToTable("Box");
            modelBuilder.Entity<BoxCreator>()
                .ToTable("BoxCreator");
            modelBuilder.Entity<Folder>()
                .ToTable("Folder");
            modelBuilder.Entity<CustAccData>()
               .ToTable("BankAccount");
            modelBuilder.Entity<CustData>()
              .ToTable("Customer");
            modelBuilder.Entity<CustRelData>()
              .ToTable("CustomerAccount");           
            modelBuilder.Entity<PrjHelix1>()
              .ToTable("PrjHelix1");
            modelBuilder.Entity<PrjVelocity1>()
              .ToTable("PrjVelocity1");
            modelBuilder.Entity<PrjVelocity2>()
              .ToTable("PrjVelocity2");



            modelBuilder.Entity<CustData>()
                .HasAlternateKey(cd => cd.CIFNo);

            modelBuilder.Entity<CustAccData>()
                .HasAlternateKey(cad => cad.CustAccountNo);


            modelBuilder.Entity<CustRelData>()
                .HasOne(crd => crd.CustData)
                .WithMany(cd => cd.CustRelDataEntries)
                .HasForeignKey(crd => crd.CustCIFNo)
                .HasPrincipalKey(cd=>cd.CIFNo);

            modelBuilder.Entity<CustRelData>()
                .HasOne(crd => crd.CustAccData)
                .WithMany(cad => cad.CustRelDataEntries)
                .HasForeignKey(crd => crd.CustAccountNo)
                .HasPrincipalKey(cad=>cad.CustAccountNo);

            modelBuilder.Entity<Folder>()
               .HasOne(fld => fld.CustData)
               .WithMany(cdt => cdt.Folders)
               .HasForeignKey(fld => fld.CustDataCIFNo)
               .HasPrincipalKey(cdt => cdt.CIFNo);

            modelBuilder.Entity<PrjHelix1>()
                .HasOne(hlx => hlx.CustData)
                .WithMany(cd => cd.PrjHelixes1)
                .HasForeignKey(hlx => hlx.CustDataCIFNo)
                .HasPrincipalKey(cd => cd.CIFNo);

            modelBuilder.Entity<PrjVelocity1>()
               .HasOne(vl1 => vl1.CustData)
               .WithMany(cd => cd.PrjVelocities1)
               .HasForeignKey(vl1 => vl1.CustDataCIFNo)
               .HasPrincipalKey(cd => cd.CIFNo);

            modelBuilder.Entity<PrjVelocity2>()
               .HasOne(vl2 => vl2.CustData)
               .WithMany(cd => cd.PrjVelocities2)
               .HasForeignKey(vl2 => vl2.CustDataCIFNo)
               .HasPrincipalKey(cd => cd.CIFNo);

            modelBuilder.Entity<Box>()
                .Property(b => b.DateBoxCreated)
                .HasDefaultValueSql("getdate()");



        }

    }
}


/* https://stackoverflow.com/questions/39911563/why-do-i-need-hashset-in-many-to-many-relation */