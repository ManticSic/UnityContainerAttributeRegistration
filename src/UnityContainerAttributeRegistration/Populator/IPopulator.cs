using Unity;


namespace UnityContainerAttributeRegistration.Populator
{
    public interface IPopulator
    {
        public IUnityContainer Populate(IUnityContainer container);
    }
}
