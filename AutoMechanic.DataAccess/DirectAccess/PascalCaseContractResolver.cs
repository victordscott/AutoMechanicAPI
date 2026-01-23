using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.DirectAccess;
public class PascalCaseContractResolver : DefaultContractResolver
{
    protected override string ResolvePropertyName(string propertyName) => string.Concat(
        propertyName.Split('_')
        .Select(Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase)
    );
}
