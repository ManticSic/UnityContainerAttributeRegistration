using System;

using Unity;
using Unity.Lifetime;

using UnityContainerAttributeRegistration.Attribute;


namespace UnityContainerAttributeRegistrationTest.Assets.RegisterFactoryTestClasses
{
    [RegisterProvider]
    public class ProviderWithOneArgument
    {
        [RegisterFactory]
        public MyClass Factory(IUnityContainer container)
        {
            return new MyClass();
        }
    }

    [RegisterProvider]
    public class ProviderWithThreeArguments
    {
        [RegisterFactory]
        public MyClass Factory(IUnityContainer container, Type typeValue, string stringValue)
        {
            return new MyClass();
        }
    }

    [RegisterProvider]
    public class ProviderUsingInterface
    {
        [RegisterFactory(typeof(IMyInterface))]
        public MyClass Factory(IUnityContainer container)
        {
            return new MyClass();
        }
    }

    [RegisterProvider]
    public class ProviderWithHierarchicalLifetimeManager
    {
        [RegisterFactory(null, typeof(HierarchicalLifetimeManager))]
        public MyClass Factory(IUnityContainer container)
        {
            return new MyClass();
        }
    }

    [RegisterProvider]
    public class ProviderWithSingletonLifetimeManager
    {
        [RegisterFactory(null, typeof(SingletonLifetimeManager))]
        public MyClass Factory(IUnityContainer container)
        {
            return new MyClass();
        }
    }

    [RegisterProvider]
    public class ProviderWithTransientLifetimeManager
    {
        [RegisterFactory(null, typeof(TransientLifetimeManager))]
        public MyClass Factory(IUnityContainer container)
        {
            return new MyClass();
        }
    }

    [RegisterProvider]
    public class ProviderWithContainerControlledLifetimeManager
    {
        [RegisterFactory(null, typeof(ContainerControlledLifetimeManager))]
        public MyClass Factory(IUnityContainer container)
        {
            return new MyClass();
        }
    }

    [RegisterProvider]
    public class ProviderWithContainerControlledTransientManager
    {
        [RegisterFactory(null, typeof(ContainerControlledTransientManager))]
        public MyClass Factory(IUnityContainer container)
        {
            return new MyClass();
        }
    }

    [RegisterProvider]
    public class ProviderWithExternallyControlledLifetimeManager
    {
        [RegisterFactory(null, typeof(ExternallyControlledLifetimeManager))]
        public MyClass Factory(IUnityContainer container)
        {
            return new MyClass();
        }
    }

    [RegisterProvider]
    public class ProviderWithPerResolveLifetimeManager
    {
        [RegisterFactory(null, typeof(PerResolveLifetimeManager))]
        public MyClass Factory(IUnityContainer container)
        {
            return new MyClass();
        }
    }

    [RegisterProvider]
    public class ProviderWithPerThreadLifetimeManager
    {
        [RegisterFactory(null, typeof(PerThreadLifetimeManager))]
        public MyClass Factory(IUnityContainer container)
        {
            return new MyClass();
        }
    }

    [RegisterProvider]
    public class ProviderWithInvalidReturnValueWithOneArgument
    {
        [RegisterFactory]
        public void Factory(IUnityContainer container)
        {
        }
    }

    [RegisterProvider]
    public class ProviderWithInvalidReturnValueWithThreeArguments
    {
        [RegisterFactory]
        public void Factory(IUnityContainer container, Type typeValue, string stringValue)
        {
        }
    }

    [RegisterProvider]
    public class ProviderWithInvalidArguments1
    {
        [RegisterFactory]
        public MyClass Factory(IUnityContainer container, string stringValue)
        {
            return new MyClass();
        }
    }

    [RegisterProvider]
    public class ProviderWithInvalidArguments2
    {
        [RegisterFactory]
        public MyClass Factory(IUnityContainer container, Type typeValue)
        {
            return new MyClass();
        }
    }

    [RegisterProvider]
    public class ProviderWithInvalidArguments3
    {
        [RegisterFactory]
        public MyClass Factory(IUnityContainer container, string stringValue, Type typeValue)
        {
            return new MyClass();
        }
    }

    [RegisterProvider]
    public class ProviderWithInvalidArguments4
    {
        [RegisterFactory]
        public MyClass Factory(IUnityContainer container, Type typeValue, string stringValue, object objectValue)
        {
            return new MyClass();
        }
    }

    [RegisterProvider]
    public class ProviderWithInvalidArguments5
    {
        [RegisterFactory]
        public MyClass Factory(object objectValue)
        {
            return new MyClass();
        }
    }

    [RegisterProvider]
    public static class StaticProvider
    {
    }

    public interface IMyInterface
    {
    }

    public class MyClass : IMyInterface
    {
    }
}
