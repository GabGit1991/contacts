using System.ComponentModel.DataAnnotations;

public class PersonneModel : IValidatableObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MinLength(2)]
    [MaxLength(50)]
    [Display(Name = "Nom")]
    public string? Nom { get; set; }

    [MinLength(2)]
    [MaxLength(50)]
    [Display(Name = "Prénom")]
    public string? Prenom { get; set; }

    public virtual ICollection<TelModel>? NumeroDeTels { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (String.IsNullOrWhiteSpace(this.Nom) && String.IsNullOrWhiteSpace(this.Prenom))
        {
            yield return new ValidationResult("Le nom ou le prénom doivent être précise", new string[] { nameof(this.Nom), nameof(this.Prenom) });
        }
    }
}