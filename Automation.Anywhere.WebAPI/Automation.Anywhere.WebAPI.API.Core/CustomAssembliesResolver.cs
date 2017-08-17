using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;
using Automation.Anywhere.WebAPI.API.Core.Controllers;

namespace Automation.Anywhere.WebAPI.API.Core
{
    public class CustomAssembliesResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            var baseAssemblies = base.GetAssemblies().ToList();
            var assemblies = new List<Assembly>(baseAssemblies) { typeof(DocumentVaultsController).Assembly };
            baseAssemblies.AddRange(assemblies);

            return baseAssemblies.Distinct().ToList();
        }
    }
}
