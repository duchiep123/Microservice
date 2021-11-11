using GarageManagementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.Repository
{
    public interface ICarRepository
    {
        IEnumerable<Car> GetCars();
        Car GetCarById(int id);
        void AddCar(Car car);
        void Delete(object key);
        void Update(object key, Car items);
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
