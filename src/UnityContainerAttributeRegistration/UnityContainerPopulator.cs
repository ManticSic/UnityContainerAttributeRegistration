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
    /// <summary>
    /// Creates a populated or populates an <see cref="Unity.IUnityContainer" />, depending on the provided assemblies
    /// </summary>
    public sealed class UnityContainerPopulator
    {
        private readonly IAppDomainAdapter appDomain;

        /// <summary>
        /// Use <see cref="System.AppDomain.CurrentDomain"/> to populate an <see cref="Unity.IUnityContainer" />
        /// </summary>
        public UnityContainerPopulator() : this(new AppDomainAdapter())
        {
        }

        /// <summary>
        /// Use <paramref name="appDomain"/> to populate an <see cref="Unity.IUnityContainer" />
        /// </summary>
        /// <param name="appDomain">Custom <see cref="UnityContainerAttributeRegistration.Adapter.IAppDomainAdapter"/></param>
        public UnityContainerPopulator([NotNull] IAppDomainAdapter appDomain)
        {
            this.appDomain = appDomain;
        }

        /// <summary>
        /// Create and populate a new <see cref="Unity.UnityContainer"/>
        /// </summary>
        /// <returns>New <see cref="Unity.UnityContainer"/> with registered types</returns>
        public IUnityContainer Populate()
        {
            return Populate(new UnityContainer());
        }

        /// <summary>
        /// Populate <paramref name="container"/>
        /// </summary>
        /// <param name="container"><see cref="Unity.IUnityContainer"/> to register types</param>
        /// <returns><paramref name="container"/></returns>
        public IUnityContainer Populate([NotNull] IUnityContainer container)
        {
            RegisterByTypeAttribute(container);

            return container;
        }

        private void RegisterByTypeAttribute([NotNull] IUnityContainer container)
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
