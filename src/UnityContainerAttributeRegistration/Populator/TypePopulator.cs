using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using Unity;
using Unity.Lifetime;

using UnityContainerAttributeRegistration.Adapter;
using UnityContainerAttributeRegistration.Attribute;
using UnityContainerAttributeRegistration.Exention;


namespace UnityContainerAttributeRegistration.Populator
{
    internal class TypePopulator : Populator
    {

        public TypePopulator(IAppDomainAdapter appDomain) : base(appDomain)
        {
        }

        public override IUnityContainer Populate(IUnityContainer container)
        {
            IList<Type> typesWithAttribute = GetTypesWith<RegisterTypeAttribute>(TypeDefined.Inherit)
               .ToList();

            foreach(Type to in typesWithAttribute)
            {
                if(to.IsStatic() || to.IsAbstract || to.IsInterface)
                {
                    throw new InvalidOperationException("TODO throw better exception");
                }

                RegisterTypeAttribute attribute = to.GetCustomAttribute<RegisterTypeAttribute>();
                ITypeLifetimeManager lifetimeManager = attribute.LifetimeManager == null
                                                           ? null
                                                           : GetInstanceByType<ITypeLifetimeManager>(attribute.LifetimeManager);

                container.RegisterType(attribute.From, to, lifetimeManager);
            }

            return container;
        }

        [CanBeNull]
        private T GetInstanceByType<T>([CanBeNull] Type objectType)
        {
            Type targetType = typeof(T);

            if(objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }

            if(!targetType.IsAssignableFrom(objectType))
            {
                throw new InvalidOperationException(
                    $"Type {objectType.FullName} cannot be assigned from {targetType.FullName}");
            }

            ConstructorInfo ctor = objectType.GetConstructor(Type.EmptyTypes);

            if(ctor == null)
            {
                throw new InvalidOperationException(
                    $"Cannot create instance of ITypeLifetimeManager. No default constructor found for {objectType.FullName}");
            }

            return (T) ctor.Invoke(new object[0]);
        }
    }
}
