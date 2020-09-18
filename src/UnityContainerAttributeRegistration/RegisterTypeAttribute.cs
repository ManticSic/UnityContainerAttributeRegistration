using System;

using JetBrains.Annotations;


namespace UnityContainerAttributeRegistration
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterTypeAttribute : Attribute
    {
        public RegisterTypeAttribute([CanBeNull] Type from = null,
                                     TypeLifetimeManager lifetimeManager =
                                         TypeLifetimeManager.ContainerControlledLifetimeManager)
        {
            From            = from;
            LifetimeManager = lifetimeManager;
        }

        [CanBeNull]
        internal Type From { get; }

        internal TypeLifetimeManager LifetimeManager { get; }
    }
}
