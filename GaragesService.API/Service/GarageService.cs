using GarageManagementModels;
using GaragesService.API.Repository;
using GaragesService.API.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaragesService.API.Service
{
    public class GarageService : IGarageService
    {
        private readonly IGarageRepository _garageRepository;
        public GarageService(IGarageRepository garageRepository) {
            _garageRepository = garageRepository;
        }
        public async Task<ResponseAddGarageModel> AddNewGarage()
        {
            Garage garage = new Garage()
            {
                Address = "Dong Nai",
                Name = "My Garage"
            };
            _garageRepository.AddGarage(garage);
            await _garageRepository.SaveChangesAsync();
            return new ResponseAddGarageModel() { Garage = garage, Message = "Success", Status = 0 };
        }

        public Garage GetGarageById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
