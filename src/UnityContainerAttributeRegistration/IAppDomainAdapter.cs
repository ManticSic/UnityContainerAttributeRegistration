using System.Collections.Generic;
using System.Reflection;


namespace UnityContainerAttributeRegistration
{
    public interface IAppDomainAdapter
    {
        public IList<Assembly> GetAssemblies();
    }
}
