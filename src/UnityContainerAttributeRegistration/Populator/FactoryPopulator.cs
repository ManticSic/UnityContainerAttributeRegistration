using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using Unity;
using Unity.Lifetime;

using UnityContainerAttributeRegistration.Attribute;


namespace UnityContainerAttributeRegistration.Populator
{
    internal class FactoryPopulator : Populator
    {
        public override IUnityContainer Populate(IUnityContainer container, IList<Type> typesWithAttribute)
        {
            IEnumerable<FactoryToRegister> factoriesToRegister =
                typesWithAttribute.SelectMany(providerClassType => GetInstancesToRegisterFor(container, providerClassType));

            foreach(FactoryToRegister factoryToRegister in factoriesToRegister)
            {
                Type                                        returnType      = factoryToRegister.ReturnType;
                Func<IUnityContainer, Type, string, object> factory         = factoryToRegister.Factory;
                IFactoryLifetimeManager                     lifetimeManager = factoryToRegister.LifetimeManager;

                container.RegisterFactory(returnType, factory, lifetimeManager);
            }

            return container;
        }

        private IEnumerable<FactoryToRegister> GetInstancesToRegisterFor(IUnityContainer container, Type providerClassType)
        {
            object       providerClassInstance = container.Resolve(providerClassType);
            MethodInfo[] methodInfos           = providerClassType.GetMethods();

            return methodInfos
                  .Where(info => info.CustomAttributes.Any(data => data.AttributeType == typeof(RegisterFactoryAttribute)))
                  .Select(info => CreateFactoryToRegisterFrom(info, providerClassInstance))
                  .ToList();
        }

        private FactoryToRegister CreateFactoryToRegisterFrom(MethodInfo info, object instance)
        {
            RegisterFactoryAttribute attribute  = info.GetCustomAttribute<RegisterFactoryAttribute>();
            Type                     returnType = attribute.From ?? info.ReturnType;

            if(returnType == typeof(void))
            {
                throw new InvalidOperationException("Return type must not be void.");
            }

            if(!IsUnityFactorySignature(info))
            {
                throw new InvalidOperationException("Factory method signature does not match.");
            }

            IFactoryLifetimeManager lifetimeManager =
                attribute.LifetimeManager == null
                    ? null
                    : GetInstanceByType<IFactoryLifetimeManager>(attribute.LifetimeManager);

            return new FactoryToRegister(returnType, GetFactoryMethodFor(info, instance), lifetimeManager);
        }

        private bool IsUnityFactorySignature(MethodInfo methodInfo)
        {
            ParameterInfo[] parameters = methodInfo.GetParameters();

            switch(parameters.Length)
            {
                case 1 when parameters[0]
                               .ParameterType == typeof(IUnityContainer):
                case 3 when parameters[0]
                               .ParameterType == typeof(IUnityContainer) && parameters[1]
                               .ParameterType == typeof(Type) && parameters[2]
                               .ParameterType == typeof(string):
                {
                    return true;
                }
                default:
                {
                    return false;
                }
            }
        }

        private Func<IUnityContainer, Type, string, object> GetFactoryMethodFor(MethodInfo methodInfo, object instance)
        {
            return (container, typeValue, stringValue) =>
                   {
                       IList<object> invokeParams = new List<object> {container};

                       if(methodInfo.GetParameters()
                                    .Length == 3)
                       {
                           invokeParams.Add(typeValue);
                           invokeParams.Add(stringValue);
                       }

                       return methodInfo.Invoke(instance, invokeParams.ToArray());
                   };
        }

        private sealed class FactoryToRegister
        {
            public FactoryToRegister([NotNull]   Type                                        returnType,
                                     [NotNull]   Func<IUnityContainer, Type, string, object> factory,
                                     [CanBeNull] IFactoryLifetimeManager                     lifetimeManager)
            {
                ReturnType      = returnType;
                Factory         = factory;
                LifetimeManager = lifetimeManager;
            }

            [NotNull]
            public Type ReturnType { get; }

            [NotNull]
            public Func<IUnityContainer, Type, string, object> Factory { get; }

            [CanBeNull]
            public IFactoryLifetimeManager LifetimeManager { get; }
        }
    }
}
