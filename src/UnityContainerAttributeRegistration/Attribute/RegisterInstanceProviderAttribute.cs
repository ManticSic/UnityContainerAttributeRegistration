using System;


namespace UnityContainerAttributeRegistration.Attribute
{
    /// <summary>
    ///     Mark a type to be a provider for <see cref="RegisterInstanceAttribute" />
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterInstanceProviderAttribute : System.Attribute
    {
    }
}
