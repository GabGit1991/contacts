public class PersonneDAO{
    public Guid Id { get; set; }=Guid.NewGuid();
    public string? Nom { get; set; }

    public string? Prenom { get; set; }

    public virtual ICollection<TelDAO>? NumeroDeTels { get; set; }
}