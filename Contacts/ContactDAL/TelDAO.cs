public class TelDAO
{
       public Guid Id { get; set; }=Guid.NewGuid();
       public string? Numero { get; set; }
       public Guid IdPersonne { get; set; }
       public virtual PersonneDAO? Personne { get; set; }
}