using System;
using System.Collections.Generic;
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
        /// <param name="typesWith"></param>
        /// <returns>Passed <paramref name="container" />.</returns>
        IUnityContainer Populate(IUnityContainer container, IList<Type> typesWith);
    }
}
