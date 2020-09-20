using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using Unity;
using Unity.Lifetime;

using UnityContainerAttributeRegistration.Adapter;
using UnityContainerAttributeRegistration.Attribute;


namespace UnityContainerAttributeRegistration
{
    public sealed class UnityContainerBuilder
    {
        private readonly IAppDomainAdapter appDomain;

        public UnityContainerBuilder() : this(new AppDomainAdapter())
        {
        }

        public UnityContainerBuilder(IAppDomainAdapter appDomain)
        {
            this.appDomain = appDomain;
        }

        public IUnityContainer Build()
        {
            return Build(new UnityContainer());
        }

        public IUnityContainer Build(IUnityContainer container)
        {
            RegisterByTypeAttribute(container);

            return container;
        }

        private void RegisterByTypeAttribute(IUnityContainer container)
        {
            IList<Type> typesWithAttribute = GetTypesWith<RegisterTypeAttribute>(TypeDefined.Inherit)
               .ToArray();

            foreach(Type to in typesWithAttribute)
            {
                // todo check for to.IsAbstract and to.IsInterface and throw

                RegisterTypeAttribute attribute = to.GetCustomAttribute<RegisterTypeAttribute>();
                ITypeLifetimeManager lifetimeManager =
                    attribute.LifetimeManager == null ? null : GetInstanceByType<ITypeLifetimeManager>(attribute.LifetimeManager);

                container.RegisterType(attribute.From ?? to,
                                       to,
                                       lifetimeManager);
            }
        }

        [CanBeNull]
        private T GetInstanceByType<T>([CanBeNull] Type typeLifetimeManagerType)
        {
            Type targetType = typeof(T);

            if(typeLifetimeManagerType == null)
            {
                throw new ArgumentNullException(nameof(typeLifetimeManagerType));
            }

            if(!targetType.IsAssignableFrom(typeLifetimeManagerType))
            {
                throw new InvalidOperationException(
                    $"Type {typeLifetimeManagerType.FullName} cannot be assigned from {targetType.FullName}");
            }

            ConstructorInfo ctor = typeLifetimeManagerType.GetConstructor(Type.EmptyTypes);

            if(ctor == null)
            {
                throw new InvalidOperationException(
                    $"Cannot create instance of ITypeLifetimeManager. No default constructor found for {typeLifetimeManagerType.FullName}");
            }

            return (T) ctor.Invoke(new object[0]);
        }

        private IEnumerable<Type> GetTypesWith<TAttribute>(TypeDefined typeDefined) where TAttribute : System.Attribute
        {
            return appDomain.GetAssemblies()
                            .SelectMany(assembly => assembly.GetTypes())
                            .Where(type => type.IsDefined(typeof(TAttribute), typeDefined == TypeDefined.Inherit))
                ;
        }
    }
}
