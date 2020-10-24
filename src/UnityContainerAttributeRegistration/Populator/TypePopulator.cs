using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Unity;
using Unity.Lifetime;

using UnityContainerAttributeRegistration.Attribute;
using UnityContainerAttributeRegistration.Exention;
using UnityContainerAttributeRegistration.Provider;


namespace UnityContainerAttributeRegistration.Populator
{
    /// <summary>
    ///     Populator for the <see cref="UnityContainerAttributeRegistration.Attribute.RegisterTypeAttribute" />.
    /// </summary>
    internal class TypePopulator : Populator
    {
        /// <summary>
        ///     ctor
        /// </summary>
        /// <param name="assemblyProvider">
        ///     Used <see cref="IAssemblyProvider" /> to find all candidates using <see cref="RegisterTypeAttribute" />
        /// </param>
        public TypePopulator(IAssemblyProvider assemblyProvider) : base(assemblyProvider)
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
            IList<Type> typesWithAttribute = GetTypesWith<RegisterTypeAttribute>(TypeDefined.Inherit)
               .ToList();

            foreach(Type to in typesWithAttribute)
            {
                if(to.IsStatic() || to.IsAbstract)
                {
                    throw new InvalidOperationException(
                        $"Class type must not be static or abstract to be used with RegisterTypeAttribute: {to.FullName}");
                }

                RegisterTypeAttribute attribute = to.GetCustomAttribute<RegisterTypeAttribute>();
                ITypeLifetimeManager lifetimeManager = attribute.LifetimeManager == null
                                                           ? null
                                                           : GetInstanceByType<ITypeLifetimeManager>(attribute.LifetimeManager);

                container.RegisterType(attribute.From, to, lifetimeManager);
            }

            return container;
        }
    }
}
