using System;

using Unity.Lifetime;


namespace UnityContainerAttributeRegistrationTest.Helper.LifetimeManager
{
    public class TypeLifetimeManagerWithoutDefaultCtor : ITypeLifetimeManager
    {
        private readonly string anyValue;

        public TypeLifetimeManagerWithoutDefaultCtor(string anyValue)
        {
            this.anyValue = anyValue;
        }

        public Unity.Lifetime.LifetimeManager CreateLifetimePolicy()
        {
            throw new NotImplementedException();
        }
    }
}
