using System;

using JetBrains.Annotations;


namespace UnityContainerAttributeRegistration.Attribute
{
    /// <summary>
    ///     Mark a method to be registered as factory to an <see cref="Unity.IUnityContainer" />
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RegisterFactoryAttribute : System.Attribute
    {
        /// <summary>
        ///     Candidate for registration to <see cref="Unity" />.
        /// </summary>
        public RegisterFactoryAttribute([CanBeNull] Type from            = null,
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
