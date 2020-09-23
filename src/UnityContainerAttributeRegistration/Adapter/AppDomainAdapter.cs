using System;
using System.Collections.Generic;
using System.Reflection;


namespace UnityContainerAttributeRegistration.Adapter
{
    /// <inheritdoc cref="IAppDomainAdapter" />
    internal class AppDomainAdapter : IAppDomainAdapter
    {
        /// <inheritdoc cref="IAppDomainAdapter.GetAssemblies" />
        public IList<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}
