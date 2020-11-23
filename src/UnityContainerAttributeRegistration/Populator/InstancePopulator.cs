using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using Unity;
using Unity.Lifetime;

using UnityContainerAttributeRegistration.Attribute;


namespace UnityContainerAttributeRegistration.Populator
{
    /// <summary>
    ///     Populator for the <see cref="UnityContainerAttributeRegistration.Attribute.RegisterInstanceAttribute" />.
    /// </summary>
    internal class InstancePopulator : Populator
    {
        /// <inheritdoc cref="Populator.Populate" />
        /// <exception cref="InvalidOperationException">Class type must not be static or abstract.</exception>
        public override IUnityContainer Populate(IUnityContainer container, IList<Type> typesWithAttribute)
        {
            IEnumerable<InstanceToRegister> instancesToRegister =
                typesWithAttribute.SelectMany(providerClassType => GetInstancesToRegisterFor(container, providerClassType));

            foreach(InstanceToRegister instanceToRegister in instancesToRegister)
            {
                Type                     type            = instanceToRegister.Type;
                string                   name            = instanceToRegister.Name;
                object                   instance        = instanceToRegister.Instance;
                IInstanceLifetimeManager lifetimeManager = instanceToRegister.LifetimeManager;
                container.RegisterInstance(type, name, instance, lifetimeManager);
            }

            return container;
        }

        /// <summary>
        ///     Create a list of <see cref="InstanceToRegister" /> depending on class marked with <see cref="RegisterProviderAttribute" />
        /// </summary>
        /// <param name="container"><see cref="IUnityContainer" /> to resolve <paramref name="providerClassType" /></param>
        /// <param name="providerClassType">Class type used to search for <see cref="RegisterInstanceAttribute" /></param>
        /// <returns>List of instances to register with all needed parameters</returns>
        /// <exception cref="InvalidOperationException"><paramref name="providerClassType" /> type must not be static or abstract.</exception>
        private IEnumerable<InstanceToRegister> GetInstancesToRegisterFor(IUnityContainer container, Type providerClassType)
        {
            object         providerClassInstance = container.Resolve(providerClassType);
            PropertyInfo[] properties            = providerClassType.GetProperties();

            return properties
                  .Where(info => info.CustomAttributes.Any(data => data.AttributeType == typeof(RegisterInstanceAttribute)))
                  .Select(info =>
                          {
                              object                    instance  = info.GetValue(providerClassInstance);
                              RegisterInstanceAttribute attribute = info.GetCustomAttribute<RegisterInstanceAttribute>();
                              string                    name      = attribute.Name;
                              Type                      from      = attribute.From;
                              IInstanceLifetimeManager lifetimeManager =
                                  attribute.LifetimeManager == null
                                      ? null
                                      : GetInstanceByType<IInstanceLifetimeManager>(attribute.LifetimeManager);

                              return new InstanceToRegister(instance, name, from, lifetimeManager);
                          })
                  .ToList();
        }

        /// <summary>
        ///     Wrapper to register isntances
        /// </summary>
        private sealed class InstanceToRegister
        {
            public InstanceToRegister([CanBeNull] object                   instance,
                                      [CanBeNull] string                   name,
                                      [CanBeNull] Type                     type,
                                      [CanBeNull] IInstanceLifetimeManager lifetimeManager)
            {
                Instance        = instance;
                Name            = name;
                Type            = type;
                LifetimeManager = lifetimeManager;
            }

            /// <summary>
            ///     Concrete instance to register
            /// </summary>
            [CanBeNull]
            public object Instance { get; }

            /// <summary>
            ///     Name for registration.
            /// </summary>
            [CanBeNull]
            public string Name { get; }

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
