using System.ComponentModel.DataAnnotations;

namespace AristotelisThesis.Domain.Models
{
    public class Student : DomainObject
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(20, ErrorMessage = "Name cannot be longer than 20 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        [StringLength(20, ErrorMessage = "Surname cannot be longer than 20 characters.")]
        public string Surname { get; set; }

        // Vestigial: authentication is biometric-only. Registration sets Username to the academic
        // email and leaves PasswordHash empty; nothing ever reads either as a credential.
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Sex is required.")]
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Sex must be either 'Male' or 'Female'.")]
        public string Sex { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Contact Phone is required.")]
        [RegularExpression("^69[0-9]{8}$", ErrorMessage = "Contact Phone must be 10 digits and start with '69'.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Home Address is required.")]
        [StringLength(200, ErrorMessage = "Home Address cannot be longer than 200 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Academic Email is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.gr$", ErrorMessage = "Academic Email must contain '@' and end with '.gr'.")]
        public string AcademicEmail { get; set; }

        // The university's student number: unique by definition, enforced by a unique index.
        [Required(ErrorMessage = "AEM is required.")]
        [Range(1000, 99999, ErrorMessage = "AEM must be between 4 and 5 digits.")]
        public int AEM { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Semester is required.")]
        [Range(1, 12, ErrorMessage = "Semester must be between 1 and 12.")]
        public int Semester { get; set; }

        [Required(ErrorMessage = "Year of Entry is required.")]
        [Range(1900, 2100, ErrorMessage = "Year of Entry must be a valid year.")]
        public int YearOfEntry { get; set; }

        [Required(ErrorMessage = "Study level is required.")]
        public bool IsPostgraduate { get; set; }

    }
}