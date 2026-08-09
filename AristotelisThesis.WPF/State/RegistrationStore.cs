using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AristotelisThesis.WPF.State
{
    /// <summary>
    /// Carries the data collected across the multi-step registration wizard (personal info
    /// in step 2, captured face images in step 6) until the account is created at the end.
    /// Registered as a singleton so the steps, which are separate view models, share it.
    /// </summary>
    public class RegistrationStore
    {
        // --- Personal info (Register02) ---
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public string DateOfBirth { get; set; } = "";   // free text, dd/MM/yyyy
        public string AcademicEmail { get; set; } = "";
        public string Department { get; set; } = "";     // chosen from a dropdown (ComboBox)
        public string Aem { get; set; } = "";           // numeric text
        public string Semester { get; set; } = "";      // numeric text
        public string YearOfEntry { get; set; } = "";   // chosen from a dropdown (ComboBox)

        public bool IsMale { get; set; } = true;
        public bool IsFemale { get; set; }
        public bool IsPostgraduate { get; set; }
        public bool IsUndergraduate { get; set; } = true;

        // --- Captured biometrics ---
        // Each entry is the JPEG image plus its feature embedding.
        public List<(byte[] Jpeg, float[] Embedding)> CapturedFaces { get; } = new();   // Register06
        public List<(byte[] Jpeg, float[] Embedding)> CapturedPalms { get; } = new();   // Register04

        public string Sex => IsFemale ? "Female" : "Male";

        /// <summary>Accepted date-of-birth input formats.</summary>
        public static readonly string[] DateFormats = { "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy" };

        /// <summary>
        /// True only when every personal-info field is present and valid per the Student model's
        /// rules. Drives whether the registration wizard may advance past the info step.
        /// </summary>
        public bool IsPersonalInfoValid()
        {
            if (string.IsNullOrWhiteSpace(Name) || Name.Length > 20) return false;
            if (string.IsNullOrWhiteSpace(Surname) || Surname.Length > 20) return false;
            if (string.IsNullOrWhiteSpace(Address) || Address.Length > 200) return false;
            if (!Regex.IsMatch(Phone ?? "", "^69[0-9]{8}$")) return false;          // 10 digits, starts 69
            if (!Regex.IsMatch(AcademicEmail ?? "", @"^[^@\s]+@[^@\s]+\.gr$")) return false;
            if (string.IsNullOrWhiteSpace(Department)) return false;
            if (!DateTime.TryParseExact(DateOfBirth, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _)) return false;
            if (!int.TryParse(Aem, out int aem) || aem < 1000 || aem > 99999) return false;
            if (!int.TryParse(Semester, out int semester) || semester < 1 || semester > 12) return false;
            if (!int.TryParse(YearOfEntry, out int year) || year < 1900 || year > 2100) return false;
            return true;
        }

        public void Reset()
        {
            Name = Surname = Address = Phone = DateOfBirth = "";
            AcademicEmail = Department = Aem = Semester = YearOfEntry = "";
            IsMale = true;
            IsFemale = false;
            IsPostgraduate = false;
            IsUndergraduate = true;
            CapturedFaces.Clear();
            CapturedPalms.Clear();
        }
    }
}
