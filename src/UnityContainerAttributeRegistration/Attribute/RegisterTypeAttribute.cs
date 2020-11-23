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
            Name            = null;
            From            = from;
            LifetimeManager = lifetimeManager;
        }

        /// <summary>
        ///     Candidate for registration to <see cref="Unity" />.
        /// </summary>
        /// <param name="name">Name for registration</param>
        /// <param name="from"><see cref="Type" /> that will be requested.</param>
        /// <param name="lifetimeManager">
        ///     The <see cref="Unity.Lifetime.IInstanceLifetimeManager" /> that controls the lifetime of
        ///     the returned instance.
        /// </param>
        public RegisterTypeAttribute([NotNull]   string name,
                                     [CanBeNull] Type   from            = null,
                                     [CanBeNull] Type   lifetimeManager = null)
        {
            Name            = name;
            From            = from;
            LifetimeManager = lifetimeManager;
        }

        /// <summary>
        ///     Name for registration.
        /// </summary>
        [CanBeNull]
        internal string Name { get; }

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
