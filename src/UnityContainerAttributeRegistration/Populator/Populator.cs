using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
