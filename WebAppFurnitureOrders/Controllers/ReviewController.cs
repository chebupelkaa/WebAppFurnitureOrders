using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.BLL.Interfaces;
using WebAppFurniture.BLL.Services;
using WebAppFurniture.DAL.Entities;
using WebAppFurnitureOrders.Models;
using Microsoft.AspNetCore.Http;

namespace WebAppFurnitureOrders.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IClientService _clientService;



        public ReviewController(IMapper mapper, IReviewService reviewService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IClientService clientService)
        {
            _mapper = mapper;
            _reviewService = reviewService;
            _userManager = userManager;
            _signInManager = signInManager;
            _clientService = clientService;

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
        public async Task<IActionResult> AllReviewsAsync()
        {
            var reviews = _mapper.Map<List<ReviewModel>>(await _reviewService.GetAllAsync());

            foreach (var review in reviews)
            {
                var client = _mapper.Map<ClientModel>(await _clientService.GetByIdAsync(review.ClientId));
                review.Client = client;
            }
            reviews.Reverse();
            return View(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var client = await _clientService.GetClientByUserId(user.Id);
            if (model.Comment==null || model.Rating==0 || string.IsNullOrWhiteSpace(client.Surname))
            {
                var reviews = _mapper.Map<List<ReviewModel>>(await _reviewService.GetAllAsync());
                foreach (var review in reviews)
                {
                    var userr = await _userManager.GetUserAsync(User);
                    var clientt = _mapper.Map<ClientModel>(await _clientService.GetClientByUserId(userr.Id));
                    review.Client = clientt;
                }
                if (string.IsNullOrWhiteSpace(client.Surname))
                {
                    TempData["ErrorNull"] = "Заполните ФИО в профиле, чтобы оставить отзыв";
                }
                return View("AllReviews", reviews);
            }
            TempData["ErrorNull"] = null;
            var cl = _mapper.Map<ClientDTO> (client);

            if (client != null)
            {
                ReviewDTO review = new ReviewDTO()
                {
                    ClientId = cl.Id,
                    Comment = model.Comment,
                    Rating=model.Rating,
                };
                await _reviewService.CreateAsync(review);
            }
            return RedirectToAction("AllReviews");
        }

    }
}
