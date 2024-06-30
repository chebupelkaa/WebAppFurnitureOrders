using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.BLL.Interfaces;
using WebAppFurniture.BLL.Services;
using WebAppFurniture.DAL.Entities;
using WebAppFurnitureOrders.Models;
using WebAppFurnitureOrders.ViewModels;

namespace WebAppFurnitureOrders.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IWarehouseService _warehouseService;
        private readonly IProductService _productService;
        private readonly IWarehouseProductService _warehouseProductService;
        private readonly IProviderService _providerService;
        private readonly IProductGroupService _productGroupService;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        public WarehouseController(IMapper mapper, UserManager<IdentityUser> userManager,IClientService clientService, IProviderService providerService,
            IProductGroupService productGroupService, IWarehouseService warehouseService,IProductService productService,IWarehouseProductService warehouseProductService, IWebHostEnvironment hostEnvironment)
        {
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
            _warehouseService = warehouseService;
            _productService = productService;
            _warehouseProductService = warehouseProductService;
            _providerService = providerService;
            _productGroupService= productGroupService;
            _clientService = clientService;
            _userManager = userManager;
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

        public async Task<IActionResult> Index()
        {
            var warehouses = _mapper.Map<List<WarehouseModel>>(await _warehouseService.GetAllAsync());
            var warehouseproducts = _mapper.Map<List<WarehouseProductModel>>(await _warehouseProductService.GetAllAsync());
            var providers = _mapper.Map<List<ProviderModel>>(await _providerService.GetAllAsync());
            var productGroups = _mapper.Map<List<ProductGroupModel>>(await _productGroupService.GetAllAsync());
            var products = _mapper.Map<List<ProductModel>>(await _productService.GetAllAsync());

            foreach(var product in products)
            {
                product.ProductGroup = _mapper.Map<ProductGroupModel>(await _productGroupService.GetByIdAsync(product.ProductGroupId));
            }
            var viewModel = new WarehouseManagementViewModel
            {
                Warehouses = warehouses,
                Products = products,
                Providers = providers,
                ProductGroups = productGroups,
                WarehouseProducts = warehouseproducts,
                SelectedWarehouseId = warehouses.FirstOrDefault()?.Id ?? 0
            };
            return View(viewModel);
        }


        public async Task<IActionResult> GetWarehouseProducts(int warehouseNumber)
        {
            var warehouseProducts= await _warehouseProductService.GetWarehouseProducts(warehouseNumber);
            var productsView = new List<WarehouseProductViewModel>();
            foreach (var en in warehouseProducts) {

                var warehouse = await _warehouseService.GetByIdAsync(en.WarehouseId); 
                var product = await _productService.GetByIdAsync(en.ProductId);

                var productGroup = await _productGroupService.GetByIdAsync(product.ProductGroupId);
                var provider = await _providerService.GetByIdAsync(product.ProviderId);
                var productViewModel = new WarehouseProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    ProductGroup = productGroup.Name,
                    Provider = provider.Name,
                    Cost = (double)product.Cost,
                    WarehouseNumber = warehouseNumber,
                    Count = warehouse.Count,
                    Material = warehouse.Material,
                    Color = warehouse.Color,
                    Description= product.Description,
                    ImageURL = warehouse.ImageUrl,
                    WarehouseProductId= en.Id,
                    WarehouseId= en.WarehouseId,
                };
                productsView.Add(productViewModel);
            };
            return Json(productsView);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToWarehouse([FromForm] AddNewProductViewModel model)
        {
            if (model.ImageFile != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                string extension = Path.GetExtension(model.ImageFile.FileName);
                model.ImageURL = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }
            }
            else return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            var newProduct = new ProductModel
            {
                //Id = model.ProductId,
                Name = model.Name,
                Cost = model.Cost,
                Description = model.Description,
                Type = "из каталога",
                ProductGroupId = model.ProductGroupId,
                ProviderId = model.ProviderId
            };
            var product = await _productService.CreateAsync(_mapper.Map<ProductDTO>(newProduct));
            var productEntity = await _productService.GetProductByPrametersAsync(product.Name,product.Description,product.Type,(double)product.Cost,product.ProductGroupId,product.ProviderId);

            var newWarehouse = new WarehouseModel
            {
                Color=model.Color,
                Material=model.Material,
                Count=model.Count,
                ImageUrl=model.ImageURL
            };
            var checkwarhouse = await _warehouseService.GetWarehouseByEntityAsync(_mapper.Map<WarehouseDTO>(newWarehouse));
            var warehouse = new WarehouseDTO();
            var warehouseEntity = new WarehouseDTO();
            if (checkwarhouse == null)
            {
                warehouse = await _warehouseService.CreateAsync(_mapper.Map<WarehouseDTO>(newWarehouse));
                warehouseEntity = await _warehouseService.GetWarehouseByEntityAsync(warehouse);
            }
            else warehouseEntity = await _warehouseService.GetWarehouseByEntityAsync(_mapper.Map<WarehouseDTO>(newWarehouse));

            var newWarehouseProduct = new WarehouseProductModel
            {
                WarehouseNumber = model.WarehouseNumber,
                WarehouseId = warehouseEntity.Id,
                ProductId = productEntity.Id,
            };
            await _warehouseProductService.CreateAsync(_mapper.Map<WarehouseProductDTO>(newWarehouseProduct));
            return Json(new { success = true });
            
            //return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        [HttpPost]
        public async Task<IActionResult> AddExistingProductToWarehouse([FromForm] AddNewProductViewModel model)
        {
            var productEntity = await _productService.GetByIdAsync(model.ProductId);

            if (model.ImageFile != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                string extension = Path.GetExtension(model.ImageFile.FileName);
                model.ImageURL = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }
            } else return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            var newWarehouse = new WarehouseModel
            {
                Color = model.Color,
                Material = model.Material,
                Count = model.Count,
                ImageUrl = model.ImageURL
            };
            var checkwarhouse= await _warehouseService.GetWarehouseByEntityAsync(_mapper.Map<WarehouseDTO>(newWarehouse));
            var warehouse = new WarehouseDTO();
            var warehouseEntity=new WarehouseDTO();
            if (checkwarhouse==null)
            {
                warehouse = await _warehouseService.CreateAsync(_mapper.Map<WarehouseDTO>(newWarehouse));
                warehouseEntity = await _warehouseService.GetWarehouseByEntityAsync(warehouse);
            }
            else warehouseEntity = await _warehouseService.GetWarehouseByEntityAsync(_mapper.Map<WarehouseDTO>(newWarehouse));
            
                 
            var newWarehouseProduct = new WarehouseProductModel
            {
                WarehouseNumber = model.WarehouseNumber,
                WarehouseId = warehouseEntity.Id,
                ProductId = productEntity.Id,
            };
            await _warehouseProductService.CreateAsync(_mapper.Map<WarehouseProductDTO>(newWarehouseProduct));
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> EditProductWarehouse([FromForm] AddNewProductViewModel model)
        {
            var newProduct = new ProductModel
            {
                Id = model.ProductId,
                Name = model.Name,
                Cost = model.Cost,
                Description = model.Description,
                Type = "из каталога",
                ProductGroupId = model.ProductGroupId,
                ProviderId = model.ProviderId
            };
            var product = await _productService.UpdateAsync(_mapper.Map<ProductDTO>(newProduct));
            var productEntity = await _productService.GetProductByPrametersAsync(product.Name, product.Description, product.Type, (double)product.Cost, product.ProductGroupId, product.ProviderId);
            
            if (model.ImageFile != null)
            {
                model.ImageURL = null;
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                string extension = Path.GetExtension(model.ImageFile.FileName);
                model.ImageURL = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }
            }
            
            var newWarehouse = new WarehouseModel
            {
                Id=model.WarehouseId,
                Color = model.Color,
                Material = model.Material,
                Count = model.Count,
                ImageUrl = model.ImageURL
            };
            var warehouse = await _warehouseService.UpdateAsync(_mapper.Map<WarehouseDTO>(newWarehouse));
            var warehouseEntity = await _warehouseService.GetWarehouseByEntityAsync(warehouse);

            var newWarehouseProduct = new WarehouseProductModel
            {
                Id = model.WarehouseProductId,
                WarehouseNumber = model.WarehouseNumber,
                WarehouseId = warehouseEntity.Id,
                ProductId = productEntity.Id,
            };
            var existingWarehouseProduct = await _warehouseProductService.GetByIdAsync(newWarehouseProduct.Id);
            if (existingWarehouseProduct != null)
            {
                await _warehouseProductService.UpdateAsync(_mapper.Map<WarehouseProductDTO>(newWarehouseProduct));
            }

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int productId,int warehouseNumber)
        {

            var entityfordelete = await _warehouseProductService.GetWarehouseProduct(productId, warehouseNumber);
            await _warehouseProductService.DeleteAsync(entityfordelete.Id);

            return Json(new { success = true });
        }
    }
}
