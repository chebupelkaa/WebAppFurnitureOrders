using WebAppFurniture.BLL.DTO;

namespace WebAppFurniture.BLL.Interfaces
{
    public interface IClientService : IService<ClientDTO>
    {
        Task<ClientDTO> GetClientByUserId(string id);

        Task<ClientDTO> GetClientByUser(ClientDTO entity);
    }
}
