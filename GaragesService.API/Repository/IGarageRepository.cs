using GarageManagementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaragesService.API.Repository
{
    public interface IGarageRepository
    {
        IEnumerable<Garage> GetGarages();
        Garage GetGarageById(int id);
        void AddGarage(Garage garage);
        void Delete(object key);
        void Update(object key, Garage items);
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
