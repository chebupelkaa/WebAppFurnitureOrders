using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.BLL.Interfaces;
using WebAppFurniture.BLL.Services;

namespace WebAppFurnitureOrders.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        public NotificationController(INotificationService notificationService, IClientService clientService, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _notificationService = notificationService;
            _clientService = clientService;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> GetNotifications()
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _mapper.Map<ClientDTO>(await _clientService.GetClientByUserId(user.Id));
            var notifications = await _notificationService.GetNotificationsByClientIdAsync(client.Id);
            return Json(notifications);
        }
        public async Task<IActionResult> CheckUnreadNotifications()
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _mapper.Map<ClientDTO>(await _clientService.GetClientByUserId(user.Id));
            var notifications = await _notificationService.GetNotificationsByClientIdAsync(client.Id);
            var hasUnread = notifications.Any(n => n.Status == "Непрочитано");
            return Json(new { hasUnread });
        }
        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _mapper.Map<ClientDTO>(await _clientService.GetClientByUserId(user.Id));
            await _notificationService.MarkAllAsReadAsync(client.Id);
            return Ok();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
