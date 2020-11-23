using System;
using Unity.Lifetime;
using UnityContainerAttributeRegistration.Attribute;

namespace UnityContainerAttributeRegistrationTest.Assets.RegisterTypeTestClasses
{
    [RegisterType]
    public class Default
    {
    }

    [RegisterType(typeof(IAnyInterface))]
    public class ClassImplementsInterfaceWithoutLifetimeManager : IAnyInterface
    {
    }

    [RegisterType(typeof(AnyAbstractClass))]
    public class ClassInheritAbstractWithoutLifetimeManager : AnyAbstractClass
    {
    }

    [RegisterType(typeof(AnyClass))]
    public class ClassInheritClassWithoutLifetimeManager : AnyClass
    {
    }

    [RegisterType(typeof(IAnyInterface), typeof(HierarchicalLifetimeManager))]
    public class ClassImplementsInterfaceWithHierarchicalLifetimeManager
    {
    }

    [RegisterType(typeof(IAnyInterface), typeof(SingletonLifetimeManager))]
    public class ClassImplementsInterfaceWithSingletonLifetimeManager
    {
    }

    [RegisterType(typeof(IAnyInterface), typeof(TransientLifetimeManager))]
    public class ClassImplementsInterfaceWithTransientLifetimeManager
    {
    }

    [RegisterType(typeof(IAnyInterface), typeof(ContainerControlledLifetimeManager))]
    public class ClassImplementsInterfaceWithContainerControlledLifetimeManager
    {
    }

    [RegisterType(typeof(IAnyInterface), typeof(ContainerControlledTransientManager))]
    public class ClassImplementsInterfaceWithContainerControlledTransientManager
    {
    }

    [RegisterType(typeof(IAnyInterface), typeof(ExternallyControlledLifetimeManager))]
    public class ClassImplementsInterfaceWithExternallyControlledLifetimeManager
    {
    }

    [RegisterType(typeof(IAnyInterface), typeof(PerResolveLifetimeManager))]
    public class ClassImplementsInterfaceWithPerResolveLifetimeManager
    {
    }

    [RegisterType(typeof(IAnyInterface), typeof(PerThreadLifetimeManager))]
    public class ClassImplementsInterfaceWithPerThreadLifetimeManager
    {
    }

    [RegisterType(null, typeof(TransientLifetimeManager))]
    public class ClassWithLifetimeManager
    {
    }

    [RegisterType(null, typeof(LifetimeManagerWithoutInterface))]
    public class ClassWithLifetimeManagerWithoutInterface
    {
    }

    [RegisterType(null, typeof(TypeLifetimeManagerWithoutDefaultCtor))]
    public class ClassWithLifetimeManagerWithoutDefaultCtor
    {
    }

    [RegisterType(null, typeof(TransientLifetimeManager))]
    public static class StaticClassWithAttribute
    {
    }

    [RegisterType(null, typeof(TransientLifetimeManager))]
    public abstract class AbstractClassWithAttribute
    {
    }

    internal class TypeLifetimeManagerWithoutDefaultCtor : ITypeLifetimeManager
    {
        private readonly string anyValue;

        public TypeLifetimeManagerWithoutDefaultCtor(string anyValue)
        {
            this.anyValue = anyValue;
        }

        public LifetimeManager CreateLifetimePolicy()
        {
            throw new NotImplementedException();
        }
    }

    public interface IAnyInterface
    {
    }

    public abstract class AnyAbstractClass
    {
    }

    public class AnyClass
    {
    }

    public class LifetimeManagerWithoutInterface
    {
    }
}
