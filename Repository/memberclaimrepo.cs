using membermicroservice.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace membermicroservice.Repository
{
    public class memberclaimrepo : Imemberclaimrepo
    {
        Uri baseAddress = new Uri("http://20.193.144.70/api/claim"); //claim  = https://localhost:44393/api/claim
        HttpClient client;
        public memberclaimrepo()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;

        }
        public string submitClaim(memberclaim obj)
        {
            try
            {
                string data = JsonConvert.SerializeObject(obj);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(client.BaseAddress, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return "success";

                }
                return "failed";
            }
            catch (Exception e)
            {
                return "Exception Occured";
            }
        }

        public memberclaim viewClaimStatus(int id, memberclaim obj)
        {
            try
            {
                memberclaim ob = new memberclaim();

                string data = JsonConvert.SerializeObject(obj);

                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PutAsync(client.BaseAddress + "/" + id, content).Result;

                if (response.IsSuccessStatusCode)
                {

                    string data1 = response.Content.ReadAsStringAsync().Result;
                    ob = JsonConvert.DeserializeObject<memberclaim>(data);

                }

                return ob;//for claimstatus.cshtml
            }
            catch(Exception)
            {
                return null;
            }


        }
    }
}
    

