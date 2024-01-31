using AmazingTech.InternSystem.Data.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AmazingTech.InternSystem.Data
{
    public class AppDbContext : IdentityDbContext<User>, IAppDbContext
    {
        IConfiguration _configuration;

        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DatabaseFacade DatabaseFacade => throw new NotImplementedException();

        public DbSet<User> Users { get; set; }
     
        public DbSet<CongNghe> CongNghes { get; set; } = null!;
        public DbSet<NhomZalo> NhomZalos { get; set; }
        public DbSet<UserNhomZalo> UserNhomZalos { get; set; }
        public DbSet<LichPhongVan> LichPhongVans { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }
        public DbSet<KiThucTap> KiThucTaps { get; set; }
        public DbSet<TruongHoc> TruongHocs { get; set; }
        public DbSet<InternInfo> InternInfos { get; set; }
        public DbSet<UserDuAn> InternDuAns { get; set; }
        public DbSet<DuAn> DuAns { get; set; }
        public DbSet<CongNgheDuAn> CongNgheDuAns { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<ViTri> ViTris { get; set; }
        public DbSet<UserViTri> UserViTris { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Cauhoi> cauhois { get; set; }
        public DbSet<CauhoiCongnghe> cauhoiCongnghes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"),
                options => options.EnableRetryOnFailure());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CongNgheDuAn>()
                .HasOne(lp => lp.DuAn)
                .WithMany(u => u.CongNgheDuAns)
                .HasForeignKey(lp => lp.IdDuAn)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<CongNgheDuAn>()
               .HasOne(lp => lp.CongNghe)
               .WithMany(u => u.CongNgheDuAns)
               .HasForeignKey(lp => lp.IdCongNghe)
               .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<User>()
            //    .HasMany(u => u.Roles)
            //    .WithMany(r => r.Users)
            //    .UsingEntity(j => j.ToTable("UserRole"));

            modelBuilder.Entity<LichPhongVan>()
                .HasOne(lp => lp.NguoiPhongVan)
                .WithMany(u => u.LichPhongVans_PhongVan)
                .HasForeignKey(lp => lp.IdNguoiPhongVan)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<LichPhongVan>()
                .HasOne(lp => lp.NguoiDuocPhongVan)
                .WithMany(u => u.LichPhongVans_DuocPhongVan)
                .HasForeignKey(lp => lp.IdNguoiDuocPhongVan)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ThongBao>()
                .HasOne(lp => lp.NguoiGui)
                .WithMany(u => u.ThongBao_NguoiGui)
                .HasForeignKey(lp => lp.IdNguoiGui)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ThongBao>()
                .HasOne(lp => lp.NguoiNhan)
                .WithMany(u => u.ThongBao_NguoiNhan)
                .HasForeignKey(lp => lp.IdNguoiNhan)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(lp => lp.NguoiComment)
                .WithMany(u => u.Comments)
                .HasForeignKey(lp => lp.IdNguoiComment)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(lp => lp.NguoiDuocComment)
                .WithMany(u => u.Comments)
                .HasForeignKey(lp => lp.IdNguoiDuocComment)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserNhomZalo>()
                .HasOne(zl => zl.User)
                .WithMany(u => u.UserNhomZalos)
                .HasForeignKey(zl => zl.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserNhomZalo>()
                .HasOne(zl => zl.NhomZalo)
                .WithMany(u => u.UserNhomZalos)
                .HasForeignKey(zl => zl.IdNhomZalo)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserDuAn>()
                .HasOne(zl => zl.User)
                .WithMany(u => u.UserDuAns)
                .HasForeignKey(zl => zl.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserDuAn>()
                .HasOne(zl => zl.User)
                .WithMany(u => u.UserDuAns)
                .HasForeignKey(zl => zl.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            //Add moi bang UserViTri
            modelBuilder.Entity<UserViTri>()
               .HasOne(zl => zl.User)
               .WithMany(u => u.UserViTris)
               .HasForeignKey(zl => zl.UsersId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserViTri>()
               .HasOne(zl => zl.ViTri)
               .WithMany(u => u.UserViTris)
               .HasForeignKey(zl => zl.ViTrisId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CauhoiCongnghe>()
               .HasOne(zl => zl.cauhoi)
               .WithMany(u => u.CauhoiCongnghe)
               .HasForeignKey(zl => zl.IdCauhoi)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<CauhoiCongnghe>()
              .HasOne(zl => zl.CongNghe)
              .WithMany(u => u.CauhoiCongnghe)
              .HasForeignKey(zl => zl.IdCongNghe)
              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InternInfo>()
             .HasOne(zl => zl.Truong)
             .WithMany(u => u.Interns)
             .HasForeignKey(zl => zl.IdTruong)
             .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<KiThucTap>()
             .HasOne(zl => zl.Truong)
             .WithMany(u => u.KiThucTaps)
             .HasForeignKey(zl => zl.IdTruong)
             .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InternInfo>()
             .HasOne(zl => zl.KiThucTap)
             .WithMany(u => u.Interns)
             .HasForeignKey(zl => zl.KiThucTapId)
             .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PhongVan>()
            .HasOne(zl => zl.LichPhongVan)
            .WithMany(u => u.PhongVans)
            .HasForeignKey(zl => zl.IdLichPhongVan)
            .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<PhongVan>()
            .HasOne(zl => zl.CauhoiCongnghes)
            .WithMany(u => u.PhongVans)
            .HasForeignKey(zl => zl.IdCauHoiCongNghe)
            .OnDelete(DeleteBehavior.NoAction);

            // Auto generate when inserted/updated

            //     modelBuilder.Entity<Comment>()
            //         .Property(e => e.CreatedTime)
            // .ValueGeneratedOnAdd();
            //     modelBuilder.Entity<Comment>()
            //         .Property(e => e.LastUpdatedTime)
            //         .ValueGeneratedOnAddOrUpdate();

            //     modelBuilder.Entity<CongNghe>()
            //         .Property(e => e.CreatedTime)
            //         .ValueGeneratedOnAdd();
            //     modelBuilder.Entity<CongNghe>()
            //         .Property(e => e.LastUpdatedTime)
            //         .ValueGeneratedOnAddOrUpdate();

            //     modelBuilder.Entity<CongNgheDuAn>()
            //         .Property(e => e.CreatedTime)
            //         .ValueGeneratedOnAdd();
            //     modelBuilder.Entity<CongNgheDuAn>()
            //         .Property(e => e.LastUpdatedTime)
            //         .ValueGeneratedOnAddOrUpdate();

            //     modelBuilder.Entity<DuAn>()
            //         .Property(e => e.CreatedTime)
            //         .ValueGeneratedOnAdd();
            //     modelBuilder.Entity<DuAn>()
            //         .Property(e => e.LastUpdatedTime)
            //         .ValueGeneratedOnAddOrUpdate();

            //     modelBuilder.Entity<InternInfo>()
            //         .Property(e => e.CreatedTime)
            //         .ValueGeneratedOnAdd();
            //     modelBuilder.Entity<InternInfo>()
            //         .Property(e => e.LastUpdatedTime)
            //         .ValueGeneratedOnAddOrUpdate();

            //     modelBuilder.Entity<KiThucTap>()
            //         .Property(e => e.CreatedTime)
            //         .ValueGeneratedOnAdd();
            //     modelBuilder.Entity<KiThucTap>()
            //         .Property(e => e.LastUpdatedTime)
            //         .ValueGeneratedOnAddOrUpdate();

            //     modelBuilder.Entity<LichPhongVan>()
            //         .Property(e => e.CreatedTime)
            //         .ValueGeneratedOnAdd();
            //     modelBuilder.Entity<LichPhongVan>()
            //         .Property(e => e.LastUpdatedTime)
            //         .ValueGeneratedOnAddOrUpdate();

            //     modelBuilder.Entity<ViTri>()
            //         .Property(e => e.CreatedTime)
            //         .ValueGeneratedOnAdd();
            //     modelBuilder.Entity<ViTri>()
            //         .Property(e => e.LastUpdatedTime)
            //         .ValueGeneratedOnAddOrUpdate();

            //     modelBuilder.Entity<UserNhomZalo>()
            //         .Property(e => e.CreatedTime)
            //         .ValueGeneratedOnAdd();
            //     modelBuilder.Entity<UserNhomZalo>()
            //         .Property(e => e.LastUpdatedTime)
            //         .ValueGeneratedOnAddOrUpdate();

            //     modelBuilder.Entity<UserDuAn>()
            //         .Property(e => e.CreatedTime)
            //         .ValueGeneratedOnAdd();
            //     modelBuilder.Entity<UserDuAn>()
            //         .Property(e => e.LastUpdatedTime)
            //         .ValueGeneratedOnAddOrUpdate();

            //     modelBuilder.Entity<User>()
            //         .Property(e => e.CreatedTime)
            //         .ValueGeneratedOnAdd();
            //     modelBuilder.Entity<User>()
            //         .Property(e => e.LastUpdatedTime)
            //         .ValueGeneratedOnAddOrUpdate();

            //     modelBuilder.Entity<TruongHoc>()
            //         .Property(e => e.CreatedTime)
            //         .ValueGeneratedOnAdd();
            //     // modelBuilder.Entity<TruongHoc>()
            //     //     .Property(e => e.LastUpdatedTime)
            //     //     .ValueGeneratedOnAddOrUpdate();

            //     modelBuilder.Entity<ThongBao>()
            //         .Property(e => e.CreatedTime)
            //         .ValueGeneratedOnAdd();
            //     modelBuilder.Entity<ThongBao>()
            //         .Property(e => e.LastUpdatedTime)
            //         .ValueGeneratedOnAddOrUpdate();

            //     modelBuilder.Entity<NhomZalo>()
            //         .Property(e => e.CreatedTime)
            //         .ValueGeneratedOnAdd();
            //     modelBuilder.Entity<NhomZalo>()
            //         .Property(e => e.LastUpdatedTime)
            //         .ValueGeneratedOnAddOrUpdate();

            modelBuilder.HasDefaultSchema("dbo");

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries<AbstractEntity>()
                .Where(e => e.State == EntityState.Added
                        || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Entity.CreatedTime = DateTime.Now;
                    entityEntry.Entity.LastUpdatedTime = DateTime.Now;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Entity.LastUpdatedTime = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken token = default)
        {
            var entries = ChangeTracker
                .Entries<AbstractEntity>()
                .Where(e => e.State == EntityState.Added
                        || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Entity.CreatedTime = DateTime.Now;
                    entityEntry.Entity.LastUpdatedTime = DateTime.Now;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Entity.LastUpdatedTime = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, token);
        }

    }
}
