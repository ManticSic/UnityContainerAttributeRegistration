using Unity;


namespace UnityContainerAttributeRegistration.Populator
{
    /// <summary>
    ///     Interface for internal populators.
    /// </summary>
    internal interface IPopulator
    {
        /// <summary>
        ///     Populate the passed <paramref name="container" />.
        /// </summary>
        /// <param name="container"><see cref="IUnityContainer" /> to populate.</param>
        /// <returns>Passed <paramref name="container" />.</returns>
        IUnityContainer Populate(IUnityContainer container);
    }
}
