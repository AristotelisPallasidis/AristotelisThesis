namespace AristotelisThesis.Domain.Models
{
    public class Account : DomainObject
    {
        public Student AccountHolder { get; set; }
    }
}
