using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class EID
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string BirthLocation { get; set; }

        public string Nationality { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
      
        public string RegisterNumber { get; set; }
        public string ChipNumber { get; set; }
        public byte[] Image { get; set; }
        public string Error { get; set; }
    }
}
