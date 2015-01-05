using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model.Kassa
{
    public class Customer : IDataErrorInfo
    {
        [Required]
        public int ID { get; set; }
        [DisplayName("Naam")]
        [Required(ErrorMessage = "De naam is verplicht")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "De naam moet tussen de 3 en 50 karakters bevatten")]
        public string Name { get; set; }
        [DisplayName("Familienaam")]
        [Required(ErrorMessage = "De familienaam is verplicht")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "De familienaam moet tussen de 3 en 50 karakters bevatten")]
        public string SurName { get; set; }
        [DisplayName("Adres")]   
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Het adres moet tussen de 3 en 50 karakters bevatten")]
        public string Address { get; set; }
        [Required]
        [DisplayName("Balans")]
        [Range(0, double.MaxValue, ErrorMessage = "De prijs is geen getal")]
        public double Balance { get; set; }
        [DisplayName("Registernummer")]
        [StringLength(11, MinimumLength = 0, ErrorMessage = "Het registernummer bevat 11 karakters")]

        public string RegisterNumber { get; set; }
        [DisplayName("Afbeelding")]
        public byte[] Picture { get; set; }
        public bool Hidden { get; set; }


        #region xaml validate
        public string Error
        {
            get { return "De gegevens kloppen niet"; }
        }
        public string this[string columnName]
        {
            get
            {
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
