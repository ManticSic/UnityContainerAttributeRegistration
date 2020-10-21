using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

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

        /// <summary>
        ///     Create an instance for <paramref name="objectType" />.
        /// </summary>
        /// <param name="objectType"><see cref="Type" /> used to create an instance.</param>
        /// <typeparam name="T"><paramref name="objectType" /> must be type-equal to, inherit or implement <typeparamref name="T" />.</typeparam>
        /// <returns>New instance of <paramref name="objectType" /> as <typeparamref name="T" />.</returns>
        /// <exception cref="InvalidOperationException">Cannot create an instance of <paramref name="objectType" />. Whether <paramref name="objectType" /> is not type-equal, does not inherit or implement <typeparamref name="T" /> or has no default constructor.</exception>
        [NotNull]
        private T GetInstanceByType<T>([NotNull] Type objectType)
        {
            Type targetType = typeof(T);

            if(!targetType.IsAssignableFrom(objectType))
            {
                throw new InvalidOperationException($"Type {objectType.FullName} cannot be assigned from {targetType.FullName}");
            }

            ConstructorInfo ctor = objectType.GetConstructor(Type.EmptyTypes);

            if(ctor == null)
            {
                throw new InvalidOperationException(
                    $"Cannot create instance of ITypeLifetimeManager. No default constructor found for {objectType.FullName}");
            }

            return (T) ctor.Invoke(new object[0]);
        }
    }
}
