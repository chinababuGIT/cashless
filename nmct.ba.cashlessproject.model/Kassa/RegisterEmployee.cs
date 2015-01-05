using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model.Kassa
{
    public class RegisterEmployee
    { 
        #region props

        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public int RegisterID { get; set; }
        [DisplayName("Kassanaam")]
        [Required(ErrorMessage = "De kassanaam is verplicht")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "De kassanaam moet tussen de 3 en 80 karakters bevatten")]
        public string RegisterName { get; set; }
      
        [DisplayName("Toestel")]
        [Required(ErrorMessage = "Het toestel is verplicht")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Het toestel moet tussen de 3 en 80 karakters bevatten")]
        public string RegisterDevice { get; set; }

       
        public bool RegisterHidden { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Aanmaakdatum")]
        public long From { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Verwijderdatum")]
        public long Until { get; set; }

        [DisplayName("Medewerkersnaam")]
        [Required(ErrorMessage = "De medewerkersnaam is verplicht")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "De medewerkersnaam moet tussen de 3 en 50 karakters bevatten")]
        public string EmployeeName { get; set; }
        [DisplayName("Adres")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Het adres moet tussen de 3 en 50 karakters bevatten")]
        public string EmployeeAddress { get; set; }
        [DisplayName("E-mail")]
        [Required(ErrorMessage = "Het e-mailadres is verplicht")]
        [EmailAddress(ErrorMessage = "Het is geen gelig e-mailadres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Geen geldig email-adres")]
        public string EmployeeEmail { get; set; }
        [DisplayName("Telefoonnummer")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Het adres moet tussen de 3 en 50 karakters bevatten")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Geen geldig telefoonnummer")]
        public string EmployeePhone { get; set; }
        [DisplayName("Registernummer")]
        [StringLength(11, MinimumLength = 0, ErrorMessage = "Het registernummer bevat 11 karakters")]
        public string EmployeeRegisterNumber { get; set; }
        
        public byte[] EmployeePicture { get; set; }
        



        #endregion

    }
}
