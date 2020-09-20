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

using static NUnit.Framework.Assert;


namespace UnityContainerAttributeRegistrationTest
{
    public class RegisterResolverTest
    {
        [Test]
        [TestCase(TypeLifetimeManager.Default, typeof(TransientLifetimeManager))]
        [TestCase(TypeLifetimeManager.HierarchicalLifetimeManager, typeof(HierarchicalLifetimeManager))]
        [TestCase(TypeLifetimeManager.SingletonLifetimeManager, typeof(SingletonLifetimeManager))]
        [TestCase(TypeLifetimeManager.TransientLifetimeManager, typeof(TransientLifetimeManager))]
        [TestCase(TypeLifetimeManager.ContainerControlledLifetimeManager, typeof(ContainerControlledLifetimeManager))]
        [TestCase(TypeLifetimeManager.ContainerControlledTransientManager, typeof(ContainerControlledTransientManager))]
        [TestCase(TypeLifetimeManager.ExternallyControlledLifetimeManager, typeof(ExternallyControlledLifetimeManager))]
        [TestCase(TypeLifetimeManager.PerResolveLifetimeManager, typeof(PerResolveLifetimeManager))]
        [TestCase(TypeLifetimeManager.PerThreadLifetimeManager, typeof(PerThreadLifetimeManager))]
        public void TestBuild_TypeLifetimeManagers(TypeLifetimeManager lifetimeManager, Type expectedTypeLifetimeManger)
        {
            Mock<Assembly>          assemblyMock  = new Mock<Assembly>();
            Mock<IAppDomainAdapter> appDomainMock = new Mock<IAppDomainAdapter>();

            Type type = new FakeType("Test",
                                     "ClassA",
                                     assemblyMock.Object,
                                     attributes: new RegisterTypeAttribute(null, lifetimeManager));

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
            IsTrue(IsExpectedRegisteredContainer(result[1], type, type, expectedTypeLifetimeManger));
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
