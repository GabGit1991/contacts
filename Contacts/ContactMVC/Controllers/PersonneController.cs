using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class PersonneController : Controller
{
    private readonly ContactContext db;
    private readonly IMapper mapper;

    public PersonneController(ContactContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public IActionResult Index(string searchText = "")
    {
        if (searchText == null)
        {
            searchText = "";
        }
        var personnesDAO = db.Personnes
        .Where(c => c.Nom.Contains(searchText) || c.Prenom.Contains(searchText))
        .OrderBy(c => c.Nom).ThenBy(c => c.Prenom);
        var personnes = mapper.Map<IEnumerable<PersonneModel>>(personnesDAO);
        return View(personnes);
    }

    public IActionResult Details(Guid id)
    {
        var personneDAO = db.Personnes.Include(c => c.NumeroDeTels).FirstOrDefault(c => c.Id == id);
        if (personneDAO == null)
        {
            return RedirectToAction("Index");
        }
        var personne = mapper.Map<PersonneModel>(personneDAO);
        return View(personne);
    }

[HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(PersonneModel model)
    {
        if(ModelState.IsValid){
            var p=new PersonneDAO();
            mapper.Map(model,p);
            db.Personnes.Add(p);
            db.SaveChanges();
            return RedirectToAction("Details",new {Id=p.Id});
        }
        return View(model);
    }

   
}