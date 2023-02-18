using System.ComponentModel.DataAnnotations;
public class TelModel
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Display(Name = "Numéro de téléphone")]
    [Required(ErrorMessage = "{0} est requis")]
    [RegularExpression(@"^\d{2} \d{2} \d{2} \d{2} \d{2}$", ErrorMessage = "Entrez un numéro correct : xx xx xx xx xx")]
    public string? Numero { get; set; }
    public Guid IdPersonne { get; set; }
    public virtual PersonneModel? Personne { get; set; }
}