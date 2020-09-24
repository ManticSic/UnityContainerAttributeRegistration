# Changelog

## 0.1.3
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
### Code:
- changed: default used `ITypeLifetimeManager`

### Project
- added: code of conduct
- changed: readme

## CI / CD
- removed: deployment to github packages

## 0.1.0-alpha (2020-09-20)
### Code:
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
