using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityContainerAttributeRegistration.Provider
{
    /// <inheritdoc cref="IAssemblyProvider" />
    internal class AssemblyProvider : IAssemblyProvider
    {
        /// <inheritdoc cref="IAssemblyProvider.GetAssemblies" />
        public IList<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}
