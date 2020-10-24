using System;

using JetBrains.Annotations;


namespace UnityContainerAttributeRegistration.Attribute
{
    /// <summary>
    ///     Mark a property to be registered to an <see cref="Unity.IUnityContainer" />
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RegisterInstanceAttribute: System.Attribute
    {
        /// <summary>
        ///     Candidate for registration to <see cref="Unity" />.
        /// </summary>
        /// <param name="from"><see cref="Type" /> that will be requested.</param>
        /// <param name="lifetimeManager">The <see cref="Unity.Lifetime.IInstanceLifetimeManager" /> that controls the lifetime of the returned instance.</param>
        public RegisterInstanceAttribute([CanBeNull] Type from            = null,
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
