using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Created using Data first approach
// Scaffold-DbContext "Filename=Chinook_Sqlite.sqlite" Microsoft.EntityFrameworkCore.Sqlite 

namespace Chinook.Data
{
    public partial class Chinook_SqliteContext : DbContext
    {
        public Chinook_SqliteContext()
        {
        }

        public Chinook_SqliteContext(DbContextOptions<Chinook_SqliteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<Artist> Artist { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceLine> InvoiceLine { get; set; }
        public virtual DbSet<MediaType> MediaType { get; set; }
        public virtual DbSet<Playlist> Playlist { get; set; }
        public virtual DbSet<PlaylistTrack> PlaylistTrack { get; set; }
        public virtual DbSet<Track> Track { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=Chinook_Sqlite.sqlite");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>(entity =>
            {
                entity.HasIndex(e => e.AlbumId)
                    .HasName("IPK_Album")
                    .IsUnique();

                entity.HasIndex(e => e.ArtistId)
                    .HasName("IFK_AlbumArtistId");

                entity.Property(e => e.AlbumId).ValueGeneratedNever();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(160)");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Album)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.HasIndex(e => e.ArtistId)
                    .HasName("IPK_Artist")
                    .IsUnique();

                entity.Property(e => e.ArtistId).ValueGeneratedNever();

                entity.Property(e => e.Name).HasColumnType("NVARCHAR(120)");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.CustomerId)
                    .HasName("IPK_Customer")
                    .IsUnique();

                entity.HasIndex(e => e.SupportRepId)
                    .HasName("IFK_CustomerSupportRepId");

                entity.Property(e => e.CustomerId).ValueGeneratedNever();

                entity.Property(e => e.Address).HasColumnType("NVARCHAR(70)");

                entity.Property(e => e.City).HasColumnType("NVARCHAR(40)");

                entity.Property(e => e.Company).HasColumnType("NVARCHAR(80)");

                entity.Property(e => e.Country).HasColumnType("NVARCHAR(40)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(60)");

                entity.Property(e => e.Fax).HasColumnType("NVARCHAR(24)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(40)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(20)");

                entity.Property(e => e.Phone).HasColumnType("NVARCHAR(24)");

                entity.Property(e => e.PostalCode).HasColumnType("NVARCHAR(10)");

                entity.Property(e => e.State).HasColumnType("NVARCHAR(40)");

                entity.HasOne(d => d.SupportRep)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.SupportRepId);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.EmployeeId)
                    .HasName("IPK_Employee")
                    .IsUnique();

                entity.HasIndex(e => e.ReportsTo)
                    .HasName("IFK_EmployeeReportsTo");

                entity.Property(e => e.EmployeeId).ValueGeneratedNever();

                entity.Property(e => e.Address).HasColumnType("NVARCHAR(70)");

                entity.Property(e => e.BirthDate).HasColumnType("DATETIME");

                entity.Property(e => e.City).HasColumnType("NVARCHAR(40)");

                entity.Property(e => e.Country).HasColumnType("NVARCHAR(40)");

                entity.Property(e => e.Email).HasColumnType("NVARCHAR(60)");

                entity.Property(e => e.Fax).HasColumnType("NVARCHAR(24)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(20)");

                entity.Property(e => e.HireDate).HasColumnType("DATETIME");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(20)");

                entity.Property(e => e.Phone).HasColumnType("NVARCHAR(24)");

                entity.Property(e => e.PostalCode).HasColumnType("NVARCHAR(10)");

                entity.Property(e => e.State).HasColumnType("NVARCHAR(40)");

                entity.Property(e => e.Title).HasColumnType("NVARCHAR(30)");

                entity.HasOne(d => d.ReportsToNavigation)
                    .WithMany(p => p.InverseReportsToNavigation)
                    .HasForeignKey(d => d.ReportsTo);
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasIndex(e => e.GenreId)
                    .HasName("IPK_Genre")
                    .IsUnique();

                entity.Property(e => e.GenreId).ValueGeneratedNever();

                entity.Property(e => e.Name).HasColumnType("NVARCHAR(120)");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasIndex(e => e.CustomerId)
                    .HasName("IFK_InvoiceCustomerId");

                entity.HasIndex(e => e.InvoiceId)
                    .HasName("IPK_Invoice")
                    .IsUnique();

                entity.Property(e => e.InvoiceId).ValueGeneratedNever();

                entity.Property(e => e.BillingAddress).HasColumnType("NVARCHAR(70)");

                entity.Property(e => e.BillingCity).HasColumnType("NVARCHAR(40)");

                entity.Property(e => e.BillingCountry).HasColumnType("NVARCHAR(40)");

                entity.Property(e => e.BillingPostalCode).HasColumnType("NVARCHAR(10)");

                entity.Property(e => e.BillingState).HasColumnType("NVARCHAR(40)");

                entity.Property(e => e.InvoiceDate)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(e => e.Total)
                    .IsRequired()
                    .HasColumnType("NUMERIC(10,2)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<InvoiceLine>(entity =>
            {
                entity.HasIndex(e => e.InvoiceId)
                    .HasName("IFK_InvoiceLineInvoiceId");

                entity.HasIndex(e => e.InvoiceLineId)
                    .HasName("IPK_InvoiceLine")
                    .IsUnique();

                entity.HasIndex(e => e.TrackId)
                    .HasName("IFK_InvoiceLineTrackId");

                entity.Property(e => e.InvoiceLineId).ValueGeneratedNever();

                entity.Property(e => e.UnitPrice)
                    .IsRequired()
                    .HasColumnType("NUMERIC(10,2)");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceLine)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.InvoiceLine)
                    .HasForeignKey(d => d.TrackId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<MediaType>(entity =>
            {
                entity.HasIndex(e => e.MediaTypeId)
                    .HasName("IPK_MediaType")
                    .IsUnique();

                entity.Property(e => e.MediaTypeId).ValueGeneratedNever();

                entity.Property(e => e.Name).HasColumnType("NVARCHAR(120)");
            });

            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.HasIndex(e => e.PlaylistId)
                    .HasName("IPK_Playlist")
                    .IsUnique();

                entity.Property(e => e.PlaylistId).ValueGeneratedNever();

                entity.Property(e => e.Name).HasColumnType("NVARCHAR(120)");
            });

