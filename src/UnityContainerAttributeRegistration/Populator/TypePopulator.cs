﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Unity;
using Unity.Lifetime;
using UnityContainerAttributeRegistration.Attribute;

namespace UnityContainerAttributeRegistration.Populator
{
    /// <summary>
    ///     Populator for the <see cref="UnityContainerAttributeRegistration.Attribute.RegisterTypeAttribute" />.
    /// </summary>
    internal class TypePopulator : Populator
    {
        /// <inheritdoc cref="Populator.Populate" />
        /// <exception cref="InvalidOperationException">Class type must not be static or abstract.</exception>
        public override IUnityContainer Populate(IUnityContainer container, IList<Type> typesWithAttribute)
        {
            foreach (Type to in typesWithAttribute)
            {
                RegisterTypeAttribute attribute = to.GetCustomAttribute<RegisterTypeAttribute>();
                Type                  from      = attribute.From;
                string                name      = attribute.Name;
                ITypeLifetimeManager lifetimeManager = attribute.LifetimeManager == null
                                                           ? null
                                                           : GetInstanceByType<ITypeLifetimeManager>(attribute.LifetimeManager);

                container.RegisterType(from, to, name, lifetimeManager);
            }

            return container;
        }
    }
}
