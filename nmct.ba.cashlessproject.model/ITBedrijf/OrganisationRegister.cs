using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model.ITBedrijf
{
    public class OrganisationRegister
    {
        [Required]
        [DisplayName("Organisatie")]
        public int OrganisationID { get; set; }

        [DisplayName("Bedrijf")]
        //[Required(ErrorMessage = "De naam is verplicht")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,50}$", ErrorMessage = "Er zijn geen speciale tekens toegelaten")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "De naam moet tussen de 3 en 50 karakters bevatten")]
        public string OrganisationName { get; set; }

        [DisplayName("Adres")]
        //[Required(ErrorMessage = "Het adres is verplicht")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Het adres moet tussen de 3 en 80 karakters bevatten")]
        public string OrganisationAddress { get; set; }
        [DisplayName("Email")]
        //[Required(ErrorMessage = "Het e-mailadres is verplicht")]
        [EmailAddress(ErrorMessage = "Het is geen gelig e-mailadres")]
        public string OrganisationEmail { get; set; }
        [DisplayName("Telefoonnummer")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Het adres moet tussen de 3 en 50 karakters bevatten")]
        public string OrganisationPhone { get; set; }
        public byte[] OrganisationPicture { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Aanmaakdatum")]
        public long FromDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Verwijderdatum")]
        public long UntilDate { get; set; }
        [Required]
        [DisplayName("ID")]
        public int RegisterID { get; set; }
        [DisplayName("Kassa")]
        [Required(ErrorMessage = "De kassanaam is verplicht")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "De kassanaam moet tussen de 3 en 80 karakters bevatten")]
        public string RegisterName { get; set; }
        [Required(ErrorMessage = "Het toestel is verplicht")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Het toestel moet tussen de 3 en 80 karakters bevatten")]
        [DisplayName("Toestel")]
        public string RegisterDevice { get; set; }
        [DisplayName("Aankoopdatum")]
        [DataType(DataType.Date)]
        public long RegisterPurchaseDate { get; set; }
        [DisplayName("Geldigheidsdatum")]
        [DataType(DataType.Date)]
        public long RegisterExpiresDate { get; set; }


    }
}
