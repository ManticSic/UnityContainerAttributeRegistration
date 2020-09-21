[![Build Status](https://travis-ci.org/ManticSic/UnityContainerAttributeRegistration.svg?branch=master)](https://travis-ci.org/ManticSic/UnityContainerAttributeRegistration)

# Unity Container Attribute Registration
This project is inspired by the way Spring Boot allows developers to register services to the di. It provides the `@Service` annotation which marks classes for di.

This way you can immediate see if a type is registered or not. Additionally clears your start up procedure.


## Usage

You can register types to an unity container using `UnityContainerAttributeRegistration.RegisterTypeAttribute`.

```cs
using UnityContainerAttributeRegistration;

namespace My.Awesome.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IUnityContainer container = UnityContainerPopulator.Populate();
        }
    }
    
    [RegisterType]
    public class MyService
    {
    }
}
```
