using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Unity;

using UnityContainerAttributeRegistration.Attribute;
using UnityContainerAttributeRegistration.Exention;
using UnityContainerAttributeRegistration.Populator;
using UnityContainerAttributeRegistration.Provider;


namespace UnityContainerAttributeRegistration
{
    /// <summary>
    ///     Creates a populated or populates an <see cref="Unity.IUnityContainer" />, depending on the provided assemblies
    /// </summary>
    public sealed class UnityContainerPopulator
    {
        [NotNull]
        private readonly IAssemblyProvider appDomain;

        [NotNull]
        private readonly IPopulator        typePopulator;

        [NotNull]
        private readonly IPopulator        instancePopulator;

        [NotNull]
        private readonly IPopulator        factoryPopulator;

        /// <summary>
        ///     Use <see cref="System.AppDomain.CurrentDomain" /> to populate an <see cref="Unity.IUnityContainer" />
        /// </summary>
        public UnityContainerPopulator() : this(new AssemblyProvider())
        {
        }

        /// <summary>
        ///     Use <paramref name="appDomain" /> to populate an <see cref="Unity.IUnityContainer" />
        /// </summary>
        /// <param name="appDomain">Custom <see cref="IAssemblyProvider" /></param>
        public UnityContainerPopulator([NotNull] IAssemblyProvider appDomain)
        {
            this.appDomain    = appDomain;
            typePopulator     = new TypePopulator();
            instancePopulator = new InstancePopulator();
            factoryPopulator  = new FactoryPopulator();
        }

        /// <summary>
        ///     Create and populate a new <see cref="Unity.UnityContainer" />
        /// </summary>
        /// <returns>New <see cref="Unity.UnityContainer" /> with registered types</returns>
        public IUnityContainer Populate()
        {
            return Populate(new UnityContainer());
        }

        /// <summary>
        ///     Populate <paramref name="container" />
        /// </summary>
        /// <param name="container"><see cref="Unity.IUnityContainer" /> to register types</param>
        /// <returns>
        ///     <paramref name="container" />
        /// </returns>
        public IUnityContainer Populate([NotNull] IUnityContainer container)
        {
            IDictionary<Type, IList<Type>> annotatedTypes = FindAnnotatedTypes(TypeDefined.Inherit);

            typePopulator.Populate(container, annotatedTypes[typeof(RegisterTypeAttribute)]);
            instancePopulator.Populate(container, annotatedTypes[typeof(RegisterProviderAttribute)]);
            factoryPopulator.Populate(container, annotatedTypes[typeof(RegisterProviderAttribute)]);

            return container;
        }

        private IDictionary<Type, IList<Type>> FindAnnotatedTypes(TypeDefined typeDefined)
        {
            IEnumerable<Type> types = appDomain.GetAssemblies()
                                               .SelectMany(assembly => assembly.GetTypes());

            IDictionary<Type, IList<Type>> typesPerAttribute = new Dictionary<Type, IList<Type>>()
                                                   {
                                                       {typeof(RegisterTypeAttribute), new List<Type>()},
                                                       {typeof(RegisterProviderAttribute), new List<Type>()}
                                                   };

            foreach(Type classType in types)
            {
                // todo avoid nested loop
#pragma warning disable AV1532
                foreach(KeyValuePair<Type, IList<Type>> kv in typesPerAttribute)
#pragma warning restore AV1532
                {
                    Type        attributeType      = kv.Key;
                    IList<Type> typesWithAttribute = kv.Value;

                    if (classType.IsDefined(attributeType, typeDefined == TypeDefined.Inherit))
                    {
                        if(classType.IsStatic() || classType.IsAbstract)
                        {
                            throw new InvalidOperationException(
                                $"Class type must not be static or abstract to be used with RegisterTypeAttribute: {classType.FullName}");
                        }

                        typesWithAttribute.Add(classType);
                    }
                }
            }

            return typesPerAttribute;
        }
    }
}
