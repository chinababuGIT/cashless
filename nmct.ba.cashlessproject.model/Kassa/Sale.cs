using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model.Kassa
{
    public class Sale
    {
        [Required]
        public int ID { get; set; }
        [DisplayName("Hoeveelheid")]
        [Range(0, double.MaxValue, ErrorMessage = "De prijs is geen getal")]
        public double Amount { get; set; }
        [DisplayName("Totale prijs")]
        [Range(0, double.MaxValue, ErrorMessage = "De totale prijs is geen getal")]
        public double TotalPrice { get; set; }
        [Required]
        [DisplayName("Tijdstip")]
        public long Timestamp { get; set; }

        #region product
        public int ProductID { get; set; }
        [DisplayName("Product")]
        [Required(ErrorMessage = "De naam is verplicht")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,50}$", ErrorMessage = "Er zijn geen speciale tekens toegelaten")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "De naam moet tussen de 3 en 50 karakters bevatten")]
        public string ProductName { get; set; }
        [Required]
        [DisplayName("Prijs")]
        [Range(0, double.MaxValue, ErrorMessage = "De prijs is geen getal")]
        public double ProductPrice { get; set; }
        [DisplayName("Beschrijving")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "De naam moet tussen de 3 en 200 karakters bevatten")]
        public string ProductDescription { get; set; }
        public byte[] ProductPicture { get; set; }
        #endregion

        public int CustomerID { get; set; }
        [DisplayName("Klant")]
        [Required(ErrorMessage = "De klantnaam is verplicht")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "De klantnaam moet tussen de 3 en 50 karakters bevatten")]
        public string CustomerName { get; set; }
        [DisplayName("Adres")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Het adres moet tussen de 3 en 50 karakters bevatten")]
        public string CustomerAddress { get; set; }
        public byte[] CustomerPicture { get; set; }
        [DisplayName("Balans")]
        [Range(0, double.MaxValue, ErrorMessage = "De prijs is geen getal")]
        public double CustomerBalance { get; set; }

        public int RegisterID { get; set; }
        [DisplayName("Kassanaam")]
        [Required(ErrorMessage = "De kassanaam is verplicht")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "De kassanaam moet tussen de 3 en 80 karakters bevatten")]
        public string RegisterName { get; set; }
       
        [DisplayName("Toestel")]
        [Required(ErrorMessage = "Het toestel is verplicht")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Het toestel moet tussen de 3 en 80 karakters bevatten")]
        public string RegisterDevice { get; set; }
        



        public class ByProduct
        {
            [Required]
            [DisplayName("Tijdstip")]
            public long Timestamp { get; set; }
            public int ProductID { get; set; }
            [DisplayName("Product")]
            [Required(ErrorMessage = "De naam is verplicht")]
            [RegularExpression(@"^[a-zA-Z''-'\s]{1,50}$", ErrorMessage = "Er zijn geen speciale tekens toegelaten")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "De naam moet tussen de 3 en 50 karakters bevatten")]
            public string ProductName { get; set; }
            [Required]
            [DisplayName("Prijs")]
            [Range(0, double.MaxValue, ErrorMessage = "De prijs is geen getal")]
            public double ProductPrice { get; set; }
            [DisplayName("Beschrijving")]
            [StringLength(200, MinimumLength = 3, ErrorMessage = "De naam moet tussen de 3 en 200 karakters bevatten")]
            public string ProductDescription { get; set; }
            public byte[] ProductPicture { get; set; }
            public List<Sale> Sales { get; set; }
        }
        public class ByRegister
        {
            [Required]
            [DisplayName("Tijdstip")]
            public long Timestamp { get; set; }
            public int RegisterID { get; set; }
            [DisplayName("Kassanaam")]
            [Required(ErrorMessage = "De kassanaam is verplicht")]
            [StringLength(80, MinimumLength = 3, ErrorMessage = "De kassanaam moet tussen de 3 en 80 karakters bevatten")]
            public string RegisterName { get; set; }
           
            [DisplayName("Toestel")]
            [Required(ErrorMessage = "Het toestel is verplicht")]
            [StringLength(80, MinimumLength = 3, ErrorMessage = "Het toestel moet tussen de 3 en 80 karakters bevatten")]
            public string RegisterDevice { get; set; }
            public List<Sale> Sales { get; set; }
        }


    }

   
}
