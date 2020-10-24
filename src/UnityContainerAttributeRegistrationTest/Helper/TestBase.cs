using System;

using Unity;


namespace UnityContainerAttributeRegistrationTest.Helper
{
    public abstract class TestBase
    {
        protected bool IsUnityContainerRegistration(IContainerRegistration registration)
        {
            bool registeredType = registration.RegisteredType == typeof(IUnityContainer);
            bool mappedToType   = registration.MappedToType == typeof(UnityContainer);

            return registeredType && mappedToType;
        }

        protected bool IsExpectedRegisteredContainer(IContainerRegistration registration,
                                                     Type                   expectedFrom,
                                                     Type                   expectedTo,
                                                     Type                   expectedTypeLifetimeManagerType)
        {
            bool registeredType  = registration.RegisteredType == expectedFrom;
            bool mappedToType    = registration.MappedToType == expectedTo;
            bool lifetimeManager = registration.LifetimeManager.GetType() == expectedTypeLifetimeManagerType;

            return registeredType && mappedToType && lifetimeManager;
        }
    }
}
