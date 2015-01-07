using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model.ITBedrijf
{
    public class Organisation : IDataErrorInfo
    {

        #region props
        [Required]
        public int ID { get; set; }

        [DisplayName("Gebruikersnaam")]
        [Required(ErrorMessage = "De gebruikersnaam is verplicht")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,50}$", ErrorMessage = "Er zijn geen speciale tekens toegelaten")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "De gebruikersnaam moet tussen de 3 en 30 karakters bevatten")]
        public string Username { get; set; }

        [DisplayName("Wachtwoord")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Het wachtwoord moet minstens 3 karakters bevatten")]
        public string Password { get; set; }

        [DisplayName("Databasenaam")]
        [Required(ErrorMessage = "De databasenaam is verplicht")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,50}$", ErrorMessage = "Er zijn geen speciale tekens toegelaten")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "De databasenaam moet tussen de 3 en 50 karakters bevatten")]
        public string DbName { get; set; }

        [DisplayName("Databasegebruikersnaam")]
        [Required(ErrorMessage = "De gebruikersnaam is verplicht")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,50}$", ErrorMessage = "Er zijn geen speciale tekens toegelaten")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "De gebruikersnaam moet tussen de 3 en 100 karakters bevatten")]
        public string DbUsername { get; set; }

        [DisplayName("Databasewachtwoord")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Het wachtwoord moet minstens 3 karakters bevatten")]
        public string DbPassword { get; set; }


        [DisplayName("Bedrijf")]
        [Required(ErrorMessage = "De naam is verplicht")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,50}$", ErrorMessage = "Er zijn geen speciale tekens toegelaten")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "De naam moet tussen de 3 en 50 karakters bevatten")]
        public string Name { get; set; }

        [DisplayName("Adres")]
        [Required(ErrorMessage = "Het adres is verplicht")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Het adres moet tussen de 3 en 80 karakters bevatten")]
        public string Address { get; set; }

        [DisplayName("E-mail")]
        [Required(ErrorMessage = "Het e-mailadres is verplicht")]
        [EmailAddress(ErrorMessage="Het is geen gelig e-mailadres")]
        public string Email { get; set; }

        [DisplayName("Telefoonnummer")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Geen geldig telefoonnummer")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Het adres moet tussen de 3 en 50 karakters bevatten")]
        public string Phone { get; set; }

        public byte[] Logo { get; set; }

        //herhaal wachtwoord, niet in db
        [DisplayName("Herhaal wachtwoord")]
        [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen!")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Het wachtwoord moet minstens 3 karakters bevatten")]
        public string RepeatPassword { get; set; }

        //lijst registers
        public List<Register> Registers { get; set; }
        #endregion

        #region xaml validate
        public string Error
        {
            get { return "De gegevens kloppen niet"; }
        }
        public string this[string columnName]
        {
            get
            {
                if (columnName == "RepeatPassword")
                {
                    if (this.RepeatPassword==null)
                    {
                        return "Geen wachtwoord ingevuld";
                    }
                    if (this.Password != this.RepeatPassword)
                    {
                        return "De wachtwoorden zijn niet hetzelfde";
                    }
                    
                }
               

                try
                {
                    object value = this.GetType().GetProperty(columnName).GetValue(this);
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null)
                    {
                        MemberName = columnName
                    });
                }
                catch (ValidationException ex)
                {

                    return ex.Message;
                }

                return "";
            }
        }
        public bool IsValid()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this, null, null), null, true);
            
        }

        #endregion



    }


}
