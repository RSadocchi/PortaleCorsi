using Microsoft.EntityFrameworkCore;
using PortaleCorsi.DbEntities;
using PortaleCorsi.Repositories;

namespace PortaleCorsi.Context
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        readonly IConfiguration? _configuration;
        public string? ConnectionString { get; set; }



        public AppDbContext() : base() { }
        public AppDbContext(IConfiguration configuration) : this()
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            ConnectionString = _configuration.GetConnectionString("app_connection_string");
        }
        public AppDbContext(DbContextOptions options, IConfiguration configuration) : base(options) 
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            ConnectionString = _configuration.GetConnectionString("app_connection_string");
        }


        public DbSet<CorsoMaster> CorsoMasters { get; set; }
        public DbSet<CorsoLezione> CorsoLezioni { get; set; }
        public DbSet<CorsoIscrizione> CorsoIscrizioni { get; set; }
        public DbSet<AnagraficaMaster> AnagraficaMasters { get; set; }
        public DbSet<AnagraficaIndirizzo> AnagraficaIndirizzi { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            bool sensitiveDataLoggingEnabled = System.Diagnostics.Debugger.IsAttached;
            bool detailedErrorsEnabled = System.Diagnostics.Debugger.IsAttached;

            optionsBuilder
                .EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: sensitiveDataLoggingEnabled)
                .EnableDetailedErrors(detailedErrorsEnabled: detailedErrorsEnabled);

            if (!optionsBuilder.IsConfigured)
                optionsBuilder
                    .UseSqlServer(connectionString: ConnectionString ?? throw new ArgumentNullException(nameof(ConnectionString), "Missing key field"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CorsoLezione>(e =>
            {
                e.HasOne(p => p.CorsoMaster)
                    .WithMany(p => p.Lezioni)
                    .HasForeignKey(p => p.CorsoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CorsoIscrizione>(e =>
            {
                e.HasOne(p => p.CorsoMaster)
                    .WithMany(p => p.Iscrizioni)
                    .HasForeignKey(p => p.CorsoId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(p => p.AnagraficaMaster)
                    .WithMany(p => p.Iscrizioni)
                    .HasForeignKey(p => p.AnagraficaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AnagraficaIndirizzo>(e =>
            {
                e.ToTable(nameof(AnagraficaIndirizzo));
                e.HasKey(p => p.Id);
                e.Property(p => p.Indirizzo).HasMaxLength(200);
                e.Property(p => p.Civico).HasMaxLength(10);
                e.Property(p => p.Citta).HasMaxLength(100);
                e.Property(p => p.Cap).HasMaxLength(5);
                e.Property(p => p.Prov).HasMaxLength(50);
                e.Property(p => p.Alias).HasMaxLength(50);

                e.HasOne(p => p.AnagraficaMaster)
                    .WithMany(p => p.Indirizzi)
                    .HasForeignKey(p => p.AnagraficaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AnagraficaMaster>(e =>
            {
                e.ToTable(nameof(AnagraficaMaster));
                e.HasKey(p => p.Id);
                e.Property(p => p.CodiceFiscale).HasMaxLength(16);
                e.Property(p => p.Nome).HasMaxLength(10);
                e.Property(p => p.Cognome).HasMaxLength(100);

                e.HasIndex(p => p.CodiceFiscale).IsUnique(true).IsClustered(false);
            });
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            bool hasChangesBeforeSave = ChangeTracker.HasChanges();
            int saveChangesCount = await base.SaveChangesAsync(cancellationToken: cancellationToken);
            return hasChangesBeforeSave == false || saveChangesCount > 0;
        }
    }
}
