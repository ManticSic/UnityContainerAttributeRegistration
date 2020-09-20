namespace UnityContainerAttributeRegistration
{
    public enum TypeLifetimeManager
    {
        Default,
        HierarchicalLifetimeManager,
        SingletonLifetimeManager,
        TransientLifetimeManager,
        ContainerControlledLifetimeManager,
        ContainerControlledTransientManager,
        ExternallyControlledLifetimeManager,
        PerResolveLifetimeManager,
        PerThreadLifetimeManager
    }
}
