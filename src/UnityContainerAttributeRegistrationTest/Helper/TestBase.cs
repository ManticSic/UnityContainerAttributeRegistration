using System;
using Unity;
using static NUnit.Framework.Assert;


namespace UnityContainerAttributeRegistrationTest.Helper
{
    public abstract class TestBase
    {
        protected void IsUnityContainerRegistration(IContainerRegistration registration)
        {
            bool registeredType = registration.RegisteredType == typeof(IUnityContainer);
            bool mappedToType   = registration.MappedToType == typeof(UnityContainer);

            IsTrue(registeredType);
            IsTrue(mappedToType);
        }

        protected void IsExpectedRegisteredContainer(IContainerRegistration registration,
                                                     Type                   expectedFrom,
                                                     Type                   expectedTo,
                                                     Type                   expectedTypeLifetimeManagerType)
        {
            bool registeredType  = registration.RegisteredType == expectedFrom;
            bool mappedToType    = registration.MappedToType == expectedTo;
            bool lifetimeManager = registration.LifetimeManager.GetType() == expectedTypeLifetimeManagerType;

            IsTrue(registeredType);
            IsTrue(mappedToType);
            IsTrue(lifetimeManager);
        }
    }
}
