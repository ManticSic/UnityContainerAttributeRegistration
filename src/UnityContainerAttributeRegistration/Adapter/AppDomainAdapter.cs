using System;
using System.Collections.Generic;
using System.Reflection;


namespace UnityContainerAttributeRegistration.Adapter
{
    internal class AppDomainAdapter : IAppDomainAdapter
    {
        public IList<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}
