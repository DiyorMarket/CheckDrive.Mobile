using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.MechanicHandover;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckDrive.Web.Stores.MechanicHandovers
{
    public class MockMechanicHandoverDataStore
    {
        private readonly List<MechanicHandoverDto> _mechanicHandovers;

        public MockMechanicHandoverDataStore()
        {
            _mechanicHandovers = new List<MechanicHandoverDto>
            {
                new MechanicHandoverDto { Id = 1, IsHanded = true, Comments = "Completed", Status = StatusForDto.Completed, Date = DateTime.Now, MechanicId = 1, CarId = 1, DriverId = 1 },
                new MechanicHandoverDto { Id = 2, IsHanded = false, Comments = "Rejected", Status = StatusForDto.Rejected, Date = DateTime.Now.AddDays(-1), MechanicId = 2, CarId = 2, DriverId = 1 },
                new MechanicHandoverDto { Id = 2, IsHanded = false, Comments = "Rejected", Status = StatusForDto.Rejected, Date = DateTime.Now.AddDays(-2), MechanicId = 2, CarId = 2, DriverId = 1 },
                new MechanicHandoverDto { Id = 2, IsHanded = false, Comments = "Rejected", Status = StatusForDto.Rejected, Date = DateTime.Now.AddDays(-3), MechanicId = 2, CarId = 2, DriverId = 1 },
                new MechanicHandoverDto { Id = 2, IsHanded = false, Comments = "Rejected", Status = StatusForDto.Rejected, Date = DateTime.Now.AddDays(-4), MechanicId = 2, CarId = 2, DriverId = 1 },
                new MechanicHandoverDto { Id = 2, IsHanded = false, Comments = "Rejected", Status = StatusForDto.Rejected, Date = DateTime.Now.AddDays(-5), MechanicId = 2, CarId = 2, DriverId = 1 },
            };
        }

        public async Task<List<MechanicHandoverDto>> GetMechanicHandovers()
        {
            await Task.Delay(100);
            return _mechanicHandovers.ToList();
        }

        public async Task<MechanicHandoverDto> GetMechanicHandover(int id)
        {
            await Task.Delay(100);
            return _mechanicHandovers.FirstOrDefault(mh => mh.Id == id);
        }

        public async Task<MechanicHandoverDto> CreateMechanicHandover(MechanicHandoverDto mechanicHandover)
        {
            await Task.Delay(100); 
            mechanicHandover.Id = _mechanicHandovers.Max(mh => mh.Id) + 1;
            _mechanicHandovers.Add(mechanicHandover);
            return mechanicHandover;
        }

        public async Task<MechanicHandoverDto> UpdateMechanicHandover(int id, MechanicHandoverDto mechanicHandover)
        {
            await Task.Delay(100); 
            var existingMechanicHandover = _mechanicHandovers.FirstOrDefault(mh => mh.Id == id);
            if (existingMechanicHandover != null)
            {
                existingMechanicHandover.IsHanded = mechanicHandover.IsHanded;
                existingMechanicHandover.Comments = mechanicHandover.Comments;
                existingMechanicHandover.Status = mechanicHandover.Status;
                existingMechanicHandover.Date = mechanicHandover.Date;
                existingMechanicHandover.MechanicId = mechanicHandover.MechanicId;
                existingMechanicHandover.CarId = mechanicHandover.CarId;
                existingMechanicHandover.DriverId = mechanicHandover.DriverId;
            }
            return existingMechanicHandover;
        }

        public async Task DeleteMechanicHandover(int id)
        {
            await Task.Delay(100);
            var mechanicHandoverToRemove = _mechanicHandovers.FirstOrDefault(mh => mh.Id == id);
            if (mechanicHandoverToRemove != null)
            {
                _mechanicHandovers.Remove(mechanicHandoverToRemove);
            }
        }
    }
}
