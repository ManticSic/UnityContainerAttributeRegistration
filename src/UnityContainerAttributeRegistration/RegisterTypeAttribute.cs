using System;

using JetBrains.Annotations;


namespace UnityContainerAttributeRegistration
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterTypeAttribute : Attribute
    {
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
