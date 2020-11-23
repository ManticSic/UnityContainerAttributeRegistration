using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity;
using Unity.Lifetime;
using UnityContainerAttributeRegistration;
using UnityContainerAttributeRegistrationTest.Assets.RegisterFactoryTestClasses;
using UnityContainerAttributeRegistrationTest.Helper;
using static NUnit.Framework.Assert;


namespace UnityContainerAttributeRegistrationTest.Attribute
{
    public class RegisterFactoryAttributeTest : TestBase
    {
        [Test]
        [TestCase(typeof(ProviderWithOneArgument), typeof(MyClass), typeof(MyClass), typeof(TransientLifetimeManager))]
        [TestCase(typeof(ProviderWithThreeArguments), typeof(MyClass), typeof(MyClass), typeof(TransientLifetimeManager))]
        [TestCase(typeof(ProviderUsingInterface), typeof(IMyInterface), typeof(IMyInterface), typeof(TransientLifetimeManager))]
        [TestCase(typeof(ProviderWithHierarchicalLifetimeManager), typeof(MyClass), typeof(MyClass),
                  typeof(HierarchicalLifetimeManager))]
        [TestCase(typeof(ProviderWithSingletonLifetimeManager), typeof(MyClass), typeof(MyClass),
                  typeof(SingletonLifetimeManager))]
        [TestCase(typeof(ProviderWithTransientLifetimeManager), typeof(MyClass), typeof(MyClass),
                  typeof(TransientLifetimeManager))]
        [TestCase(typeof(ProviderWithContainerControlledLifetimeManager), typeof(MyClass), typeof(MyClass),
                  typeof(ContainerControlledLifetimeManager))]
        [TestCase(typeof(ProviderWithContainerControlledTransientManager), typeof(MyClass), typeof(MyClass),
                  typeof(ContainerControlledTransientManager))]
        [TestCase(typeof(ProviderWithExternallyControlledLifetimeManager), typeof(MyClass), typeof(MyClass),
                  typeof(ExternallyControlledLifetimeManager))]
        [TestCase(typeof(ProviderWithPerResolveLifetimeManager), typeof(MyClass), typeof(MyClass),
                  typeof(PerResolveLifetimeManager))]
        [TestCase(typeof(ProviderWithPerThreadLifetimeManager), typeof(MyClass), typeof(MyClass),
                  typeof(PerThreadLifetimeManager))]
        public void TestPopulate(Type providerType, Type expectedFrom, Type expectedTo, Type expectedFactoryLifetimeManagerType)
        {
            Scope scope = new Scope();

            scope.AddType(providerType);

            IUnityContainer container = new UnityContainerPopulator(scope.GetAppDomain()).Populate();

            IList<IContainerRegistration> result = container.Registrations.ToList();

            AreEqual(2, result.Count);
            IsUnityContainerRegistration(result[0]);
            IsExpectedRegisteredContainer(result[1], expectedFrom, expectedTo, expectedFactoryLifetimeManagerType);

            object obj = container.Resolve(expectedTo);
            NotNull(obj);
        }

        [Test]
        [TestCase(typeof(ProviderWithInvalidReturnValueWithOneArgument))]
        [TestCase(typeof(ProviderWithInvalidReturnValueWithThreeArguments))]
        [TestCase(typeof(ProviderWithInvalidArguments1))]
        [TestCase(typeof(ProviderWithInvalidArguments2))]
        [TestCase(typeof(ProviderWithInvalidArguments3))]
        [TestCase(typeof(ProviderWithInvalidArguments4))]
        [TestCase(typeof(ProviderWithInvalidArguments5))]
        [TestCase(typeof(StaticProvider))]
        public void TestPopulate_InvalidUsage(Type providerType)
        {
            Scope scope = new Scope();

            scope.AddType(providerType);

            Throws<InvalidOperationException>(() => new UnityContainerPopulator(scope.GetAppDomain()).Populate());
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
