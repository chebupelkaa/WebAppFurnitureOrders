
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.BLL.Interfaces;
using WebAppFurniture.DAL.Entities;
using WebAppFurniture.DAL.Interfaces;
using WebAppFurniture.DAL.Repositories;

namespace WebAppFurniture.BLL.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public ReviewService(IMapper mapper, IReviewRepository reviewRepository)
        {
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }

        public async Task<ReviewDTO> CreateAsync(ReviewDTO entity)
        {

            var mappedEntity = _mapper.Map<Review>(entity);
            mappedEntity.Date = DateTime.Now;

            await _reviewRepository.CreateAsync(mappedEntity);
            return entity;
        }

        public async Task<ReviewDTO> DeleteAsync(int id)
        {
            var existingEntity = await _reviewRepository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                throw new ArgumentException($" not found.");
            }
            await _reviewRepository.DeleteAsync(_mapper.Map<Review>(existingEntity));
            return _mapper.Map<ReviewDTO>(existingEntity);
        }

        public async Task<IEnumerable<ReviewDTO>> GetAllAsync()
        {
            var mapped = _mapper.Map <IEnumerable<ReviewDTO>>(await _reviewRepository.GetAllAsync());
            return mapped;
        }

        public async Task<ReviewDTO> GetByIdAsync(int id)
        {
            var review = _mapper.Map<ReviewDTO>(await _reviewRepository.GetByIdAsync(id));
            if (review == null)
            {
                throw new ArgumentException("User not found");
            }
            var entity = _mapper.Map<ReviewDTO>(review);
            return entity;
        }

        public Task<ReviewDTO> UpdateAsync(ReviewDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
