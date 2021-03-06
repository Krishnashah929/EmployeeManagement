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

            builder.Entity<Speciality>().HasKey(table => new
            {
                table.DoctorId,
                table.SpecialityId
            });
             
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
        public virtual DbSet<Speciality> Specialities { get; set; }
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

        /// <summary>
        /// Get Forms
        /// </summary>
        public virtual DbSet<Forms> Forms { get; set; }

        /// <summary>
        /// Get FieldDetails
        /// </summary>
        public virtual DbSet<FieldDetails> FieldDetails { get; set; }

        /// <summary>
        /// Get FieldOptions
        /// </summary>
        public virtual DbSet<FieldOptions> FieldOptions { get; set; }

        /// <summary>
        /// Get CustomerForms
        /// </summary>
        public virtual DbSet<CustomerForms> CustomerForms { get; set; }

        /// <summary>
        /// Get FormFields
        /// </summary>
        public virtual DbSet<FormFields> FormFields { get; set; }

        /// <summary>
        /// Get CustomerFormData
        /// </summary>
        public virtual DbSet<CustomerFormData> CustomerFormData { get; set; }
    }
}