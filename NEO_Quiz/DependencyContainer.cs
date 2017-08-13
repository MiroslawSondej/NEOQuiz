using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEO_Quiz
{
    class DependencyContainer
    {
        private Dictionary<string, object> objectContainer;
        private static DependencyContainer instance;

        private DependencyContainer()
        {
            objectContainer = new Dictionary<string, object>();
        }
        public static DependencyContainer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DependencyContainer();
                }
                return instance;
            }
        }

        public void AddDependency(string name, object dependencyInstance)
        {
            if(!objectContainer.ContainsKey(name))
            {
                objectContainer.Add(name, dependencyInstance);
            }
        }
        public bool IsDependencyDefined(string name)
        {
            bool defined = objectContainer.ContainsKey(name);
            return defined;
        }
        public object GetDependencyInstance(string name)
        {
            if (objectContainer.ContainsKey(name))
            {
                return objectContainer[name];
            }
            return null;
        }
        public T GetDependencyInstance<T>(string name)
        {
            try
            {
                T tObject = (T)GetDependencyInstance(name);
                return tObject;
            }
            catch (InvalidCastException e) { }

            return default(T);
        }
    }
}
