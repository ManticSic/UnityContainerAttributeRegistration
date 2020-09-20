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
        public void TestBuild_Default()
        {
            Mock<Assembly>          assemblyMock  = new Mock<Assembly>();
            Mock<IAppDomainAdapter> appDomainMock = new Mock<IAppDomainAdapter>();

            Type type = new FakeType("Test",
                                     "ClassA",
                                     assemblyMock.Object,
                                     attributes: new RegisterTypeAttribute());

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
            IsTrue(IsExpectedRegisteredContainer(result[1], type, type, typeof(TransientLifetimeManager)));
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
