using AutoMapper;
using System.Collections.Generic;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.BLL.Interfaces;
using WebAppFurniture.DAL.Entities;
using WebAppFurniture.DAL.Interfaces;
using WebAppFurniture.DAL.Repositories;

namespace WebAppFurniture.BLL.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public ClientService(IMapper mapper, IClientRepository clientRepository)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
        }
        public async Task<ClientDTO> CreateAsync(ClientDTO entity)
        {
            var mappedEntity = _mapper.Map<Client>(entity);
            mappedEntity.Surname = " ";
            mappedEntity.Phone = " ";
            await _clientRepository.CreateAsync(mappedEntity);
            return entity;
        }

        public async Task<ClientDTO> DeleteAsync(int id)
        {
            var existingEntity = await _clientRepository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                throw new ArgumentException($" not found.");
            }
            await _clientRepository.DeleteAsync(_mapper.Map<Client>(existingEntity));
            return _mapper.Map<ClientDTO>(existingEntity);
        }

        public async Task<IEnumerable<ClientDTO>> GetAllAsync()
        {
            var allClients = _mapper.Map<IEnumerable<ClientDTO>>(await _clientRepository.GetAllAsync());
            return allClients;
        }

        public async Task<ClientDTO> GetByIdAsync(int id)
        {
            var client = _mapper.Map<ClientDTO>(await _clientRepository.GetByIdAsync(id));
            return client;
        }

        public async Task<ClientDTO> GetClientByUser(ClientDTO entity)
        {
            var allClients = await _clientRepository.GetAllAsync();
            var client = allClients.FirstOrDefault(c => c.User.Email == entity.User.Email);
            var mapped = _mapper.Map<ClientDTO>(client);
            return mapped;
        }

        public async Task<ClientDTO> GetClientByUserId(string id)
        {
            var allClients = await _clientRepository.GetAllAsync();
            var client = allClients.FirstOrDefault(t => t.User.Id == id);
            var result = _mapper.Map<ClientDTO>(client);
            return result;
        }

        public async Task<ClientDTO> UpdateAsync(ClientDTO entity)
        {
            var existing = await _clientRepository.GetByIdAsync(entity.Id);

            if (existing == null)
            {
                throw new ArgumentException("Client not found");
            }

            existing.Surname = entity.Surname;
            existing.Phone = entity.Phone;
            existing.Address = entity.Address;
            existing.Picture = entity.Picture;
            existing.User = entity.User;

            existing.Reviews = null;
            existing.Orders = null;
            existing.Notifications = null;
            try
            {
                await _clientRepository.UpdateAsync(existing);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return entity;
        }
    }
}
