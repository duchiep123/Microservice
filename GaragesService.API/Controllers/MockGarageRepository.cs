using GarageManagementModels;
using GaragesService.API.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaragesService.API.Controllers
{
    public class MockGarageRepository : Mock<IGarageRepository>
    {
        public MockGarageRepository MockGetByID(Garage result)
        {
            Setup(x => x.GetGarageById(It.IsAny<int>()))
            .Returns(result);
            
            return this;
        }

        public MockGarageRepository MockGetAllGarage(List<Garage> results)
        {
            Setup(x => x.GetGarages())
            .Returns(results);

            return this;
        }
        public MockGarageRepository VerifyForGarage(Times times) {
            Verify(x => x.GetGarageById(It.IsAny<int>()), times);
            return this;
        }
    }

}
