using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Transactions;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.BLL.Interfaces;
using WebAppFurniture.BLL.Services;
using WebAppFurniture.DAL.Entities;
using WebAppFurnitureOrders.Models;
using WebAppFurnitureOrders.ViewModels;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace WebAppFurnitureOrders.Controllers
{
    public class OrderController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IClientService _clientService;
        private readonly IOrderService _orderService;
        private readonly IWarehouseService _warehouseService;
        private readonly IProductService _productService;
        private readonly IParameterService _parameterService;
        private readonly IParameterProductService _parameterProductService;
        private readonly INotificationService _notificationService;
        private readonly IProductGroupService _productGroupService;

        private readonly IMapper _mapper;

        public OrderController(IClientService clientService, 
            IWarehouseService warehouseService, IMapper mapper, UserManager<IdentityUser> userManager,IProductGroupService productGroupService,
            SignInManager<IdentityUser> signInManager, IOrderService orderService, IProductService productService, IParameterService parameterService, IParameterProductService parameterProductService, INotificationService notificationService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _orderService = orderService;
            _clientService = clientService;
            _productService = productService;
            _warehouseService = warehouseService;
            _parameterService = parameterService;
            _parameterProductService = parameterProductService;
            _notificationService = notificationService;
            _productGroupService= productGroupService;
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
        public IActionResult ToOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrderAsync(string furnitureType, string color, string texture, string height, string width, string depth,string cost)
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _mapper.Map<ClientDTO>(await _clientService.GetClientByUserId(user.Id));

            if(string.IsNullOrWhiteSpace(client.Phone) || string.IsNullOrWhiteSpace(client.Surname))
            {
                TempData["orderWarning"] = "Заполните поля ФИО и телефона в профиле";
                return Json(new { success = false, errorMessage = "Заполните поля ФИО и телефона в профиле" });
            }

            var parameters = new List<ParameterModel>
            {
                new ParameterModel { Name = "Высота", Value = height }, 
                new ParameterModel { Name = "Ширина", Value = width},
                new ParameterModel { Name = "Глубина", Value = depth},
                new ParameterModel { Name = "Цвет", Value = color },
                new ParameterModel { Name = "Текстура", Value = texture }
            };
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) { 
                foreach (var parameter in parameters)
                {
                    var existingParameter = await _parameterService.GetParameterByNameAndValueAsync(parameter.Name, parameter.Value);
                    if (existingParameter == null)
                    {
                        var p = await _parameterService.CreateAsync(_mapper.Map<ParameterDTO>(parameter));
                        p = await _parameterService.GetParameterByNameAndValueAsync(p.Name, p.Value);
                        parameter.Id = p.Id;
                    }
                    else parameter.Id = existingParameter.Id;
                }

                var prGroup= await _productGroupService.FindByNameAsync(furnitureType);
                var product = new ProductModel
                {
                    Name = "собираемый " + furnitureType.ToLower(),
                    Description = $"цвет: {color.ToLower()}, текстура: {texture.ToLower()}, высота: {height}, ширина: {width}, глубина:{depth}",
                    Type = "под заказ",
                    Cost = Convert.ToDouble(cost),//Convert.ToDouble(cost)
                    ProviderId = 1,
                    ProductGroupId = prGroup.Id,
                };

                var pr = await _productService.CreateAsync(_mapper.Map<ProductDTO>(product));
                var existingProduct = await _productService.GetProductByPrametersAsync(pr.Name, pr.Description, pr.Type, (double)pr.Cost, pr.ProductGroupId, pr.ProviderId);
                var productId = existingProduct.Id;

                try
                {
                    foreach (var parameter in parameters)
                    {
                        var existingParameterProduct = await _parameterProductService.GetParameterProductByParametersAsync(productId, parameter.Id);
                        if (existingParameterProduct == null)
                        {
                            var parameterProduct = new ParameterProductModel
                            {
                                ProductId = productId,
                                ParameterId = parameter.Id
                            };
                            await _parameterProductService.CreateAsync(_mapper.Map<ParameterProductDTO>(parameterProduct));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ОШИБКА: "+ex.ToString());
                }
               
                var order = new OrderModel
                {
                    Date = DateTime.Now,
                    ProductId = productId,
                    Status = "Отправлено специалисту",
                    ClientId = client.Id,
                    ProgressCount = 0,
                    TotalCost = Convert.ToDouble(product.Cost)
                };
                await _orderService.CreateAsync(_mapper.Map<OrderDTO>(order));


                var productgroup = _mapper.Map<ProductGroupModel>(await _productGroupService.GetByIdAsync(product.ProductGroupId)); 
                var name = product.Name;
                NotificationDTO notification = new NotificationDTO() { Message = "Вы заказали " + name, Status = "Непрочитано", ClientId = client.Id , Date = DateTime.Now };
                await _notificationService.CreateAsync(notification);

                transaction.Complete();
            }
            return Json(new { success = true });
        }

        public async Task<IActionResult> OrdersForSpecialistAsync()
        {
            var orders = _mapper.Map<List<OrderModel>>(await _orderService.GetAllAsync());
            var customOrders = new List<CustomOrderViewModel>();
            var regularOrders = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                var product = _mapper.Map<ProductModel>(await _productService.GetByIdAsync(order.ProductId));
                var client = _mapper.Map<ClientModel>(await _clientService.GetByIdAsync(order.ClientId));
                order.Client = client;
                order.Product = product;
                order.Product.ProductGroup = _mapper.Map<ProductGroupModel>(await _productGroupService.GetByIdAsync(order.Product.ProductGroupId));

                if (product.Type != "под заказ")
                {
                    var warehouseProduct = product.WarehouseProducts.FirstOrDefault(wp => wp.ProductId == product.Id);
                    var warehouse = _mapper.Map<WarehouseModel>(await _warehouseService.GetByIdAsync(warehouseProduct.WarehouseId));
                    warehouseProduct.Warehouse = warehouse;
                    var color = "";
                    var material = "";
                    if (warehouseProduct != null)
                    {
                        color = warehouseProduct.Warehouse.Color;
                        material = warehouseProduct.Warehouse.Material;
                    }
                    var orderViewModel = new OrderViewModel
                    {
                        Id = order.Id,
                        Date = order.Date,
                        ProgressCount = order.ProgressCount,
                        TotalCost = order.TotalCost,
                        Status = order.Status,
                        Client = order.Client,
                        ClientId = order.ClientId,
                        Product = order.Product,
                        ProductId = order.Product.Id,
                        Color = color,
                        Material = material
                    };
                    regularOrders.Add(orderViewModel);
                }
                else
                {
                    var parameterProducts = _mapper.Map<List<ParameterProductModel>>(await _parameterProductService.GetByProductIdAsync(product.Id));
                    foreach (var parameterProduct in parameterProducts)
                    {
                        var p = _mapper.Map<ProductModel>(await _productService.GetByIdAsync(parameterProduct.ProductId));
                        var param = _mapper.Map<ParameterModel>(await _parameterService.GetByIdAsync(parameterProduct.ParameterId));
                        parameterProduct.Product = p;
                        parameterProduct.Parameter = param;
                    }
                    var customOrderViewModel = new CustomOrderViewModel
                    {
                        Id = order.Id,
                        Date = order.Date,
                        ProgressCount = order.ProgressCount,
                        TotalCost = order.TotalCost,
                        Status = order.Status,
                        Client = order.Client,
                        ClientId = order.ClientId,
                        Product = order.Product,
                        ProductId = order.Product.Id,
                        ParameterProduct = parameterProducts

                    };
                    customOrders.Add(customOrderViewModel);
                }
            }
            var viewModel = new OrdersViewModel
            {
                CustomOrders = customOrders,
                RegularOrders = regularOrders
            };
            return View(viewModel);
        }

        public async Task<IActionResult> StatisticsAsync(DateTime? startDate, DateTime? endDate)
        {
            var viewModel = new SalesStatisticsViewModel();

            viewModel.StartDate = startDate ?? DateTime.Now.AddMonths(-1);
            viewModel.EndDate = endDate ?? DateTime.Now.AddDays(1);

            var categorySalesStatistics = await _orderService.GetCategorySalesStatisticsAsync(viewModel.StartDate, viewModel.EndDate);
            var categorySalesList = categorySalesStatistics.ToList();
            viewModel.CategorySalesStatistics = _mapper.Map<List<CategorySalesStatistics>>(categorySalesList);
            viewModel.CategoryNames = viewModel.CategorySalesStatistics?.Select(c => c.CategoryName) ?? Enumerable.Empty<string>();
            viewModel.CategorySales = viewModel.CategorySalesStatistics?.Select(c => c.TotalSales) ?? Enumerable.Empty<decimal>();


            var canceledSalesStatistics = await _orderService.GetCanceledSalesStatisticsAsync(viewModel.StartDate, viewModel.EndDate);
            var canceledSalesList = canceledSalesStatistics.ToList(); 
            viewModel.CanceledSalesStatistics = _mapper.Map<List<CanceledSalesStatistics>>(canceledSalesList);

            viewModel.TotalCost= await _orderService.GetTotalCostStatisticsAsync(viewModel.StartDate, viewModel.EndDate);

            return View(viewModel);
        }

        public async Task<IActionResult> GetTotalCost(DateTime? startDate, DateTime? endDate)
        {
            var Start = startDate ?? DateTime.Now.AddMonths(-1);
            var End = endDate ?? DateTime.Now;
            var totalcost = await _orderService.GetTotalCostStatisticsAsync(Start, End);
            return Json(totalcost);
        }
        public async Task<IActionResult> GetCanceledProducts(DateTime? startDate, DateTime? endDate)
        {
            var Start = startDate ?? DateTime.Now.AddMonths(-1);
            var End= endDate ?? DateTime.Now;
            var canceledOrders = await _orderService.GetCanceledSalesStatisticsAsync(Start, End);

            var json = JsonConvert.SerializeObject(_mapper.Map<IEnumerable<CanceledSalesStatistics>>(canceledOrders));

            return Json(json);
        }

        public async Task<IActionResult> StatisticsChart(DateTime? startDate, DateTime? endDate)
        {
            var categorySalesStatistics = await _orderService.GetCategorySalesStatisticsAsync(startDate ?? DateTime.Now.AddMonths(-1), endDate ?? DateTime.Now);
            var categorySalesList = categorySalesStatistics.ToList();

            return Json(categorySalesList);
        }

        public async Task<IActionResult> GetTopProducts(DateTime? startDate, DateTime? endDate,int count)
        {
            var topProducts = _mapper.Map<IEnumerable<ProductStatsModel>>(await _productService.GetTopProductsByOrderCount(startDate ?? DateTime.Now.AddMonths(-1), endDate ?? DateTime.Now, count));
            foreach (var product in topProducts)
            {
                var productgroup = _mapper.Map<ProductGroupModel>(await _productGroupService.GetByIdAsync(product.Product.ProductGroupId));
                product.Product.ProductGroup=productgroup;
                product.Product.ProductGroup.Products = null;
                product.Product.Orders = null;
                product.Product.WarehouseProducts = null;
            }
            var json = JsonConvert.SerializeObject(_mapper.Map<IEnumerable<ProductStatsModel>>(topProducts));
            return Ok(json);
        }

        public async Task<IActionResult> GetWorstProducts(DateTime? startDate, DateTime? endDate, int count)
        {
            var worstProducts = _mapper.Map <IEnumerable<ProductStatsModel>>(await _productService.GetWorstProductsByOrderCount(startDate ?? DateTime.Now.AddMonths(-1), endDate ?? DateTime.Now, count));
            foreach (var product in worstProducts)
            {
                var productgroup = _mapper.Map<ProductGroupModel>(await _productGroupService.GetByIdAsync(product.Product.ProductGroupId));
                product.Product.ProductGroup = productgroup;
                product.Product.ProductGroup.Products = null;
                product.Product.Orders = null;
                product.Product.WarehouseProducts = null;
            }
            var json = JsonConvert.SerializeObject(_mapper.Map<IEnumerable<ProductStatsModel>>(worstProducts));
            return Ok(json);
        }



        public async Task<IActionResult> AllOrdersAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _mapper.Map<ClientModel>(await _clientService.GetClientByUserId(user.Id));

            var orders = _mapper.Map<List<OrderModel>>(await _orderService.GetOrdersByClientIdAsync(client.Id));
            var customOrders = new List<CustomOrderViewModel>();
            var regularOrders = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                var product = _mapper.Map<ProductModel>(await _productService.GetByIdAsync(order.ProductId));
                order.Client = client;
                order.Product = product;
                var productGroup = _mapper.Map<ProductGroupModel>(await _productGroupService.GetByIdAsync(product.ProductGroupId));
                order.Product.ProductGroup = productGroup;
                if (product.Type != "под заказ")
                {
                    var warehouseProduct = product.WarehouseProducts.FirstOrDefault(wp => wp.ProductId == product.Id);
                    var warehouse = _mapper.Map<WarehouseModel>(await _warehouseService.GetByIdAsync(warehouseProduct.WarehouseId));
                    warehouseProduct.Warehouse = warehouse;
                    var color = "";
                    var material = "";
                    if (warehouseProduct != null)
                    {
                        color = warehouseProduct.Warehouse.Color;
                        material = warehouseProduct.Warehouse.Material;
                    }
                    var orderViewModel = new OrderViewModel
                    {
                        Id = order.Id,
                        Date = order.Date,
                        ProgressCount = order.ProgressCount,
                        TotalCost = order.TotalCost,
                        Status = order.Status,
                        Client = order.Client,
                        ClientId = order.ClientId,
                        Product = order.Product,
                        ProductId = order.Product.Id,
                        Color = color,
                        Material = material
                    };
                    regularOrders.Add(orderViewModel);
                }
                else
                {

                    var parameterProducts = order.Product.ParameterProducts;
                    //var parameterProducts = _mapper.Map<List<ParameterProductModel>>(await _parameterProductService.GetByProductIdAsync(order.ProductId));
                    foreach (var parameterProduct in parameterProducts)
                    {
                        var p = _mapper.Map<ProductModel>(await _productService.GetByIdAsync(parameterProduct.ProductId));
                        var param = _mapper.Map<ParameterModel>(await _parameterService.GetByIdAsync(parameterProduct.ParameterId));
                        parameterProduct.Product = p;
                        parameterProduct.Parameter = param;
                    }

                    var customOrderViewModel = new CustomOrderViewModel
                    {
                        Id = order.Id,
                        Date = order.Date,
                        ProgressCount = order.ProgressCount,
                        TotalCost = order.TotalCost,
                        Status = order.Status,
                        Client = order.Client,
                        ClientId = order.ClientId,
                        Product = order.Product,
                        ProductId = order.Product.Id,
                        ParameterProduct = parameterProducts

                    };
                    customOrders.Add(customOrderViewModel);
                }

            }
            var viewModel = new OrdersViewModel
            {
                CustomOrders = customOrders,
                RegularOrders = regularOrders
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = _mapper.Map<OrderModel>(await _orderService.GetByIdAsync(orderId));
            var client = _mapper.Map<ClientDTO>(await _clientService.GetByIdAsync(order.ClientId));
            var product = _mapper.Map<ProductModel>(await _productService.GetByIdAsync(order.ProductId));

            var productgroup = _mapper.Map<ProductGroupModel>(await _productGroupService.GetByIdAsync(product.ProductGroupId));
            order.Status = newStatus;
            if (newStatus == "Готово")
            {
                order.ProgressCount = 100;
                NotificationDTO notification = new NotificationDTO() { Message = "Заказ " + productgroup.Name +" "+ product.Name + " готов к выдаче", Status = "Непрочитано", ClientId = client.Id , Date = DateTime.Now };
                await _notificationService.CreateAsync(notification);
            }
            else if (newStatus == "Отправлено специалисту")
            {
                order.ProgressCount = 0;
            }
            else if (newStatus == "Передан в производство")
            {
                order.ProgressCount = 50;
            }
            else if (newStatus == "Оформляется")
            {
                order.ProgressCount = 20;
            }
            else if (newStatus == "Отменен")
            {
                order.ProgressCount = 0;
            }
            var e =await _orderService.UpdateAsync(_mapper.Map<OrderDTO>(order));
            var newProgressCount = order.ProgressCount;
            return Json(new { newProgressCount });
        }
        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int orderId, string newStatus)
        {
            var order = _mapper.Map<OrderModel>(await _orderService.GetByIdAsync(orderId));
            var client = _mapper.Map<ClientDTO>(await _clientService.GetByIdAsync(order.ClientId));
            var product = _mapper.Map<ProductModel>(await _productService.GetByIdAsync(order.ProductId));

            var productgroup = _mapper.Map<ProductGroupModel>(await _productGroupService.GetByIdAsync(product.ProductGroupId));
            order.Status = newStatus;

            if (newStatus == "Отменен")
            {
                order.ProgressCount = 0;
                NotificationDTO notification = new NotificationDTO() { Message = "Заказ " + productgroup.Name + " " + product.Name + " отменен", Status = "Непрочитано", ClientId = client.Id , Date = DateTime.Now };
                await _notificationService.CreateAsync(notification);
            }
            var e = await _orderService.UpdateAsync(_mapper.Map<OrderDTO>(order));
            return Json(new { success = true });
        }
        public async Task<IActionResult> CreateOrderAsync()
        {
            return View();
        }
    }
}
