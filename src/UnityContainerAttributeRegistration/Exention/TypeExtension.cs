using System;

namespace UnityContainerAttributeRegistration.Exention
{
    /// <summary>
    ///     Extensions for <see cref="Type" />.
    /// </summary>
    internal static class TypeExtension
    {
        /// <summary>
        ///     Checks if a type is static.
        /// </summary>
        /// <param name="self"><see cref="Type" /> to be checked.</param>
        /// <returns>whether a type is static or not.</returns>
        public static bool IsStatic(this Type self)
        {
            return self.IsAbstract && self.IsSealed;
        }
    }
}
