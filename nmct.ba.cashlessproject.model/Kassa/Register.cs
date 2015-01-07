using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model.Kassa
{
    public class Register
    {
        #region props
        [Required]
        public int ID { get; set; }
        [DisplayName("Kassanaam")]
        [Required(ErrorMessage = "De kassanaam is verplicht")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "De kassanaam moet tussen de 3 en 80 karakters bevatten")]
        public string Name { get; set; }
       
        [DisplayName("Toestel")]
        [Required(ErrorMessage = "Het toestel is verplicht")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Het toestel moet tussen de 3 en 80 karakters bevatten")]
        public string Device { get; set; }
        public bool Hidden { get; set; }
                
        
       
        #endregion
    }
}
