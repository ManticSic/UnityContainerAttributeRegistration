using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using Unity;

using UnityContainerAttributeRegistration.Provider;


namespace UnityContainerAttributeRegistration.Populator
{
    /// <summary>
    ///     <see cref="IPopulator" /> providing some basic functionality for reflection.
    /// </summary>
    internal abstract class Populator : IPopulator
    {
        private readonly IAssemblyProvider assemblyProvider;

        /// <summary>
        ///     ctor
        /// </summary>
        /// <param name="assemblyProvider">Used <see cref="IAssemblyProvider" /> for searching for candidates.</param>
        protected Populator(IAssemblyProvider assemblyProvider)
        {
            this.assemblyProvider = assemblyProvider;
        }

        /// <inheritdoc cref="IPopulator.Populate" />
        public abstract IUnityContainer Populate(IUnityContainer container);

        /// <summary>
        ///     Find all types using <typeparamref name="TAttribute" />.
        /// </summary>
        /// <param name="typeDefined">Using inheritance or not.</param>
        /// <typeparam name="TAttribute"><see cref="System.Attribute" /> used by searched <see cref="Type" />s.</typeparam>
        /// <returns>List of all <see cref="Type" />s in an <see cref="IAssemblyProvider" /> using <typeparamref name="TAttribute" />.</returns>
        protected IEnumerable<Type> GetTypesWith<TAttribute>(TypeDefined typeDefined) where TAttribute : System.Attribute
        {
            return assemblyProvider.GetAssemblies()
                            .SelectMany(assembly => assembly.GetTypes())
                            .Where(type => type.IsDefined(typeof(TAttribute), typeDefined == TypeDefined.Inherit))
                ;
        }

        /// <summary>
        ///     Create an instance for <paramref name="objectType" />.
        /// </summary>
        /// <param name="objectType"><see cref="Type" /> used to create an instance.</param>
        /// <typeparam name="T"><paramref name="objectType" /> must be type-equal to, inherit or implement <typeparamref name="T" />.</typeparam>
        /// <returns>New instance of <paramref name="objectType" /> as <typeparamref name="T" />.</returns>
        /// <exception cref="InvalidOperationException">Cannot create an instance of <paramref name="objectType" />. Whether <paramref name="objectType" /> is not type-equal, does not inherit or implement <typeparamref name="T" /> or has no default constructor.</exception>
        [NotNull]
        protected T GetInstanceByType<T>([NotNull] Type objectType)
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
