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
            IUnityContainer container = UnityContainerBuilder.Build();
        }
    }
    
    [RegisterTypeAttribute]
    public class MyService
    {
    }
}
```

## FooBar

### `RegisterTypeAttribute(from, lifetimeManager)`
#### Description
Mark a type to be registered.
#### Parameters
- `from`
    - type: `System.Type`
    - optional: `true`
    - description: Type which will be requested, when using unity.
- `lifetimeManager`
    - type: `UnityContainerAttributeRegistration.TypeLifetimeManager`
    - optional: `true`
    - description: `ITypeLifetimeManager` which should be used.

### `UnityContainerBuilder.Build()`
#### Description
Search for all types using `RegisterTypeAttribute` and register these types to a new `IUnityContainer`.
#### Return value
- type: `Unity.IUnityContainer`
- description: New `Unity.IUnityContainer` instance, containing the registered types.

### `UnityContainerBuilder.Build(container)`
#### Description
Search for all types using `RegisterTypeAttribute` and register these types to the passed `IUnityContainer`.
#### Parameters
- `container`
    - type: `Unity.IUnityContainer`
    - optional: `false`
    - description: `IUnityContainer` instance to register types
#### Return value
- type: `Unity.IUnityContainer`
- description: The passed `Unity.IUnityContainer`, containing the registered types.
