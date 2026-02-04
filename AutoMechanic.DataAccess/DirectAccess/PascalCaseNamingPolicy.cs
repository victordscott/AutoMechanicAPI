using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;

namespace AutoMechanic.DataAccess.DirectAccess
{
    public class PascalCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return string.Concat(name.Split('_').Select(Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase));
        }
    }
}
