using System;


namespace UnityContainerAttributeRegistration.Exention
{
    internal static class TypeExtension
    {
        public static bool IsStatic(this Type self)
        {
            return self.IsAbstract && self.IsSealed;
        }
    }
}
