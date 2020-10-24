using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using Unity;
using Unity.Lifetime;

using UnityContainerAttributeRegistration.Provider;
using UnityContainerAttributeRegistration.Attribute;
using UnityContainerAttributeRegistration.Exention;


namespace UnityContainerAttributeRegistration.Populator
{
    internal class InstancePopulator : Populator
    {
        /// <summary>
        ///     ctor
        /// </summary>
        /// <param name="appDomain">Used <see cref="IAssemblyProvider" /> for searching for candidates.</param>
        public InstancePopulator(IAssemblyProvider appDomain) : base(appDomain)
        {
        }

        /// <summary>
        ///     Populate the passed <paramref name="container" />.
        /// </summary>
        /// <param name="container"><see cref="IUnityContainer" /> to populate.</param>
        /// <returns>Passed <paramref name="container" />.</returns>
        /// <exception cref="InvalidOperationException">Class type must not be static or abstract.</exception>
        public override IUnityContainer Populate(IUnityContainer container)
        {
            IList<Type> typesWithAttribute = GetTypesWith<RegisterInstanceProviderAttribute>(TypeDefined.Inherit)
               .ToList();

            IEnumerable<InstanceToRegister> instancesToRegister =
                typesWithAttribute.SelectMany(providerClassType => GetInstancesToRegisterFor(container, providerClassType));

            foreach(InstanceToRegister instanceToRegister in instancesToRegister)
            {
                Type                     type            = instanceToRegister.Type;
                object                   instance        = instanceToRegister.Instance;
                IInstanceLifetimeManager lifetimeManager = instanceToRegister.LifetimeManager;
                container.RegisterInstance(type, instance, lifetimeManager);
            }

            return container;
        }

        /// <summary>
        ///     Create a list of <see cref="InstanceToRegister" /> depending on class marked with <see cref="RegisterInstanceProviderAttribute" />
        /// </summary>
        /// <param name="container"><see cref="IUnityContainer" /> to resolve <paramref name="providerClassType" /></param>
        /// <param name="providerClassType">Class type used to search for <see cref="RegisterInstanceAttribute" /></param>
        /// <returns>List of instances to register with all needed parameters</returns>
        /// <exception cref="InvalidOperationException"><paramref name="providerClassType" /> type must not be static or abstract.</exception>
        private IEnumerable<InstanceToRegister> GetInstancesToRegisterFor(IUnityContainer container, Type providerClassType)
        {
            if(providerClassType.IsStatic() || providerClassType.IsAbstract)
            {
                throw new InvalidOperationException(
                    $"Class type must not be static or abstract to be used with RegisterTypeAttribute: {providerClassType.FullName}");
            }

            object         providerClassInstance = container.Resolve(providerClassType);
            PropertyInfo[] properties            = providerClassType.GetProperties();

            return properties
                  .Where(info => info.CustomAttributes.Any(data => data.AttributeType == typeof(RegisterInstanceAttribute)))
                  .Select(info =>
                          {
                              object                    instance  = info.GetValue(providerClassInstance);
                              RegisterInstanceAttribute attribute = info.GetCustomAttribute<RegisterInstanceAttribute>();
                              Type                      from      = attribute.From;
                              IInstanceLifetimeManager lifetimeManager =
                                  attribute.LifetimeManager == null
                                      ? null
                                      : GetInstanceByType<IInstanceLifetimeManager>(attribute.LifetimeManager);

                              return new InstanceToRegister(instance, from, lifetimeManager);
                          })
                  .ToList();
        }

        /// <summary>
        ///     Wrapper to regsiter isntances
        /// </summary>
        private sealed class InstanceToRegister
        {
            public InstanceToRegister([CanBeNull] object                   instance,
                                      [CanBeNull] Type                     type,
                                      [CanBeNull] IInstanceLifetimeManager lifetimeManager)
            {
                Instance        = instance;
                Type            = type;
                LifetimeManager = lifetimeManager;
            }

            /// <summary>
            ///     Concrete instance to register
            /// </summary>
            [CanBeNull]
            public object Instance { get; }

            /// <summary>
            ///     Requested type
            /// </summary>
            [CanBeNull]
            public Type Type { get; }

            /// <summary>
            ///     Used lifetime manager
            /// </summary>
            [CanBeNull]
            public IInstanceLifetimeManager LifetimeManager { get; }
        }
    }
}
