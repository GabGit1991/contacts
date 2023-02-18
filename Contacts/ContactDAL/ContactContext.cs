using Microsoft.EntityFrameworkCore;

public delegate void SetOptionalModelDelegate(ModelBuilder builder);
public delegate void SeedDataDelegate(ModelBuilder builder);


public class ContactContext : DbContext
{
    private readonly SeedDataDelegate SeedData;
    private readonly SetOptionalModelDelegate SetOptionalModel;


    // ContactContext
    // Est-il créé par var ContactContext=new ContactContext();
    // var c=new PersonneController();
    // var b=new Brouette();
    // => 
    public ContactContext(DbContextOptions options,
               // Je récois dans le constructeur la fonction qui permet d'ajouter les doonées initiales
        SetOptionalModelDelegate setOptionalModel,

        SeedDataDelegate seedData

    ) : base(options)
    {

        this.SeedData = seedData;
        this.SetOptionalModel = setOptionalModel;
                this.Database.EnsureCreated();
    }
    public DbSet<PersonneDAO> Personnes { get; set; }
    public DbSet<TelDAO> NumerosDeTels { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PersonneDAO>(entity =>
        {
            // Essentielles
            entity.HasKey(c => c.Id);
            entity.HasMany(c => c.NumeroDeTels).WithOne(c => c.Personne).HasForeignKey(c => c.IdPersonne);
        });

        modelBuilder.Entity<TelDAO>(entity =>
        {
            entity.HasKey(c => c.Id);

            // Je ne peux enregistrer un numéro qu'une fois par personne
            entity.HasIndex(c => new { c.IdPersonne, c.Numero }).IsUnique(true);
        });

        SetOptionalModel(modelBuilder);

        SeedData(modelBuilder);

    }


    // Ce délégué correspond à une méthode qui permet d'ajouter les données initiales

    // void SeedData(ModelBuilder modelBuilder)
    // {
    //     // Seed => Données initiales
    //     var p1 = new PersonneDAO() { Nom = "Paul", Prenom = "Emploi" };
    //     var p2 = new PersonneDAO() { Nom = "Ringo", Prenom = "Star" };
    //     var p3 = new PersonneDAO() { Nom = "Lennon", Prenom = "John" };
    //     modelBuilder.Entity<PersonneDAO>().HasData(p1, p2, p3);
    //     var tels = new List<TelDAO>(){
    //         new TelDAO(){ Numero="01 67 87 67 56",IdPersonne=p1.Id},
    //         new TelDAO(){ Numero="01 62 87 67 56",IdPersonne=p1.Id},
    //         new TelDAO(){ Numero="01 61 87 67 56",IdPersonne=p1.Id},
    //         new TelDAO(){ Numero="01 61 87 67 56",IdPersonne=p2.Id}
    //     };
    //     modelBuilder.Entity<TelDAO>().HasData(tels);
    // }


    // // Définitions optionnelles de la BDD
    // void SetOptionalModel(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<PersonneDAO>(entity =>
    //     {
    //         entity.Property(c => c.Nom).HasMaxLength(50);
    //         entity.Property(c => c.Prenom).HasMaxLength(50);
    //         entity.ToTable("TBL_Personnes");
    //         entity.Property(c => c.Id).HasColumnName("PK_Personne");
    //     });

    //     modelBuilder.Entity<TelDAO>(entity =>
    //     {
    //         entity.Property(c => c.Numero).HasMaxLength(50);
    //         entity.Property(c => c.Id).HasColumnName("PK_Personne");
    //         entity.Property(c => c.IdPersonne).HasColumnName("FK_Personne");
    //         entity.ToTable("TBL_Tels");
    //     });
    // }
}