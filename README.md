[![Build Status](https://travis-ci.org/ManticSic/UnityContainerAttributeRegistration.svg?branch=master)](https://travis-ci.org/ManticSic/UnityContainerAttributeRegistration)

# Unity Container Attribute Registration
This project is inspired by the way Spring Boot allows developers to register services to the di. It provides the `@Service` annotation which marks classes for di.

This way you can immediate see if a type is registered or not. Additionally clears your start up procedure.


## Usage

### Register types

You can register types to an unity container using `UnityContainerAttributeRegistration.RegisterTypeAttribute`.

```cs
using UnityContainerAttributeRegistration;

namespace My.Awesome.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UnityContainerPopulator populator = new UnityContainerPopulator();
            IUnityContainer container = populator.Populate();
        }
    }
    
    [RegisterType]
    public class MyService
    {
    }
}
```

### Register instances

You can register instances to an unity container using `UnityContainerAttributeRegistration.Attribute.RegisterProviderAttribute` and `UnityContainerAttributeRegistration.Attribute.RegisterInstanceAttribute`.

Classes marked with `UnityContainerAttributeRegistration.Attribute.RegisterProviderAttribute` will be instantiated using the container which should be populated with the instances.
So you can use already registered services to create the instances, which should be later registered.

```cs
namespace My.Awesome.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UnityContainerPopulator populator = new UnityContainerPopulator();
            IUnityContainer container = populator.Populate();
        }
    }
    
    [RegisterProvider]
    public class InstanceProvider
    {
        [RegisterInstance]
        public string Token = "Hard coded token";
    }
}
```

### Register Factory

You can register factory methods to an unity container using `UnityContainerAttributeRegistration.Attribute.RegisterProviderAttribute` and `UnityContainerAttributeRegistration.Attribute.RegisterFactoryAttribute`.

Classes marked with `UnityContainerAttributeRegistration.Attribute.RegisterProviderAttribute` will be instantiated using the container which should be populated with the instances.
So you can use already registered services to create the instances, which should be later registered.

Its only important to have the right parameters (see example).

```cs
namespace My.Awesome.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UnityContainerPopulator populator = new UnityContainerPopulator();
            IUnityContainer container = populator.Populate();
        }
    }
    
    [RegisterProvider]
    public class InstanceProvider
    {
        [RegisterFactory]
        public MyClass Factory(IUnityContainer container)
        {
            // do some magic
        }

        [RegisterFactory]
        public MyClass Factory(IUnityContainer container, Type typeValue, string stringvalue)
        {
            // do some magic
        }
    }
}
```

### Using a custom container

It is possible to populate a already created container.

```cs
namespace My.Awesome.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            UnityContainerPopulator populator = new UnityContainerPopulator();
            populator.Populate(container);
        }
    }
}
```

### Restrict the checked assemblies

You can restrict the assemblies to check, e. g. to create a container only containing registrations of one assembly.

```cs
namespace My.Awesome.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IAssemblyProvider assemblyProvider = new CustomAssemblyProvider();
            UnityContainerPopulator populator = new UnityContainerPopulator(assemblyProvider);
            IUnityContainer container = populator.Populate();
        }
    }

    public class CustomAssemblyProvider : IAssemblyProvider
    {
        public IList<Assembly> GetAssemblies()
        {
            // return a list of assemblies to use
        }
    }
}
```

### Register with name

You can use the overloads of `RegisterTypeAttribute`, `RegisterInstanceAttribute` and `RegisterFactoryAttriube` to name the registration.

```cs
namespace My.Awesome.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UnityContainerPopulator populator = new UnityContainerPopulator();
            IUnityContainer container = populator.Populate();

            string token = container.Resolve<string>("token");
            string url   = container.Resolve<string>("url");
        }
    }
    
    [RegisterProvider]
    public class InstanceProvider
    {
        [RegisterInstance("token")]
        public string Token = "Hard coded token";

        [RegisterInstance("url")]
        public string Url = "https://hardcoded.url";
    }
}
```
