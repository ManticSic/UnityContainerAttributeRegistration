using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using Unity;
using Unity.Lifetime;


namespace UnityContainerAttributeRegistration
{
    public class UnityContainerBuilder
    {
        public static IUnityContainer Build()
        {
            IUnityContainer container = new UnityContainer();
            IList<Type> typesWithAttribute = GetTypesWith<RegisterTypeAttribute>(false)
               .ToArray();

            foreach(Type to in typesWithAttribute)
            {
                RegisterTypeAttribute attribute = to.GetCustomAttribute<RegisterTypeAttribute>();

                container.RegisterType(attribute.From ?? to,
                                       to,
                                       GetTypeLifetimeManager(attribute.LifetimeManager));
            }

            return container;
        }

        internal static IEnumerable<Type> GetTypesWith<TAttribute>(bool inherit) where TAttribute : Attribute
        {
            return AppDomain.CurrentDomain
                            .GetAssemblies()
                            .SelectMany(assembly => assembly.GetTypes())
                            .Where(type => type.IsDefined(typeof(TAttribute), inherit))
                ;
        }

        private static ITypeLifetimeManager GetTypeLifetimeManager(TypeLifetimeManager typeLifetimeManager)
        {
            switch(typeLifetimeManager)
            {
                case TypeLifetimeManager.HierarchicalLifetimeManager:
                    return new HierarchicalLifetimeManager();
                case TypeLifetimeManager.SingletonLifetimeManager:
                    return new SingletonLifetimeManager();
                case TypeLifetimeManager.TransientLifetimeManager:
                    return new TransientLifetimeManager();
                case TypeLifetimeManager.ContainerControlledLifetimeManager:
                    return new ContainerControlledLifetimeManager();
                case TypeLifetimeManager.ContainerControlledTransientManager:
                    return new ContainerControlledTransientManager();
                case TypeLifetimeManager.ExternallyControlledLifetimeManager:
                    return new ExternallyControlledLifetimeManager();
                case TypeLifetimeManager.PerResolveLifetimeManager:
                    return new PerResolveLifetimeManager();
                case TypeLifetimeManager.PerThreadLifetimeManager:
                    return new PerThreadLifetimeManager();
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}
