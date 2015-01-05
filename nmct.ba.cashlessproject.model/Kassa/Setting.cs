using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model.Kassa
{
    public class Setting
    {
        public TypeEnum Type { get; set; }
        public int RegisterID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public enum TypeEnum
        {
            klant, bediende
        }
    }
}
