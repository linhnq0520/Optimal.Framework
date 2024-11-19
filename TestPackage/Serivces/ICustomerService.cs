namespace TestPackage.Serivces
{
    public interface ICustomerService
    {
        Task<bool> InsertCustomer(object customer);
    }
}
