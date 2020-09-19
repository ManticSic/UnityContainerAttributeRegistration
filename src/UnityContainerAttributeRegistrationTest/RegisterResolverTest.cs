using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Unity;
using Unity.Lifetime;

using UnityContainerAttributeRegistration;

using static NUnit.Framework.Assert;


namespace UnityContainerAttributeRegistrationTest
{
    public class RegisterResolverTest
    {
        [Test]
        public void TestBuild_Default()
        {
            IUnityContainer container = UnityContainerBuilder.Build();

            IList<IContainerRegistration> result = container.Registrations.ToArray();

            AreEqual(10, result.Count());

            IsTrue(IsUnityContainerRegistration(result[0]));

            IsTrue(IsExpectedRegisteredContainer(result[1],
                                                 typeof(ClassDefault),
                                                 typeof(ClassDefault),
                                                 typeof(ContainerControlledLifetimeManager)));

            IsTrue(IsExpectedRegisteredContainer(result[2],
                                                 typeof(IInterface),
                                                 typeof(ClassHierarchicalLifetimeManager),
                                                 typeof(HierarchicalLifetimeManager)));

            IsTrue(IsExpectedRegisteredContainer(result[3],
                                                 typeof(ClassSingletonLifetimeManager),
                                                 typeof(ClassSingletonLifetimeManager),
                                                 typeof(SingletonLifetimeManager)));

            IsTrue(IsExpectedRegisteredContainer(result[4],
                                                 typeof(ClassTransientLifetimeManager),
                                                 typeof(ClassTransientLifetimeManager),
                                                 typeof(TransientLifetimeManager)));

            IsTrue(IsExpectedRegisteredContainer(result[5],
                                                 typeof(ClassContainerControlledLifetimeManager),
                                                 typeof(ClassContainerControlledLifetimeManager),
                                                 typeof(ContainerControlledLifetimeManager)));

            IsTrue(IsExpectedRegisteredContainer(result[6],
                                                 typeof(ClassContainerControlledTransientManager),
                                                 typeof(ClassContainerControlledTransientManager),
                                                 typeof(ContainerControlledTransientManager)));

            IsTrue(IsExpectedRegisteredContainer(result[7],
                                                 typeof(ClassExternallyControlledLifetimeManager),
                                                 typeof(ClassExternallyControlledLifetimeManager),
                                                 typeof(ExternallyControlledLifetimeManager)));

            IsTrue(IsExpectedRegisteredContainer(result[8],
                                                 typeof(ClassPerResolveLifetimeManager),
                                                 typeof(ClassPerResolveLifetimeManager),
                                                 typeof(PerResolveLifetimeManager)));

            IsTrue(IsExpectedRegisteredContainer(result[9],
                                                 typeof(ClassPerThreadLifetimeManager),
                                                 typeof(ClassPerThreadLifetimeManager),
                                                 typeof(PerThreadLifetimeManager)));
        }

        [Test]
        public void TestBuild_WithCustomContainer()
        {
            IUnityContainer container = new UnityContainer();

            IUnityContainer result = UnityContainerBuilder.Build(container);

            AreSame(container, result);
        }

        private bool IsUnityContainerRegistration(IContainerRegistration registration)
        {
            bool registeredType = registration.RegisteredType == typeof(IUnityContainer);
            bool mappedToType   = registration.MappedToType == typeof(UnityContainer);

            return registeredType && mappedToType;
        }

        private bool IsExpectedRegisteredContainer(IContainerRegistration registration,
                                                   Type                   expectedRegisteredType,
                                                   Type                   expectedMappedToType,
                                                   Type                   expectedTypeLifetimeManagerType)
        {
            bool registeredType  = registration.RegisteredType == expectedRegisteredType;
            bool mappedToType    = registration.MappedToType == expectedMappedToType;
            bool lifetimeManager = registration.LifetimeManager.GetType() == expectedTypeLifetimeManagerType;

            return registeredType && mappedToType && lifetimeManager;
        }

        [RegisterType]
        private class ClassDefault
        {
        }

        [RegisterType(typeof(IInterface), TypeLifetimeManager.HierarchicalLifetimeManager)]
        private class ClassHierarchicalLifetimeManager : IInterface
        {
        }

        [RegisterType(typeof(ClassSingletonLifetimeManager), TypeLifetimeManager.SingletonLifetimeManager)]
        private class ClassSingletonLifetimeManager
        {
        }

        [RegisterType(lifetimeManager: TypeLifetimeManager.TransientLifetimeManager)]
        private class ClassTransientLifetimeManager
        {
        }

        [RegisterType(lifetimeManager: TypeLifetimeManager.ContainerControlledLifetimeManager)]
        private class ClassContainerControlledLifetimeManager
        {
        }

        [RegisterType(lifetimeManager: TypeLifetimeManager.ContainerControlledTransientManager)]
        private class ClassContainerControlledTransientManager
        {
        }

        [RegisterType(lifetimeManager: TypeLifetimeManager.ExternallyControlledLifetimeManager)]
        private class ClassExternallyControlledLifetimeManager
        {
        }

        [RegisterType(lifetimeManager: TypeLifetimeManager.PerResolveLifetimeManager)]
        private class ClassPerResolveLifetimeManager
        {
        }

        [RegisterType(lifetimeManager: TypeLifetimeManager.PerThreadLifetimeManager)]
        private class ClassPerThreadLifetimeManager
        {
        }

        private interface IInterface
        {
        }
    }
}
