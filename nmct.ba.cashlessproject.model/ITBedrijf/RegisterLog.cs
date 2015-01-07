using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model.ITBedrijf
{
    public class RegisterLog
    {
        [Required]
        [DisplayName("Tijdstip")]
        public long LogTimestamp { get; set; }
        [DisplayName("Bericht")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Het bericht moet tussen de 3 en 80 karakters bevatten")]
        public string LogMessage { get; set; }
        [DisplayName("Stacktrace")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "De stacktrace moet tussen de 3 en 100 karakters bevatten")]
        public string LogStackTrace { get; set; }


        [Required]
        [DisplayName("ID")]
        public int RegisterID { get; set; }
        [DisplayName("Kassanaam")]
        [Required(ErrorMessage = "De kassanaam is verplicht")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "De kassanaam moet tussen de 3 en 80 karakters bevatten")]
        public string RegisterName { get; set; }
       
        [DisplayName("Toestel")]
        [Required(ErrorMessage = "Het toestel is verplicht")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Het toestel moet tussen de 3 en 50 karakters bevatten")]
        public string RegisterDevice { get; set; }
        [DisplayName("Aankoopdatum")]
        [DataType(DataType.Date)]
        public long RegisterPurchaseDate { get; set; }
        [DisplayName("Verwijderdatum")]
        [DataType(DataType.Date)]
        public long RegisterExpiresDate { get; set; }
    }
}
