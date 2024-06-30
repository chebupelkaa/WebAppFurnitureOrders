using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.BLL.Interfaces;
using WebAppFurniture.DAL.Entities;
using WebAppFurniture.DAL.Interfaces;
using WebAppFurniture.DAL.Repositories;

namespace WebAppFurniture.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public NotificationService(IMapper mapper, INotificationRepository notificationRepository)
        {
            _mapper = mapper;
            _notificationRepository = notificationRepository;
        }
        public async Task<NotificationDTO> CreateAsync(NotificationDTO entity)
        {
            var mappedEntity = _mapper.Map<Notification>(entity);
            await _notificationRepository.CreateAsync(mappedEntity);
            return entity;
        }

        public async Task<NotificationDTO> DeleteAsync(int id)
        {
            var existingEntity = await _notificationRepository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                throw new ArgumentException($"Notification not found.");
            }
            await _notificationRepository.DeleteAsync(_mapper.Map<Notification>(existingEntity));
            return _mapper.Map<NotificationDTO>(existingEntity);
        }

        public async Task<IEnumerable<NotificationDTO>> GetAllAsync()
        {
            var mapped = _mapper.Map<IEnumerable<NotificationDTO>>(await _notificationRepository.GetAllAsync());
            return mapped;
        }

        public async Task<NotificationDTO> GetByIdAsync(int id)
        {
            var entity = _mapper.Map<NotificationDTO>(await _notificationRepository.GetByIdAsync(id));
            if (entity == null)
            {
                throw new ArgumentException("not found");
            }
            var mapped = _mapper.Map<NotificationDTO>(entity);
            return mapped;
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotificationsByClientIdAsync(int id)
        {
            var all = _mapper.Map<IEnumerable<NotificationDTO>>(await _notificationRepository.GetAllAsync());
            return all.Where(o => o.ClientId == id);
        }

        public async Task<IEnumerable<NotificationDTO>> MarkAllAsReadAsync(int clientid)
        {
            var all = _mapper.Map<IEnumerable<NotificationDTO>>(await _notificationRepository.GetAllAsync());
            var notifications = all.Where(o => o.ClientId == clientid);
            foreach (var notification in notifications)
            {
                if (notification.Status == "Непрочитано")
                {
                    notification.Status = "Прочитано";
                    await _notificationRepository.UpdateAsync(_mapper.Map<Notification>(notification));
                }
            }
            return notifications;
        }

        public async Task<NotificationDTO> UpdateAsync(NotificationDTO entity)
        {
            var existingEntity = await _notificationRepository.GetByIdAsync(entity.Id);
            if (existingEntity == null)
            {
                throw new ArgumentException($"{entity.Id} not found.");
            }
            await _notificationRepository.UpdateAsync(existingEntity);
            return _mapper.Map<NotificationDTO>(existingEntity);
        }
    }
}
