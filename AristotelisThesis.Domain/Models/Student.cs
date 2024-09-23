using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AristotelisThesis.Domain.Models
{
    public class Student
    {
        // Unique student ID (could be generated automatically or manually assigned)
        [Required(ErrorMessage = "Student ID is required.")]
        public int Id { get; set; }

        // Student's first name
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(20, ErrorMessage = "Name cannot be longer than 20 characters.")]
        public string Name { get; set; }

        // Student's last name (surname)
        [Required(ErrorMessage = "Surname is required.")]
        [StringLength(20, ErrorMessage = "Surname cannot be longer than 20 characters.")]
        public string Surname { get; set; }

        // Gender (must be male or female)
        [Required(ErrorMessage = "Sex is required.")]
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Sex must be either 'Male' or 'Female'.")]
        public string Sex { get; set; }

        // Student's date of birth
        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }

        // Contact phone number (must be 10 digits and start with "69")
        [Required(ErrorMessage = "Contact Phone is required.")]
        [RegularExpression("^69[0-9]{8}$", ErrorMessage = "Contact Phone must be 10 digits and start with '69'.")]
        public string ContactPhone { get; set; }

        // Home address
        [Required(ErrorMessage = "Home Address is required.")]
        [StringLength(200, ErrorMessage = "Home Address cannot be longer than 200 characters.")]
        public string HomeAddress { get; set; }

        // Academic email (must contain @ and .gr)
        [Required(ErrorMessage = "Academic Email is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.gr$", ErrorMessage = "Academic Email must contain '@' and end with '.gr'.")]
        public string AcademicEmail { get; set; }

        // AEM (unique student number, must be between 4-5 digits)
        [Required(ErrorMessage = "AEM is required.")]
        [Range(1000, 99999, ErrorMessage = "AEM must be between 4 and 5 digits.")]
        public int AEM { get; set; }

        // Department (e.g., Computer Science, Engineering)
        [Required(ErrorMessage = "Department is required.")]
        public string Department { get; set; }

        // Current semester of the student
        [Required(ErrorMessage = "Semester is required.")]
        [Range(1, 8, ErrorMessage = "Semester must be between 1 and 8.")]
        public int Semester { get; set; }

        // Year the student entered the institution
        [Required(ErrorMessage = "Year of Entry is required.")]
        [Range(1900, 2100, ErrorMessage = "Year of Entry must be a valid year.")]
        public int YearOfEntry { get; set; }

        // Indicates if the student is undergraduate or postgraduate
        [Required(ErrorMessage = "Study level is required.")]
        public bool IsPostgraduate { get; set; }
    }
}