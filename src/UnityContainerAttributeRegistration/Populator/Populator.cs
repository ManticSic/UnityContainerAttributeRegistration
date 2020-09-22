using System;
using System.Collections.Generic;
using System.Linq;

using Unity;

using UnityContainerAttributeRegistration.Adapter;


namespace UnityContainerAttributeRegistration.Populator
{
    internal abstract class Populator : IPopulator
    {
        private readonly IAppDomainAdapter appDomain;

        public Populator(IAppDomainAdapter appDomain)
        {
            this.appDomain = appDomain;
        }

        public abstract IUnityContainer Populate(IUnityContainer container);

        protected IEnumerable<Type> GetTypesWith<TAttribute>(TypeDefined typeDefined) where TAttribute : System.Attribute
        {
            return appDomain.GetAssemblies()
                            .SelectMany(assembly => assembly.GetTypes())
                            .Where(type => type.IsDefined(typeof(TAttribute), typeDefined == TypeDefined.Inherit))
                ;
        }
    }
}