            modelBuilder.Entity<PlaylistTrack>(entity =>
            {
                entity.HasKey(e => new { e.PlaylistId, e.TrackId });

                entity.HasIndex(e => e.TrackId)
                    .HasName("IFK_PlaylistTrackTrackId");

                entity.HasIndex(e => new { e.PlaylistId, e.TrackId })
                    .HasName("IPK_PlaylistTrack")
                    .IsUnique();

                entity.HasOne(d => d.Playlist)
                    .WithMany(p => p.PlaylistTrack)
                    .HasForeignKey(d => d.PlaylistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.PlaylistTrack)
                    .HasForeignKey(d => d.TrackId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Track>(entity =>
            {
                entity.HasIndex(e => e.AlbumId)
                    .HasName("IFK_TrackAlbumId");

                entity.HasIndex(e => e.GenreId)
                    .HasName("IFK_TrackGenreId");

                entity.HasIndex(e => e.MediaTypeId)
                    .HasName("IFK_TrackMediaTypeId");

                entity.HasIndex(e => e.TrackId)
                    .HasName("IPK_Track")
                    .IsUnique();

                entity.Property(e => e.TrackId).ValueGeneratedNever();

                entity.Property(e => e.Composer).HasColumnType("NVARCHAR(220)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(200)");

                entity.Property(e => e.UnitPrice)
                    .IsRequired()
                    .HasColumnType("NUMERIC(10,2)");

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Track)
                    .HasForeignKey(d => d.AlbumId);

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Track)
                    .HasForeignKey(d => d.GenreId);

                entity.HasOne(d => d.MediaType)
                    .WithMany(p => p.Track)
                    .HasForeignKey(d => d.MediaTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
