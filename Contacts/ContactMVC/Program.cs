

using AutoMapper;
using Microsoft.EntityFrameworkCore;

var mapperConfiguration=new MapperConfiguration(config=>{
    config.CreateMap<PersonneDAO,PersonneModel>().ReverseMap();
     config.CreateMap<TelDAO,TelModel>().ReverseMap();
});

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMapper>(mapperConfiguration.CreateMapper());

// Add services to the container.
builder.Services.AddControllersWithViews();


SeedDataDelegate ProData=(modelBuilder)=>{
        // Seed => Données initiales
        var p1 = new PersonneDAO() { Nom = "Paul", Prenom = "Emploi" };
        var p2 = new PersonneDAO() { Nom = "Ringo", Prenom = "Star" };
        var p3 = new PersonneDAO() { Nom = "Lennon", Prenom = "John" };
        var p4 = new PersonneDAO() { Nom = "Lefou", Prenom = "Pierot" };
        modelBuilder.Entity<PersonneDAO>().HasData(p1, p2, p3,p4);
        var tels = new List<TelDAO>(){
            new TelDAO(){ Numero="01 67 87 67 56",IdPersonne=p1.Id},
            new TelDAO(){ Numero="01 62 87 67 56",IdPersonne=p1.Id},
            new TelDAO(){ Numero="01 61 87 67 56",IdPersonne=p4.Id},
            new TelDAO(){ Numero="01 61 87 67 56",IdPersonne=p2.Id}
        };
        modelBuilder.Entity<TelDAO>().HasData(tels);
};

SeedDataDelegate PersoData=(modelBuilder)=>{
        // Seed => Données initiales
        var p1 = new PersonneDAO() { Nom = "Paul", Prenom = "Emploi" };

        modelBuilder.Entity<PersonneDAO>().HasData(p1);
        var tels = new List<TelDAO>(){
            new TelDAO(){ Numero="01 67 87 67 56",IdPersonne=p1.Id},
            new TelDAO(){ Numero="01 62 87 67 56",IdPersonne=p1.Id}

        };
        modelBuilder.Entity<TelDAO>().HasData(tels);
};


// Je place dans l'injection de dépendance la fonction
// destinée à inserer les données initiales
builder.Services.AddSingleton<SeedDataDelegate>(s=>{
    // Fonction qui renvoit l'objet à ajouter en tant que singleton
    // s=> services
    var config=s.GetRequiredService<IConfiguration>();
    if(config.GetValue<string>("data")=="pro"){
        return ProData;
    }
    else
    {
        return PersoData;
    }
});
// Je place dans l'injection de dépendance la fonction
// destinée à spécifier les caractérisitiques de la BDD
builder.Services.AddSingleton<SetOptionalModelDelegate>((modelBuilder)=>{
        // modelBuilder.Entity<PersonneDAO>(entity =>
        // {
        //     entity.Property(c => c.Nom).HasMaxLength(50);
        //     entity.Property(c => c.Prenom).HasMaxLength(50);
        //     entity.ToTable("TBL_Personnes");
        //     entity.Property(c => c.Id).HasColumnName("PK_Personne");
        // });

        // modelBuilder.Entity<TelDAO>(entity =>
        // {
        //     entity.Property(c => c.Numero).HasMaxLength(50);
        //     entity.Property(c => c.Id).HasColumnName("PK_Personne");
        //     entity.Property(c => c.IdPersonne).HasColumnName("FK_Personne");
        //     entity.ToTable("TBL_Tels");
        // });
});

// Ajoute la capacité à l'injection de dépendance de fournir un objet de type ContactContext
builder.Services.AddDbContext<ContactContext>(options=>{
    options.UseInMemoryDatabase("mabase");
    //options.UseSqlServer("name=ContactsBDD");
    //options.UseOracle("name=ContactsBDD");
    //options.UseMySql("name=ContactsBDD");
});













var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Personne}/{action=Index}/{id?}");

app.Run();
