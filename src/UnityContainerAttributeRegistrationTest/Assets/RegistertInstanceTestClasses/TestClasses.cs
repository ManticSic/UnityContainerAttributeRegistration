using Unity.Lifetime;

using UnityContainerAttributeRegistration.Attribute;


namespace UnityContainerAttributeRegistrationTest.Assets.RegistertInstanceTestClasses
{
    [RegisterInstanceProvider]
    public class DefaultProvider
    {
        [RegisterInstance]
        public AnyClass Value
        {
            get => new AnyClass();
        }
    }

    [RegisterInstanceProvider]
    public class ProviderUsingFromWithoutLifetimeManager
    {
        [RegisterInstance(typeof(IAnyInterface))]
        public AnyClass Value
        {
            get => new AnyClass();
        }
    }

    [RegisterInstanceProvider]
    public class ProviderUsingFromWithSingletonLifetimeManager
    {
        [RegisterInstance(typeof(IAnyInterface), typeof(SingletonLifetimeManager))]
        public AnyClass Value
        {
            get => new AnyClass();
        }
    }

    [RegisterInstanceProvider]
    public class ProviderUsingFromWithContainerControlledLifetimeManager
    {
        [RegisterInstance(typeof(IAnyInterface), typeof(ContainerControlledLifetimeManager))]
        public AnyClass Value
        {
            get => new AnyClass();
        }
    }

    [RegisterInstanceProvider]
    public class ProviderUsingFromWithExternallyControlledLifetimeManager
    {
        [RegisterInstance(typeof(IAnyInterface), typeof(ExternallyControlledLifetimeManager))]
        public AnyClass Value
        {
            get => new AnyClass();
        }
    }

    [RegisterInstanceProvider]
    public class ProviderWithExternallyControlledLifetimeManager
    {
        [RegisterInstance(null, typeof(ExternallyControlledLifetimeManager))]
        public AnyClass Value
        {
            get => new AnyClass();
        }
    }

    [RegisterInstanceProvider]
    public class ProviderWithLifetimemanagerWithoutInterface
    {
        [RegisterInstance(null, typeof(LifetimeManagerWithoutInterface))]
        public AnyClass Value
        {
            get => new AnyClass();
        }
    }

    [RegisterInstanceProvider]
    public static class StaticClassWithAttribute
    {
    }

    [RegisterInstanceProvider]
    public abstract class AbstractClassWithAttribute
    {
    }

    public class AnyClass : IAnyInterface
    {
    }

    public interface IAnyInterface
    {
    }

    public class LifetimeManagerWithoutInterface
    {
    }
}
