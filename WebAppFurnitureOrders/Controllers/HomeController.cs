using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.BLL.Interfaces;
using WebAppFurnitureOrders.Models;
using WebAppFurnitureOrders.ViewModels;

namespace WebAppFurnitureOrders.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWarehouseService _warehouseService;
        private readonly IProductService _productService;
        private readonly IWarehouseProductService _warehouseProductService;
        private readonly INotificationService _notificationService;
        private readonly IProviderService _providerService;
        private readonly IProductGroupService _productGroupService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IClientService _clientService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public HomeController(ILogger<HomeController> logger, IMapper mapper, 
            IProductService productService, IWarehouseProductService warehouseProductService, IWarehouseService warehouseService,
            IProductGroupService productGroupService, IProviderService providerService, UserManager<IdentityUser> userManager, IClientService clientService, IOrderService orderService, INotificationService notificationService)
        {
            _logger = logger;
            _mapper = mapper;
            _productService = productService;
            _warehouseProductService = warehouseProductService;
            _warehouseService = warehouseService;
            _productGroupService = productGroupService;
            _providerService = providerService;
            _userManager = userManager;
            _clientService = clientService;
            _orderService = orderService;
            _notificationService = notificationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
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
        public async Task<IActionResult> Catalog()
        {
            var warehouseProducts = _mapper.Map<List<WarehouseProductModel>>(await _warehouseProductService.GetAllAsync());
            var products = new List<ProductViewModel>();
            var productss = _mapper.Map<List<ProductModel>>(await _productService.GetAllCatalogAsync());
            await Output(products, productss, warehouseProducts);

            return View(products);
        }

        private async Task Output(List<ProductViewModel> products, List<ProductModel> pro, List<WarehouseProductModel> warehouseProducts)
        {
            foreach (var p in pro)
            {
                var provider = _mapper.Map<ProviderModel>(await _providerService.GetByIdAsync(p.ProviderId));
                var group = _mapper.Map<ProductGroupModel>(await _productGroupService.GetByIdAsync(p.ProductGroupId));
                p.Provider = provider;
                p.ProductGroup = group;

                var associatedWarehouseProducts = warehouseProducts.Where(wp => wp.ProductId == p.Id).ToList();
                var images = new List<string>();
                if (associatedWarehouseProducts.Count > 0)
                {
                    foreach (var warehouseProduct in associatedWarehouseProducts)
                    {
                        var warehouse = _mapper.Map<WarehouseModel>(await _warehouseService.GetByIdAsync(warehouseProduct.WarehouseId));
                        var imageUrl = warehouse.ImageUrl;
                        images.Add(imageUrl);
                    }
                    var productViewModel = new ProductViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Cost = (double)p.Cost,
                        Type = p.Type,
                        ProductGroupId = p.ProductGroupId,
                        ProductGroup = p.ProductGroup,
                        ProviderId = p.Provider.Id,
                        Provider = p.Provider,
                        ImageUrls = images
                    };
                    products.Add(productViewModel);
                }
            }
        }

        public async Task<IActionResult> Filtered(string furnitureType)
        {
            var warehouseProducts = _mapper.Map<List<WarehouseProductModel>>(await _warehouseProductService.GetAllAsync());
            var products = new List<ProductViewModel>();
            var productss = _mapper.Map<List<ProductModel>>(await _productService.GetAllFilteredAsync(furnitureType));
            await Output(products, productss, warehouseProducts);

            if (products != null)
            {
                ViewBag.Results = products;
            }

            return View("Catalog", products);
        }
        public async Task<IActionResult> Search(string searchText)
        {
            var warehouseProducts = _mapper.Map<List<WarehouseProductModel>>(await _warehouseProductService.GetAllAsync());
            var products = new List<ProductViewModel>();
            var productss = _mapper.Map<List<ProductModel>>(await _productService.GetAllSearchedAsync(searchText));
            await Output(products, productss, warehouseProducts);

            if (products != null) ViewBag.Results = products;
            
            return View("Catalog", products);
        }

        public async Task<IActionResult> Details(int productId)
        {
            var product = _mapper.Map<ProductModel>(await _productService.GetByIdAsync(productId));

            var warehouseProducts = _mapper.Map<List<WarehouseProductModel>>(await _warehouseProductService.GetAllAsync());
            var provider = _mapper.Map<ProviderModel>(await _providerService.GetByIdAsync(product.ProviderId));
            var group = _mapper.Map<ProductGroupModel>(await _productGroupService.GetByIdAsync(product.ProductGroupId));

            var associatedWarehouseProducts = warehouseProducts.Where(wp => wp.ProductId == product.Id).ToList();

            var images = new List<string>();
            var colors = new List<string>();

            foreach (var warehouseProduct in associatedWarehouseProducts)
            {
                var warehouse = _mapper.Map<WarehouseModel>(await _warehouseService.GetByIdAsync(warehouseProduct.WarehouseId));
                var imageUrl = warehouse.ImageUrl;
                var color = warehouse.Color;
                images.Add(imageUrl);
                colors.Add(color);
            }

            var productViewModel = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description= product.Description,
                Cost = (double)product.Cost,
                Type = product.Type,
                ProductGroupId = product.ProductGroupId,
                ProviderId = product.ProviderId,
                ProductGroup = group,
                Provider = provider,
                ImageUrls = images,
                Colors = colors,
            };
            return View(productViewModel);
        }


        public async Task<IActionResult> OrderProduct(OrderViewModel model)
        {
            var product = await _productService.GetByIdAsync(model.ProductId);
            var user = await _userManager.GetUserAsync(User);
            var client = _mapper.Map<ClientDTO>(await _clientService.GetClientByUserId(user.Id));

            if (string.IsNullOrWhiteSpace(client.Phone) || string.IsNullOrWhiteSpace(client.Surname))
            {
                return Json(new { success = false, errorMessage = "Заполните поля ФИО и телефона в профиле" });
            }

            var order = new OrderModel
            {
                Date = DateTime.Now,
                ProgressCount = 0,
                TotalCost = model.TotalCost,
                Status = model.Status,
                ClientId = client.Id,
                ProductId = model.ProductId,
            };

            var orderDto = _mapper.Map<OrderDTO>(order);
            await _orderService.CreateAsync(orderDto);

            @model.Product = _mapper.Map<ProductModel>(await _productService.GetByIdAsync(@model.ProductId));

            @model.Product.ProductGroup = _mapper.Map<ProductGroupModel>(await _productGroupService.GetByIdAsync(@model.Product.ProductGroupId));
            var productgroup = @model.Product.ProductGroup.Name;
            var name = @model.Product.Name;

            NotificationDTO notification = new NotificationDTO() { Message = "Вы заказали " + productgroup + " "+name, Status = "Непрочитано", ClientId = client.Id ,Date=DateTime.Now };
            await _notificationService.CreateAsync(notification);

            return Json(new { success = true });
        }
        public async Task<IActionResult> SelectColor(string Color,int prouctId)
        {

            return RedirectToAction("Details");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}