using AIC.Portal.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIC.Portal.DbContexts
{
	public class ContactFormContext : DbContext
	{
		public ContactFormContext(DbContextOptions<ContactFormContext> options)
			: base(options)
		{
		}

		public DbSet<ContactForm> ContactForms { get; set; }
		public DbSet<governorates> Governorates { get; set; }
		public DbSet<Countries> Countries { get; set; }
		public DbSet<type> Types { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ContactForm>(entity =>
			{
				entity.ToTable("ContactForm");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id).HasColumnName("id");
				entity.Property(e => e.FullName).HasColumnName("FullName");
				entity.Property(e => e.Message).HasColumnName("message");
				entity.Property(e => e.EmailAddress).HasColumnName("EmailAddress");
				entity.Property(e => e.PhoneNumber).HasColumnName("PhoneNumber");
				//entity.HasOne(e => e.Country).WithMany(c => c.ContactForms);
				entity.HasOne(e => e.city).WithMany(c => c.ContactForms).OnDelete(DeleteBehavior.NoAction);
				entity.HasOne(e => e.Type).WithMany(c => c.ContactForms);
				entity.Property(e => e.work).HasColumnName("work");
			});

			modelBuilder.Entity<governorates>(entity =>
			{
				entity.ToTable("Governorates");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.province_id).HasColumnName("province_id");
				entity.Property(e => e.Id).HasColumnName("Id");
				entity.Property(e => e.GovernorateNameAr).HasColumnName("GovernorateNameAr");
				entity.Property(e => e.GovernorateNameEn).HasColumnName("GovernorateNameEn");
				entity.HasOne(d => d.country)
					.WithMany(p => p.Governorates);
                    

            });

			modelBuilder.Entity<Countries>(entity =>
			{
				entity.ToTable("Countries");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id).HasColumnName("Id");
				entity.Property(e => e.CountryName).HasColumnName("CountryName");
                entity.HasMany(c => c.Governorates)
				.WithOne(g => g.country)
				;
            });

			modelBuilder.Entity<type>(entity =>
			{
				entity.ToTable("Types");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id).HasColumnName("Id");
				entity.Property(e => e.Type).HasColumnName("Type");
			});
		}
	}
}
