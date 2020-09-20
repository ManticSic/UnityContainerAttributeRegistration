using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Moq;

using NUnit.Framework;

using Unity;
using Unity.Lifetime;

using UnityContainerAttributeRegistration;

using UnityContainerAttributeRegistrationTest.Helper;
using UnityContainerAttributeRegistrationTest.Helper.LifetimeManager;

using static NUnit.Framework.Assert;


namespace UnityContainerAttributeRegistrationTest
{
    public class RegisterResolverTest
    {
        [Test]
        [TestCase(null, typeof(TransientLifetimeManager))]
        [TestCase(typeof(HierarchicalLifetimeManager), typeof(HierarchicalLifetimeManager))]
        [TestCase(typeof(SingletonLifetimeManager), typeof(SingletonLifetimeManager))]
        [TestCase(typeof(TransientLifetimeManager), typeof(TransientLifetimeManager))]
        [TestCase(typeof(ContainerControlledLifetimeManager), typeof(ContainerControlledLifetimeManager))]
        [TestCase(typeof(ContainerControlledTransientManager), typeof(ContainerControlledTransientManager))]
        [TestCase(typeof(ExternallyControlledLifetimeManager), typeof(ExternallyControlledLifetimeManager))]
        [TestCase(typeof(PerResolveLifetimeManager), typeof(PerResolveLifetimeManager))]
        [TestCase(typeof(PerThreadLifetimeManager), typeof(PerThreadLifetimeManager))]
        public void TestBuild_TypeLifetimeManagers(Type lifetimeManagerType, Type expectedTypeLifetimeMangerType)
        {
            Mock<Assembly>          assemblyMock  = new Mock<Assembly>();
            Mock<IAppDomainAdapter> appDomainMock = new Mock<IAppDomainAdapter>();

            Type type = new FakeType("Test",
                                     "ClassA",
                                     assemblyMock.Object,
                                     attributes: new RegisterTypeAttribute(null, lifetimeManagerType));

            Type[] typesWithAttribute =
            {
                type
            };

            assemblyMock.Setup(mock => mock.GetTypes())
                        .Returns(typesWithAttribute);

            appDomainMock.Setup(mock => mock.GetAssemblies())
                         .Returns(new List<Assembly>
                                  {
                                      assemblyMock.Object
                                  }.ToArray());

            IUnityContainer container = new UnityContainerBuilder(appDomainMock.Object).Build();

            IList<IContainerRegistration> result = container.Registrations.ToArray();

            AreEqual(2, result.Count);

            IsTrue(IsUnityContainerRegistration(result[0]));
            IsTrue(IsExpectedRegisteredContainer(result[1], type, type, expectedTypeLifetimeMangerType));
        }

        [Test]
        [TestCase(typeof(TypeLifetimeManagerWithoutDefaultCtor))]
        [TestCase(typeof(LifetimeManagerWithoutInterface))]
        public void TestBuild_InvalidTypeLifetimeManagers(Type invalidLifetimeManagerType)
        {
            Mock<Assembly>             assemblyMock  = new Mock<Assembly>();
            Mock<IAppDomainAdapter> appDomainMock = new Mock<IAppDomainAdapter>();

            Type type = new FakeType("Test",
                                     "ClassA",
                                     assemblyMock.Object,
                                     attributes: new RegisterTypeAttribute(null, invalidLifetimeManagerType));

            Type[] typesWithAttribute =
            {
                type
            };

            assemblyMock.Setup(mock => mock.GetTypes())
                        .Returns(typesWithAttribute);

            appDomainMock.Setup(mock => mock.GetAssemblies())
                         .Returns(new List<Assembly>
                                  {
                                      assemblyMock.Object
                                  }.ToArray());

            Throws<InvalidOperationException>(() => new UnityContainerBuilder(appDomainMock.Object).Build());
        }

        [Test]
        public void TestBuild_WithCustomContainer()
        {
            Mock<Assembly>          assemblyMock  = new Mock<Assembly>();
            Mock<IAppDomainAdapter> appDomainMock = new Mock<IAppDomainAdapter>();

            assemblyMock.Setup(mock => mock.GetTypes())
                        .Returns(new Type[0]);
            appDomainMock.Setup(mock => mock.GetAssemblies())
                         .Returns(new List<Assembly>
                                  {
                                      assemblyMock.Object
                                  }.ToArray());

            IUnityContainer container = new UnityContainer();

            IUnityContainer result = new UnityContainerBuilder().Build(container);

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
    }
}
