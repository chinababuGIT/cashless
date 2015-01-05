using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model.ITBedrijf
{
    public class Log
    {
        [Required]
        [DisplayName("ID")]
        public int RegisterID { get; set; }
        [Required]
        [DisplayName("Tijdstip")]
        public Single Timestamp { get; set; }
        [DisplayName("Bericht")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Het bericht moet tussen de 3 en 80 karakters bevatten")]
        public string Message { get; set; }
        [DisplayName("Stacktrace")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "De stacktrace tussen de 3 en 100 karakters bevatten")]
        public string StackTrace { get; set; }


    }
}
