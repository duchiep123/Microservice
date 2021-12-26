using GarageManagementModels;
using GaragesService.API.RequestModels;
using GaragesService.API.Repository;
using GaragesService.API.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using AutoMapper;
using GarageManagementModels.Dtos;

namespace GaragesService.API.Service
{
    public class GarageService : IGarageService
    {
        private readonly IGarageRepository _garageRepository;
        private readonly IMapper _mapper;
        public GarageService(IGarageRepository garageRepository, IMapper mapper ) {
            _garageRepository = garageRepository;
            _mapper = mapper;
        }
        public async Task<ResponseAddGarageModel> AddNewGarage(RequestCreateGarageModel request)
        {
            Garage garage = new Garage()
            {
                Address = request.Address,
                Name = request.Name
            };
            _garageRepository.AddGarage(garage);
            await _garageRepository.SaveChangesAsync();
            return new ResponseAddGarageModel() { Garage = garage, Message = "Success", Status = 0 };
        }
        public ReturnGetGarageModel GetGarageById(int id)
        {
            var result = _garageRepository.GetGarageById(id);
            if (result != null) {
                var tmp = _mapper.Map<GarageReadDto>(result);
                ReturnGetGarageModel returnGetGarageModel = new ReturnGetGarageModel()
                {
                    GarageId = tmp.Id,
                    GarageName = tmp.Name,
                    Address = tmp.Address,
                    Cars = tmp.Cars.ToList<CarReadDto>()
                };
                return returnGetGarageModel;
            }
            return new ReturnGetGarageModel();
        }
    }
}
