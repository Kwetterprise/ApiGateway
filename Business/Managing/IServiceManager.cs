namespace Kwetterprise.ApiGateway.Business.Managing
{
    using System.Threading.Tasks;
    using Kwetterprise.ApiGateway.Business.Models;

    public interface IServiceManager
    {
        Task FetchServices();

        Service? Get(string name);
    }
}
