using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEO_Quiz
{
    public class AppBootstrapper
    {
        public AppBootstrapper()
        {
            DependencyContainer.Instance.AddDependency("SettingsManager", new AppSettingsManager());
        }
    }
}
