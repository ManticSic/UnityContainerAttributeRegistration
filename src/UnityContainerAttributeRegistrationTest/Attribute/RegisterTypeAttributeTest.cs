using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity;
using Unity.Lifetime;
using UnityContainerAttributeRegistration;
using UnityContainerAttributeRegistrationTest.Assets.RegisterTypeTestClasses;
using UnityContainerAttributeRegistrationTest.Helper;
using static NUnit.Framework.Assert;


namespace UnityContainerAttributeRegistrationTest.Attribute
{
    internal class RegisterTypeAttributeTest : TestBase
    {
        [Test]
        [TestCase(typeof(Default),
                  typeof(Default),
                  typeof(TransientLifetimeManager))]
        [TestCase(typeof(ClassImplementsInterfaceWithoutLifetimeManager),
                  typeof(IAnyInterface),
                  typeof(TransientLifetimeManager))]
        [TestCase(typeof(ClassInheritAbstractWithoutLifetimeManager),
                  typeof(AnyAbstractClass),
                  typeof(TransientLifetimeManager))]
        [TestCase(typeof(ClassInheritClassWithoutLifetimeManager),
                  typeof(AnyClass),
                  typeof(TransientLifetimeManager))]
        [TestCase(typeof(ClassImplementsInterfaceWithHierarchicalLifetimeManager),
                  typeof(IAnyInterface),
                  typeof(HierarchicalLifetimeManager))]
        [TestCase(typeof(ClassImplementsInterfaceWithSingletonLifetimeManager),
                  typeof(IAnyInterface),
                  typeof(SingletonLifetimeManager))]
        [TestCase(typeof(ClassImplementsInterfaceWithTransientLifetimeManager),
                  typeof(IAnyInterface),
                  typeof(TransientLifetimeManager))]
        [TestCase(typeof(ClassImplementsInterfaceWithContainerControlledLifetimeManager),
                  typeof(IAnyInterface),
                  typeof(ContainerControlledLifetimeManager))]
        [TestCase(typeof(ClassImplementsInterfaceWithContainerControlledTransientManager),
                  typeof(IAnyInterface),
                  typeof(ContainerControlledTransientManager))]
        [TestCase(typeof(ClassImplementsInterfaceWithExternallyControlledLifetimeManager),
                  typeof(IAnyInterface),
                  typeof(ExternallyControlledLifetimeManager))]
        [TestCase(typeof(ClassImplementsInterfaceWithPerResolveLifetimeManager),
                  typeof(IAnyInterface),
                  typeof(PerResolveLifetimeManager))]
        [TestCase(typeof(ClassImplementsInterfaceWithPerThreadLifetimeManager),
                  typeof(IAnyInterface),
                  typeof(PerThreadLifetimeManager))]
        [TestCase(typeof(ClassWithLifetimeManager),
                  typeof(ClassWithLifetimeManager),
                  typeof(TransientLifetimeManager))]
        public void TestPopulate(Type to, Type expectedFrom, Type expectedTypeLifetimeMangerType)
        {
            Scope scope = new Scope();

            scope.AddType(to);

            IUnityContainer container = new UnityContainerPopulator(scope.GetAppDomain()).Populate();

            IList<IContainerRegistration> result = container.Registrations.ToArray();

            AreEqual(2, result.Count);

            IsUnityContainerRegistration(result[0]);
            IsExpectedRegisteredContainer(result[1], expectedFrom, to, expectedTypeLifetimeMangerType);
        }

        [Test]
        [TestCase(typeof(ClassWithLifetimeManagerWithoutInterface))]
        [TestCase(typeof(ClassWithLifetimeManagerWithoutDefaultCtor))]
        [TestCase(typeof(StaticClassWithAttribute))]
        [TestCase(typeof(AbstractClassWithAttribute))]
        public void TestPopulate_InvalidUsage(Type to)
        {
            Scope scope = new Scope();

            scope.AddType(to);

            Throws<InvalidOperationException>(() => new UnityContainerPopulator(scope.GetAppDomain()).Populate());
        }

        [Test]
        public void TestPopulate_WithName()
        {
            Scope scope = new Scope();

            scope.AddType(typeof(ClassWithName));

            IUnityContainer container = new UnityContainerPopulator(scope.GetAppDomain()).Populate();

            ClassWithName result = container.Resolve<ClassWithName>("my-type");

            IsNotNull(result);
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
