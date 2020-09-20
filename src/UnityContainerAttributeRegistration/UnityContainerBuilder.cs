﻿using System;
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
            return Build(new UnityContainer());
        }

        public static IUnityContainer Build(IUnityContainer container)
        {
            RegisterByTypeAttribute(container);

            return container;
        }

        private static void RegisterByTypeAttribute(IUnityContainer container)
        {
            IList<Type> typesWithAttribute = GetTypesWith<RegisterTypeAttribute>(TypeDefined.Inherit)
               .ToArray();

            foreach(Type to in typesWithAttribute)
            {
                // todo check for to.IsAbstract and to.IsInterface and throw

                RegisterTypeAttribute attribute = to.GetCustomAttribute<RegisterTypeAttribute>();

                container.RegisterType(attribute.From ?? to,
                                       to,
                                       GetTypeLifetimeManager(attribute.LifetimeManager));
            }
        }

        private static IEnumerable<Type> GetTypesWith<TAttribute>(TypeDefined typeDefined) where TAttribute : Attribute
        {
            return AppDomain.CurrentDomain
                            .GetAssemblies()
                            .SelectMany(assembly => assembly.GetTypes())
                            .Where(type => type.IsDefined(typeof(TAttribute), typeDefined == TypeDefined.Inherit))
                ;
        }

        private static ITypeLifetimeManager GetTypeLifetimeManager(TypeLifetimeManager typeLifetimeManager)
        {
            switch(typeLifetimeManager)
            {
                case TypeLifetimeManager.Default:
                {
                    return null;
                }
                case TypeLifetimeManager.HierarchicalLifetimeManager:
                {
                    return new HierarchicalLifetimeManager();
                }
                case TypeLifetimeManager.SingletonLifetimeManager:
                {
                    return new SingletonLifetimeManager();
                }
                case TypeLifetimeManager.TransientLifetimeManager:
                {
                    return new TransientLifetimeManager();
                }
                case TypeLifetimeManager.ContainerControlledLifetimeManager:
                {
                    return new ContainerControlledLifetimeManager();
                }
                case TypeLifetimeManager.ContainerControlledTransientManager:
                {
                    return new ContainerControlledTransientManager();
                }
                case TypeLifetimeManager.ExternallyControlledLifetimeManager:
                {
                    return new ExternallyControlledLifetimeManager();
                }
                case TypeLifetimeManager.PerResolveLifetimeManager:
                {
                    return new PerResolveLifetimeManager();
                }
                case TypeLifetimeManager.PerThreadLifetimeManager:
                {
                    return new PerThreadLifetimeManager();
                }
                default:
                {
                    throw new InvalidEnumArgumentException();
                }
            }
        }
    }
}
