using Ecard.API;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Ecard.Models;
using static Ecard.Models.EcardDetail;
using System.Collections.Generic;

namespace Ecard.Services
{
    public static class DependantsService
    {
        public static async Task<Dependant> GetDependants(Guid Id)
        {
            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync(ApiDetail.ApiUrl1 + "api/Dependants/" + Id + "");

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;

                    var getDependants = JsonConvert.DeserializeObject<Dependant>(results);

                    return getDependants;
                }

                if (getData.IsSuccessStatusCode == false)
                {
                    HttpResponseMessage getPM = await client.GetAsync(ApiDetail.ApiUrl1 + "api/Members/Id?Id=" + Id + "");

                    if (getPM.IsSuccessStatusCode)
                    {
                        string results = getPM.Content.ReadAsStringAsync().Result;

                        var data = JsonConvert.DeserializeObject<Dependant>(results);

                        return data;
                    }
                }

                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return null;
            }
        }

        public static async Task<List<EcardDetail>> GetEcardDetails(Guid MemberId)
        {
            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("https://b67c-154-70-3-144.ap.ngrok.io/api/MemberAuth/GetEcardDetails?MemberId=8C804F42-A02D-44B1-9859-C052B0CC6319");

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;

                    var getDependants = JsonConvert.DeserializeObject<List<EcardDetail>>(results);

                    return getDependants;
                }

                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return null;
            }
        }
    }
}