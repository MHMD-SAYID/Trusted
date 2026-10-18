using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Abstraction;
using Trusted.Core.IRepositories;
using Trusted.Core.IServices;
using Trusted.ViewModel.DTO.Response.Lookups;

namespace Trusted.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGameRepository _gameRepository;

        public GameService(IHttpContextAccessor httpContextAccessor, IGameRepository gameRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _gameRepository = gameRepository;
        }

        public async Task<Result<List<GamelookupRes>>> GetAllAsync()
        {
            try
            {
                var games = await _gameRepository.GetAllAsync();
                var request = _httpContextAccessor.HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";
                var response = games.Select(g => new GamelookupRes
                {
                    Id = g.Id,
                    ArName = g.ArName,
                    EnName = g.EnName,
                    Logo = $"{baseUrl}/{g.Logo}",
                    //BackGroundColor = g.BackGroundColor
                }).ToList();
                return new Result<List<GamelookupRes>>
                {
                    IsSuccess = true,
                    ArRes = "تم جلب البيانات بنجاح",
                    EnRes = "Data fetched successfully",
                    Data = response,
                    StatusCode = 200,

                };
            }
            catch (Exception ex)
            {
                return new Result<List<GamelookupRes>>
                {
                    IsSuccess = false,
                    ArRes = "مشكله فى الخدمه",
                    EnRes = "Service is down",
                    Data = null,
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message, ex.InnerException?.Message ?? "Nothing" }
                };
            }
        }
        public async Task<Result<List<GamelookupRes>>> GetPopularAsync()
        {

            try
            {
                var games = await _gameRepository.FindAllAsync(g => g.IsPopular);

                var request = _httpContextAccessor.HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";

                var response = games.Select(g => new GamelookupRes
                {
                    Id = g.Id,
                    ArName = g.ArName,
                    EnName = g.EnName,
                    Logo = $"{baseUrl}/{g.Logo}",
                    //BackGroundColor = g.BackGroundColor
                }).ToList();
                return new Result<List<GamelookupRes>>
                {
                    IsSuccess = true,
                    ArRes = "تم جلب البيانات بنجاح",
                    EnRes = "Data fetched successfully",
                    Data = response,
                    StatusCode = 200,
                };
            }
            catch (Exception ex)
            {
                return new Result<List<GamelookupRes>>
                {
                    IsSuccess = false,
                    ArRes = "مشكله فى الخدمه",
                    EnRes = "Service is down",
                    Data = null,
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message, ex.InnerException?.Message ?? "Nothing" }
                };
            }
        }
    }
}
