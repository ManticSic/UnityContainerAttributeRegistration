using System;


namespace UnityContainerAttributeRegistration.Attribute
{
    /// <summary>
    ///     Mark a type to be a provider for <see cref="RegisterInstanceAttribute" /> and <see cref="RegisterFactoryAttribute" />
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterProviderAttribute : System.Attribute
    {
    }
}
