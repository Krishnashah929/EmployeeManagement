#region using
using EM.Common;
using EM.Entity;
using Microsoft.EntityFrameworkCore;
using System;
#endregion

namespace EM.EFContext
{

    /// <summary>
    /// Defines the <see cref="ApplicationDbContext" />.
    /// </summary>
    public partial class ApplicationDbContext : DbContext, IDatabaseContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options<see cref="DbContextOptions{ApplicationDbContext}"/>.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// The OnModelCreating.
        /// </summary>
        /// <param name="builder">The builder<see cref="ModelBuilder"/>.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
               .HasData(
               new User
               {
                   UserId = 1,
                   FirstName = "Admin",
                   Lastname = "Admin",
                   EmailAddress = "Admin@gmail.com",
                   Password = EncryptionDecryption.Encrypt("Admin@123").ToString(),
                   Role = "1",
                   IsActive = true
               }
               );
        }

        /// <summary>
        /// Gets or sets the Setting.
        /// </summary>
        public virtual DbSet<Setting> Setting { get; set; }

        /// <summary>
        /// Get or set the user.
        /// </summary>
        public virtual DbSet<User> User { get; set; }
        /// <summary>
        /// Get or set the user role.
        /// </summary>
        public virtual DbSet<UserRole> UserRole { get; set; }
        /// <summary>
        /// Get all doctors details.
        /// </summary>
        public virtual DbSet<Doctor> Doctor { get; set; }
        /// <summary>
        /// Get doctor's speciality
        /// </summary>
        public virtual DbSet<Speciality> Specialitie { get; set; }
        /// <summary>
        /// Do Appointment
        /// </summary>
        public virtual DbSet<Appointment> Appointment { get; set; }
        /// <summary>
        /// Get City 
        /// </summary>
        public virtual DbSet<City> Cities { get; set; }

        /// <summary>
        /// Get State
        /// </summary>
        public virtual DbSet<State> States { get; set; }

        /// <summary>
        /// Get Country
        /// </summary>
        public virtual DbSet<Country> Country { get; set; }

        /// <summary>
        /// Get Email 
        /// </summary>
        public virtual DbSet<Email> Email { get; set; }

    }
}