using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.BLL.Interfaces;
using WebAppFurniture.BLL.Services;
using WebAppFurnitureOrders.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.IO;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;

namespace WebAppFurnitureOrders.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IClientService _clientService;
        private readonly INotificationService _notificationService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public AccountController(IMapper mapper, RoleManager<IdentityRole> roleManager, INotificationService notificationService,
            IWebHostEnvironment hostEnvironment, IClientService clientService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _clientService = clientService;
            _hostEnvironment = hostEnvironment;
            _roleManager = roleManager;
            _notificationService = notificationService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            TempData["UserAvatar"] = " ";
            if (User.Identity.IsAuthenticated)
            {
                var user = _userManager.GetUserAsync(User).Result;
                var client = _mapper.Map<ClientModel>(_clientService.GetClientByUserId(user.Id).Result);
                if (client.Picture == null) TempData["UserAvatar"] = "default.png";
                else TempData["UserAvatar"] = client.Picture;
            }
            else TempData["UserAvatar"] = null;
            base.OnActionExecuting(context);
        }
        public IActionResult LogIn()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                var emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
                var isValidEmail = Regex.IsMatch(model.Email, emailPattern);

                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Пользователь с таким email уже существует");
                    var errorss = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    TempData["ErrorsReg"] = errorss;
                    return View(model);
                }
                if (!isValidEmail)
                {
                    ModelState.AddModelError("Email", "Неправильный формат email");
                    var errorss = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    TempData["ErrorsReg"] = errorss;
                    return View(model);
                }

                IdentityUser user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    ClientDTO client = new ClientDTO() { Phone = " ", User = user, Address = " ", Picture = "default.png", Surname = " " };
                    var clientResult = await _clientService.CreateAsync(client);

                    var roleExists = await _roleManager.RoleExistsAsync("Клиент");
                    if (!roleExists)
                    {
                        var role = new IdentityRole("Клиент");
                        await _roleManager.CreateAsync(role);
                    }
                    await _userManager.AddToRoleAsync(user, "Клиент");

                    var newClient = await _clientService.GetClientByUser(clientResult);

                    NotificationDTO notification = new NotificationDTO() { Message = "Добро пожаловать!", Status = "Непрочитано", ClientId = newClient.Id, Date = DateTime.Now };
                    await _notificationService.CreateAsync(notification);
                    return RedirectToAction("Catalog", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            TempData["ErrorsReg"] = errors;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser == null)
                {
                    ModelState.AddModelError("Email", "Пользователя с таким email не существует");
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(email, password, true, false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("WrongEmailOrPass", "Неверный пароль");
                    return View();
                }

                return RedirectToAction("Catalog", "Home");
            }
            return View();
        }
        public async Task<IActionResult> LogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Catalog", "Home");
        }


        public async Task<IActionResult> EditProfileAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _mapper.Map<ClientDTO>(await _clientService.GetClientByUserId(user.Id));
            var result = new ClientModel
            {
                Email = user.Email,
                Surname = client.Surname ?? "",
                Address = client.Address ?? "",
                Phone = client.Phone ?? "",
                Picture = client.Picture ?? "",
            };
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfileAsync(ClientModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var client = await _clientService.GetClientByUserId(user.Id);

            if (!string.IsNullOrEmpty(model.CurrentPassword))
            {
                var isPasswordValid = await IsPasswordValid(model.CurrentPassword, user.Id);
                if (isPasswordValid)
                {

                    if (model.ImageFile != null)
                    {
                        model.Picture = null;
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                        string extension = Path.GetExtension(model.ImageFile.FileName);
                        model.Picture = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/images/avatar", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(fileStream);
                        }
                    }
                    else
                    {
                        model.Picture = client.Picture;
                    }

                    if (!string.IsNullOrEmpty(model.Phone) || !string.IsNullOrEmpty(model.Surname))
                    {

                        var phoneRegex = new Regex(@"^\+?\d{1,3}?\d{9}$");
                        if (!phoneRegex.IsMatch(model.Phone))
                        {
                            ModelState.AddModelError("Phone", "Неправильный формат номера телефона.");
                        }
                        else
                        {

                            client = await _clientService.GetClientByUserId(user.Id);
                            var updated = new ClientDTO
                            {
                                Id = client.Id,
                                Surname = model.Surname ?? "",
                                Address = model.Address ?? "",
                                Phone = model.Phone ?? "",
                                Picture = model.Picture ?? "",
                            };
                            await _clientService.UpdateAsync(updated);

                            if (user != null && !string.IsNullOrEmpty(model.NewPassword))
                            {
                                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.CurrentPassword, isPersistent: false, lockoutOnFailure: false);
                                if (result.Succeeded)
                                {
                                    if (model.NewPassword == model.ConfirmPassword)
                                    {
                                        var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                                        if (changePasswordResult.Succeeded)
                                        {
                                            TempData["SuccessMessage"] = "Пароль успешно обновлен.";
                                        }
                                        else ModelState.AddModelError("ConfirmPassword", "Ошибка.");

                                    }
                                    else ModelState.AddModelError("ConfirmPassword", "Пароль и его подтверждение не совпадают.");
                                }
                            }
                            else TempData["SuccessMessage"] = "Данные обновлены.";
                            //ModelState.Remove("CurrentPassword");

                        }
                    }
                    else { ModelState.AddModelError("Phone", "Введите номер телефона и ФИО."); }

                }
                else ModelState.AddModelError("CurrentPassword", "Неверный пароль.");

            }
            else { ModelState.AddModelError("CurrentPassword", "Введите пароль."); }

            return View(model);
            //return  RedirectToAction("EditProfile", model);
        }

        public async Task<bool> IsPasswordValid(string currentPassword, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, currentPassword, false);
                return result.Succeeded;
            }

            return false;
        }
    }
}
