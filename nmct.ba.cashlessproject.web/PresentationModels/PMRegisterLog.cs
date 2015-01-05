using nmct.ba.cashlessproject.model.ITBedrijf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.web.PresentationModels
{
    public class PMRegisterLog
    {
        public PMRegisterLog()
        {
            RegisterLogNew = new RegisterLog();
        }

        public RegisterLog RegisterLogNew { get; set; }
        public List<RegisterLog> RegisterLogs { get; set; }
        public SelectList SelectRegister { get; set; }
    }
}