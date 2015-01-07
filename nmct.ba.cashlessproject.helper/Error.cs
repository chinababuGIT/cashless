using Newtonsoft.Json;
using nmct.ba.cashlessproject.model.Kassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.helper
{
    public class Error
    {
        public static async Task<bool> Log(Log log,string token)
        {
            string URL_log = "http://localhost:8080/api/log";
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(token);
                string json = JsonConvert.SerializeObject(log);
                HttpResponseMessage response = await client.PostAsync(URL_log, new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }

        }
    }
}
