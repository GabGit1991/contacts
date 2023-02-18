using AutoMapper;
using Microsoft.AspNetCore.Mvc;

public class TelController : Controller
{
    private readonly ContactContext db;
    private readonly IMapper mapper;

    public TelController(ContactContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

[HttpPost]
  public IActionResult Create(TelModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var dao = new TelDAO();
                mapper.Map(model, dao);
   
                db.NumerosDeTels.Add(dao);
                db.SaveChanges();
            }
            catch (System.Exception)
            {


            }
        }

        return RedirectToAction("Details", "Personne", new { Id = model.IdPersonne });
    }
   

    public IActionResult Delete(Guid id)
    {
        var dao = db.NumerosDeTels.Find(id);
        if (dao == null)
        {
            return RedirectToAction("Index", "Personne");
        }
        var idPersonne=dao.IdPersonne;
        db.NumerosDeTels.Remove(dao);
        db.SaveChanges();
        return RedirectToAction("Details", "Personne", new {id=idPersonne});
    }
}
