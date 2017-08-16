using APICafeSuspendu;
using APICafeSuspendu.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        // Change the name of the table to be Users instead of AspNetUsers in order to find the Id
        //modelBuilder.Entity<IdentityUser>()
        //     .ToTable("Users").Property(p => p.Id).HasColumnName("UserID");
        //modelBuilder.Entity<ApplicationUser>()
        //    .ToTable("Users").Property(p => p.Id).HasColumnName("UserID");
        modelBuilder.Entity<Charity>()
          .HasRequired(c => c.ApplicationUserCoffee)
          .WithMany()
          .WillCascadeOnDelete(false);
        modelBuilder.Entity<Charity>()
            .HasRequired(c => c.ApplicationUserPerson)
            .WithMany()
            .WillCascadeOnDelete(false);

        //modelBuilder.Entity<ApplicationUser>()
        //    .HasMany(o => o.TimeTables)
        //    .WithOptional()
        //    .HasForeignKey(c => c.ApplicationUser);
        //.WithRequired(c => c.ApplicationUserPerson)
        //.WillCascadeOnDelete(false);
        base.OnModelCreating(modelBuilder);
    }
    


    public ApplicationDbContext()
        : base("DefaultConnection", throwIfV1Schema: false)
    {
        //LazyLoading -> récupérer un utilisateur, puis tirer par après sur la navigation property "Roles" n'exécute pas une nouvelle requête pour rapatrier
        // les rôles de l'utilisateur retourné. Voir modes de chargement d'entités liées (lazy loading vs eager loading) sur la wiki
        Configuration.ProxyCreationEnabled = true;
        Configuration.LazyLoadingEnabled = true;
        //Database.SetInitializer(new DbInitializer());
    }

    public static ApplicationDbContext Create()
    {
        return new ApplicationDbContext();
    }

    public System.Data.Entity.DbSet<APICafeSuspendu.Models.Charity> Charities { get; set; }

    public System.Data.Entity.DbSet<APICafeSuspendu.Models.TimeTable> TimeTables { get; set; }

    public System.Data.Entity.DbSet<APICafeSuspendu.Models.Booking> Bookings { get; set; }

    public System.Data.Entity.DbSet<APICafeSuspendu.Models.Terminal> Terminals { get; set; }
}