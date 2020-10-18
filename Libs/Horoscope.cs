using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Globalization;

namespace Horoscope.Libs
{
    public class Horoscope
    {
        public enum Signs
        {
            Aries = 1,
            Taurus = 2,
            Gemini = 3,
            Cancer = 4,
            Leo = 5,
            Virgo = 6,
            Libra = 7,
            Scorpio = 8,
            Sagittarius = 9,
            Capricorn = 10,
            Aquarius = 11,
            Pisces = 12
        }

        private readonly RestClient _http = new RestClient("http://mobs.horo4.me/apps/horo/v2/");

        public Response GetBySign(Signs sign)
        {
            var request = new RestRequest("getHoroscopeBySignTTY");
            request.AddOrUpdateParameter("sign_id", (int)sign);
            request.AddOrUpdateParameter("content_language", CultureInfo.InstalledUICulture.TwoLetterISOLanguageName);
            return JsonConvert.DeserializeObject<Response>(this._http.Get(request).Content);
        }

        public struct Response
        {
            [JsonProperty("yesterday")]
            public Data Yesterday;
            [JsonProperty("today")]
            public Data Today;
            [JsonProperty("tomorrow")]
            public Data Tomorrow;
            [JsonProperty("week")]
            public Data Week;
            [JsonProperty("month")]
            public Data Month;
            [JsonProperty("year")]
            public List<Data> Year;

            public struct Data
            {
                [JsonProperty("date")]
                public string Date;
                [JsonProperty("text")]
                public string Text;
                [JsonProperty("sign_id")]
                public int SignId;
                [JsonProperty("url")]
                public string Url;
            }
        }
    }
}
