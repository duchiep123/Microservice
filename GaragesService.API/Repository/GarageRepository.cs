using GarageManagementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaragesService.API.Repository
{
    public class GarageRepository : IGarageRepository
    {
        private GarageManagementContext _context;

        public GarageRepository(GarageManagementContext context)
        {

            _context = context;
        }
        public void AddGarage(Garage garage)
        {
            _context.Garage.Add(garage);
        }

        public void Delete(object key)
        {
            throw new NotImplementedException();
        }

        public Garage GetGarageById(int id)
        {
            var garage = _context.Garage
                       .Where(g => g.Id == id)
                       .FirstOrDefault();
            return garage;
        }

        public IEnumerable<Garage> GetGarages()
        {
            return _context.Garage.AsEnumerable();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(object key, Garage items)
        {
            throw new NotImplementedException();
        }
    }
}
