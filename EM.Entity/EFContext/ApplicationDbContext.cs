namespace EM.EFContext
{
    using EM.Entity;
    using Microsoft.EntityFrameworkCore;

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
    }
}