# Changelog

## 0.3.0 (2020-11-18)
### Code
- added: Attribute to register a factory
- changed: Rename `RegisterInstanceProvider` to `RegisterProvider`
- changed: Improve performance by reducing iterations

### Project

## 0.2.0 (2020-10-24)
### Code
- added: Attribute to register concrete instances
- changed: Rename `AppDomainAdapter` to `AssemblyProvider`

### Project
- changed: Library is now compiled as .Net Standard 2.0, 2.1 and .Net Core 2.0 and compatible with 
    - .Net Core 2.0, 3.0
    - .Net Framework 4.6.1
    - Mono 5.4, 6.4
    - Xamarin.iOS 10.14, 12.16
    - Xamarin.Max 3.8, 5.16
    - Xamarin.Android 8.0, 10.0
    - UWP 10.0.16299
    - Unity 2018.1
 - changed: XML Doc is now only requiered for release builds
 - changed: extend docs
 - fixed: Readme with old docs

## 0.1.3 (2020-09-24)
### Code
- added: XMLDocumentation for all `public`, `internal` and `protected` types, fields, properties and methods
- changed: improve testing

### CI / CD
- added: XMLDocumentation to nuget package

## 0.1.2 (2020-09-20)
### Code
- added: XMLDocumentation for `public` types
- changed: name of `UnityContainerBuilder` to `UnityContainerPopulator`
    - changed: `UnityContainerPopulator` is no longer static
    - changed: provide AppDomain by wrapper class
- changed: constructor of `TypeRegisterAttribute`
    - added: accepting type implementing `ITypeLifetimeManager`
    - removed: usage of `TypeLifetimeManager` enum
- changed: namespace of several public and internal classes
- changed: improve testing

### Project
- changed: readme
- changed: changelog

### CI / CD
- add changelog and readme to release

## 0.1.1 (2020-09-20)
### Project
- changed: readme

### CI / CD
- changed: nuget package meta data
- changed: package is no longer a development dependency


## 0.1.0 (2020-09-20)
### Code
- changed: default used `ITypeLifetimeManager`

### Project
- added: code of conduct
- changed: readme

## CI / CD
- removed: deployment to github packages

## 0.1.0-alpha (2020-09-20)
### Code
- added: `RegisterTypeAttribute`
- added: `UnityContainerBuilder`
- added: tests for `RegisterTypeAttribute`

### Project
- added: license
- added: code guidelines / code style

## CI / CD
- added: Travis CI
    - run build
    - run tests
    - deploy to nuget.org
    - deploy to github packages
- added: SonarCloud
