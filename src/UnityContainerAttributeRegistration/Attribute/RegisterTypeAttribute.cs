using System;
using JetBrains.Annotations;

namespace UnityContainerAttributeRegistration.Attribute
{
    /// <summary>
    ///     Mark a class to be registered to an <see cref="Unity.IUnityContainer" />
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterTypeAttribute : System.Attribute
    {
        /// <summary>
        ///     Candidate class for registration to <see cref="Unity" />.
        /// </summary>
        /// <param name="from"><see cref="Type" /> that will be requested.</param>
        /// <param name="lifetimeManager">
        ///     The <see cref="Unity.Lifetime.ITypeLifetimeManager" /> that controls the lifetime of the
        ///     returned instance.
        /// </param>
        public RegisterTypeAttribute([CanBeNull] Type from            = null,
                                     [CanBeNull] Type lifetimeManager = null)
        {
            From            = from;
            LifetimeManager = lifetimeManager;
        }

        /// <summary>
        ///     <see cref="Type" /> that will be requested.
        /// </summary>
        [CanBeNull]
        internal Type From { get; }

        /// <summary>
        ///     The <see cref="LifetimeManager" /> that controls the lifetime
        ///     of the returned instance.
        /// </summary>
        [CanBeNull]
        internal Type LifetimeManager { get; }
    }
}
