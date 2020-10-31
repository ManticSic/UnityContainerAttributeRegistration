using Unity.Lifetime;

using UnityContainerAttributeRegistration.Attribute;


namespace UnityContainerAttributeRegistrationTest.Assets.RegistertInstanceTestClasses
{
    [RegisterProvider]
    public class DefaultProvider
    {
        [RegisterInstance]
        public AnyClass Value
        {
            get => new AnyClass();
        }
    }

    [RegisterProvider]
    public class ProviderUsingFromWithoutLifetimeManager
    {
        [RegisterInstance(typeof(IAnyInterface))]
        public AnyClass Value
        {
            get => new AnyClass();
        }
    }

    [RegisterProvider]
    public class ProviderUsingFromWithSingletonLifetimeManager
    {
        [RegisterInstance(typeof(IAnyInterface), typeof(SingletonLifetimeManager))]
        public AnyClass Value
        {
            get => new AnyClass();
        }
    }

    [RegisterProvider]
    public class ProviderUsingFromWithContainerControlledLifetimeManager
    {
        [RegisterInstance(typeof(IAnyInterface), typeof(ContainerControlledLifetimeManager))]
        public AnyClass Value
        {
            get => new AnyClass();
        }
    }

    [RegisterProvider]
    public class ProviderUsingFromWithExternallyControlledLifetimeManager
    {
        [RegisterInstance(typeof(IAnyInterface), typeof(ExternallyControlledLifetimeManager))]
        public AnyClass Value
        {
            get => new AnyClass();
        }
    }

    [RegisterProvider]
    public class ProviderWithExternallyControlledLifetimeManager
    {
        [RegisterInstance(null, typeof(ExternallyControlledLifetimeManager))]
        public AnyClass Value
        {
            get => new AnyClass();
        }
    }

    [RegisterProvider]
    public class ProviderWithLifetimemanagerWithoutInterface
    {
        [RegisterInstance(null, typeof(LifetimeManagerWithoutInterface))]
        public AnyClass Value
        {
            get => new AnyClass();
        }
    }

    [RegisterProvider]
    public static class StaticClassWithAttribute
    {
    }

    [RegisterProvider]
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
