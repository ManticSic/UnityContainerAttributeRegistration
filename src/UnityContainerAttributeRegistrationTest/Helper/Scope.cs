using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Moq;

using UnityContainerAttributeRegistration.Adapter;


namespace UnityContainerAttributeRegistrationTest.Helper
{
    internal class Scope
    {
        private readonly Mock<Assembly>          assemblyMock;
        private readonly Mock<IAppDomainAdapter> appDomainMock;

        private readonly IList<Type> typesInAssembly = new List<Type>();

        public Scope()
        {
            assemblyMock  = new Mock<Assembly>();
            appDomainMock = new Mock<IAppDomainAdapter>();
        }

        public Assembly Assembly
        {
            get => assemblyMock.Object;
        }

        public void AddType(Type type)
        {
            typesInAssembly.Add(type);
        }

        public IAppDomainAdapter GetAppDomain()
        {
            assemblyMock.Setup(mock => mock.GetTypes())
                        .Returns(typesInAssembly.ToArray());

            appDomainMock.Setup(mock => mock.GetAssemblies())
                         .Returns(new List<Assembly>
                                  {
                                      assemblyMock.Object
                                  });

            return appDomainMock.Object;
        }
    }
}
