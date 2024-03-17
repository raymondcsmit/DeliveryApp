namespace Delivery.Services.Abstract
{
    public interface ILookupServiceFactory
    {
        ILookupService<T> Create<T>() where T : class;
    }

}
