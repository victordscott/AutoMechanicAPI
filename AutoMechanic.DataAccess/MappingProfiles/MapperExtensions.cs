using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.MappingProfiles
{
    public static class MapperExtensions
    {
        public static T ResolveJson<T>(this JObject jobj, string target)
        {
            return JsonConvert.DeserializeObject<T>(jobj.SelectToken(target).ToString());
        }
    }
}
