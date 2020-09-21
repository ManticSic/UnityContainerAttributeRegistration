using System;

using JetBrains.Annotations;


namespace UnityContainerAttributeRegistration.Attribute
{
    /// <summary>
    /// Mark a class to be registered to an <see cref="Unity.IUnityContainer"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterTypeAttribute : System.Attribute
    {
        /// <param name="from"><see cref="Type"/> that will be requested.</param>
        /// <param name="lifetimeManager">The <see cref="Unity.Lifetime.ITypeLifetimeManager"/> that controls the lifetime of the returned instance.</param>
        public RegisterTypeAttribute([CanBeNull] Type from            = null,
                                     [CanBeNull] Type lifetimeManager = null)
        {
            From            = from;
            LifetimeManager = lifetimeManager;
        }

        [CanBeNull]
        internal Type From { get; }

        [CanBeNull]
        internal Type LifetimeManager { get; }
    }
}
