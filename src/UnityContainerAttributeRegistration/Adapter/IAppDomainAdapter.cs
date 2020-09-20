using System.Collections.Generic;
using System.Reflection;


namespace UnityContainerAttributeRegistration.Adapter
{
    public interface IAppDomainAdapter
    {
        public IList<Assembly> GetAssemblies();
    }
}
