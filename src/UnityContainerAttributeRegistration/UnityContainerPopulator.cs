using JetBrains.Annotations;

using Unity;

using UnityContainerAttributeRegistration.Populator;
using UnityContainerAttributeRegistration.Provider;


namespace UnityContainerAttributeRegistration
{
    /// <summary>
    ///     Creates a populated or populates an <see cref="Unity.IUnityContainer" />, depending on the provided assemblies
    /// </summary>
    public sealed class UnityContainerPopulator
    {
        private readonly IPopulator        typePopulator;
        private readonly IPopulator        instancePopulator;

        /// <summary>
        ///     Use <see cref="System.AppDomain.CurrentDomain" /> to populate an <see cref="Unity.IUnityContainer" />
        /// </summary>
        public UnityContainerPopulator() : this(new AssemblyProvider())
        {
        }

        /// <summary>
        ///     Use <paramref name="appDomain" /> to populate an <see cref="Unity.IUnityContainer" />
        /// </summary>
        /// <param name="appDomain">Custom <see cref="IAssemblyProvider" /></param>
        public UnityContainerPopulator([NotNull] IAssemblyProvider appDomain)
        {
            typePopulator = new TypePopulator(appDomain);
            instancePopulator = new InstancePopulator(appDomain);
        }

        /// <summary>
        ///     Create and populate a new <see cref="Unity.UnityContainer" />
        /// </summary>
        /// <returns>New <see cref="Unity.UnityContainer" /> with registered types</returns>
        public IUnityContainer Populate()
        {
            return Populate(new UnityContainer());
        }

        /// <summary>
        ///     Populate <paramref name="container" />
        /// </summary>
        /// <param name="container"><see cref="Unity.IUnityContainer" /> to register types</param>
        /// <returns>
        ///     <paramref name="container" />
        /// </returns>
        public IUnityContainer Populate([NotNull] IUnityContainer container)
        {
            typePopulator.Populate(container);
            instancePopulator.Populate(container);

            return container;
        }
    }
}
