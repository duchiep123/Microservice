using GarageManagementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.Repository
{
    public class CarRepository : ICarRepository
    {
        private GarageManagementContext _context;

        public CarRepository(GarageManagementContext context) {

            _context = context;
        }
        public void AddCar(Car car)
        {
            _context.Cars.Add(car);
        }

        public void Delete(object key)
        {
            throw new NotImplementedException();
        }

        public Car GetCarById(int id)
        {
            var car = _context.Cars
                       .Where(c => c.Id == id)
                       .FirstOrDefault();
            return car;
        }

        public IEnumerable<Car> GetCars()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(object key, Car items)
        {
            throw new NotImplementedException();
        }
    }
}
