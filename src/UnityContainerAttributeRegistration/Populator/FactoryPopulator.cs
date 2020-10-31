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
    /// <summary>
    ///     Populator for the <see cref="UnityContainerAttributeRegistration.Attribute.RegisterFactoryAttribute" />.
    /// </summary>
    internal class FactoryPopulator : Populator
    {
        /// <inheritdoc cref="Populator.Populate" />
        /// <exception cref="InvalidOperationException">Invalid factory method.</exception>
        public override IUnityContainer Populate(IUnityContainer container, IList<Type> typesWithAttribute)
        {
            IEnumerable<FactoryToRegister> factoriesToRegister =
                typesWithAttribute.SelectMany(providerClassType => GetFactoryToRegisterFor(container, providerClassType));

            foreach(FactoryToRegister factoryToRegister in factoriesToRegister)
            {
                Type                                        returnType      = factoryToRegister.ReturnType;
                Func<IUnityContainer, Type, string, object> factory         = factoryToRegister.Factory;
                IFactoryLifetimeManager                     lifetimeManager = factoryToRegister.LifetimeManager;

                container.RegisterFactory(returnType, factory, lifetimeManager);
            }

            return container;
        }

        /// <summary>
        ///     Get all candidates to register.
        /// </summary>
        /// <param name="container"><see cref="IUnityContainer" /> to instantiate the provider class</param>
        /// <param name="providerClassType">Factory provider class</param>
        /// <returns>List of candidates to register</returns>
        /// <exception cref="InvalidOperationException">Invalid factory method.</exception>
        private IEnumerable<FactoryToRegister> GetFactoryToRegisterFor(IUnityContainer container, Type providerClassType)
        {
            object       providerClassInstance = container.Resolve(providerClassType);
            MethodInfo[] methodInfos           = providerClassType.GetMethods();

            return methodInfos
                  .Where(info => info.CustomAttributes.Any(data => data.AttributeType == typeof(RegisterFactoryAttribute)))
                  .Select(info => CreateFactoryToRegisterFrom(info, providerClassInstance))
                  .ToList();
        }

        /// <summary>
        ///     Transform a <see cref="MethodInfo" /> and an instance to a <see cref="FactoryToRegister" /> object.
        /// </summary>
        /// <param name="info"><see cref="MethodInfo" /> of the factory method.</param>
        /// <param name="instance">Instance of the factory method.</param>
        /// <returns>Wrapper for easy registration.</returns>
        /// <exception cref="InvalidOperationException">Invalid factory method.</exception>
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

        /// <summary>
        ///     Verify that the parameters matches the requirements of Unity
        /// </summary>
        /// <param name="methodInfo"><see cref="MethodInfo" /> to check.</param>
        /// <returns>Whether <paramref name="methodInfo" /> matches the requirements or not.</returns>
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

        /// <summary>
        ///     Create a wrapper function for a factory method.
        /// </summary>
        /// <param name="methodInfo"><see cref="MethodInfo" /> of the factory method.</param>
        /// <param name="instance">Instance of the factory method.</param>
        /// <returns><see cref="Func{TResult}" /> to call the factory method.</returns>
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

        /// <summary>
        ///     Wrapper to register factories
        /// </summary>
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

            /// <summary>
            ///     Requested type as return type of the factory
            /// </summary>
            [NotNull]
            public Type ReturnType { get; }

            /// <summary>
            ///     Factory method
            /// </summary>
            [NotNull]
            public Func<IUnityContainer, Type, string, object> Factory { get; }

            /// <summary>
            ///     Used lifetime manager
            /// </summary>
            [CanBeNull]
            public IFactoryLifetimeManager LifetimeManager { get; }
        }
    }
}
