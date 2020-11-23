using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Unity;
using Unity.Lifetime;

using UnityContainerAttributeRegistration;

using UnityContainerAttributeRegistrationTest.Assets.RegistertInstanceTestClasses;
using UnityContainerAttributeRegistrationTest.Helper;

using static NUnit.Framework.Assert;


namespace UnityContainerAttributeRegistrationTest.Attribute
{
    public class RegisterInstanceAttributeTest : TestBase
    {
        [Test]
        [TestCase(typeof(DefaultProvider), typeof(AnyClass), typeof(AnyClass), typeof(ContainerControlledLifetimeManager))]
        [TestCase(typeof(ProviderUsingFromWithoutLifetimeManager), typeof(IAnyInterface), typeof(AnyClass),
                  typeof(ContainerControlledLifetimeManager))]
        [TestCase(typeof(ProviderUsingFromWithSingletonLifetimeManager), typeof(IAnyInterface), typeof(AnyClass),
                  typeof(SingletonLifetimeManager))]
        [TestCase(typeof(ProviderUsingFromWithContainerControlledLifetimeManager), typeof(IAnyInterface), typeof(AnyClass),
                  typeof(ContainerControlledLifetimeManager))]
        [TestCase(typeof(ProviderUsingFromWithExternallyControlledLifetimeManager), typeof(IAnyInterface), typeof(AnyClass),
                  typeof(ExternallyControlledLifetimeManager))]
        [TestCase(typeof(ProviderWithExternallyControlledLifetimeManager), typeof(AnyClass), typeof(AnyClass),
                  typeof(ExternallyControlledLifetimeManager))]
        public void TestPopulate(Type providerType, Type expectedFrom, Type expectedTo, Type expectedInstanceLifetimeManagerType)
        {
            Scope scope = new Scope();

            scope.AddType(providerType);

            IUnityContainer container = new UnityContainerPopulator(scope.GetAppDomain()).Populate();

            IList<IContainerRegistration> result = container.Registrations.ToArray();

            AreEqual(2, result.Count);
            IsUnityContainerRegistration(result[0]);
            IsExpectedRegisteredContainer(result[1], expectedFrom, expectedTo, expectedInstanceLifetimeManagerType);
        }

        [Test]
        [TestCase(typeof(StaticClassWithAttribute))]
        [TestCase(typeof(AbstractClassWithAttribute))]
        [TestCase(typeof(ProviderWithLifetimemanagerWithoutInterface))]
        public void TestPopulate_InvalidUsage(Type providerType)
        {
            Scope scope = new Scope();

            scope.AddType(providerType);

            Throws<InvalidOperationException>(() => new UnityContainerPopulator(scope.GetAppDomain()).Populate());
        }

        [Test]
        public void TestPopulate_WithName()
        {
            Scope scope = new Scope();

            scope.AddType(typeof(ProviderWithName));

            IUnityContainer container = new UnityContainerPopulator(scope.GetAppDomain()).Populate();

            string val1 = container.Resolve<string>("val1");
            string val2 = container.Resolve<string>("val2");

            AreEqual("Foo", val1);
            AreEqual("Bar", val2);
        }

        [Test]
        public void TestPopulate_WithCustomContainer()
        {
            Scope scope = new Scope();

            IUnityContainer container = new UnityContainer();

            IUnityContainer result = new UnityContainerPopulator(scope.GetAppDomain()).Populate(container);

            AreSame(container, result);
        }
    }
}
