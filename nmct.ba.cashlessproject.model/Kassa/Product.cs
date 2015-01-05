using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model.Kassa
{
    public class Product:IDataErrorInfo
    {
        [Required]
        public int ID { get; set; }
        [DisplayName("Product")]
        [Required(ErrorMessage="De naam is verplicht")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,50}$", ErrorMessage = "Er zijn geen speciale tekens toegelaten")]
        [StringLength(50,MinimumLength=3,ErrorMessage="De naam moet tussen de 3 en 50 karakters bevatten")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Prijs")]
        [Range(0, double.MaxValue, ErrorMessage = "De prijs is geen getal")]
        public double Price { get; set; }

        [DisplayName("Beschrijving")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "De beschrijving moet tussen de 3 en 200 karakters bevatten")]
        public string Description { get; set; }

        [DisplayName("Afbeelding")]
        public byte[] Picture { get; set; } 
        public bool Hidden { get; set; }
       
        //voor aantal, niet in database
        [DisplayName("Aantal")]
        [Range(0, double.MaxValue, ErrorMessage = "Het aantal is geen getal")]
        public double Amount { get; set; }

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
                        MemberName=columnName
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
