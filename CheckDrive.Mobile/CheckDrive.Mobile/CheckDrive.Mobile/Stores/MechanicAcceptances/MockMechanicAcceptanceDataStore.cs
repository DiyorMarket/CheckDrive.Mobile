using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.ApiContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckDrive.Web.Stores.MechanicAcceptances
{
    public class MockMechanicAcceptanceDataStore
    {
        private readonly List<MechanicAcceptanceDto> _mechanicAcceptances;

        public MockMechanicAcceptanceDataStore()
        {
            _mechanicAcceptances = new List<MechanicAcceptanceDto>
            {
                new MechanicAcceptanceDto { Id = 1, IsAccepted = true, Comments = "Completed", Status =StatusForDto.Completed, Date = DateTime.Now, Distance = 10, MechanicHandoverId = 1, DriverId = 1 },
                new MechanicAcceptanceDto { Id = 2, IsAccepted = false, Comments = "Rejected", Status = StatusForDto.Rejected, Date = DateTime.Now.AddDays(-1), Distance = 20, MechanicHandoverId = 1, DriverId = 1 },
                new MechanicAcceptanceDto { Id = 2, IsAccepted = false, Comments = "Rejected", Status = StatusForDto.Rejected, Date = DateTime.Now.AddDays(-2), Distance = 20, MechanicHandoverId = 1, DriverId = 2 },
                new MechanicAcceptanceDto { Id = 2, IsAccepted = false, Comments = "Rejected", Status = StatusForDto.Rejected, Date = DateTime.Now.AddDays(-3), Distance = 20, MechanicHandoverId = 1, DriverId = 1 },
                new MechanicAcceptanceDto { Id = 2, IsAccepted = false, Comments = "Rejected", Status = StatusForDto.Rejected, Date = DateTime.Now.AddDays(-4), Distance = 20, MechanicHandoverId = 1 , DriverId = 1},
                new MechanicAcceptanceDto { Id = 2, IsAccepted = false, Comments = "Rejected", Status = StatusForDto.Rejected, Date = DateTime.Now.AddDays(-5), Distance = 20, MechanicHandoverId = 1, DriverId = 2 },
                new MechanicAcceptanceDto { Id = 2, IsAccepted = false, Comments = "Rejected", Status = StatusForDto.Rejected, Date = DateTime.Now.AddDays(-6), Distance = 20, MechanicHandoverId = 2, DriverId = 2 },
            };
        }

        public List<MechanicAcceptanceDto> GetMechanicAcceptances()
        {
            return _mechanicAcceptances.ToList();
        }

        public MechanicAcceptanceDto GetMechanicAcceptance(int id)
        {
            return _mechanicAcceptances.FirstOrDefault(ma => ma.Id == id);
        }

        public async Task<MechanicAcceptanceDto> CreateMechanicAcceptance(MechanicAcceptanceDto mechanicAcceptance)
        {
            await Task.Delay(100); 
            mechanicAcceptance.Id = _mechanicAcceptances.Max(ma => ma.Id) + 1;
            _mechanicAcceptances.Add(mechanicAcceptance);
            return mechanicAcceptance;
        }

        public async Task<MechanicAcceptanceDto> UpdateMechanicAcceptance(int id, MechanicAcceptanceDto mechanicAcceptance)
        {
            await Task.Delay(100); 
            var existingMechanicAcceptance = _mechanicAcceptances.FirstOrDefault(ma => ma.Id == id);
            if (existingMechanicAcceptance != null)
            {
                existingMechanicAcceptance.IsAccepted = mechanicAcceptance.IsAccepted;
                existingMechanicAcceptance.Comments = mechanicAcceptance.Comments;
                existingMechanicAcceptance.Status = mechanicAcceptance.Status;
                existingMechanicAcceptance.Date = mechanicAcceptance.Date;
                existingMechanicAcceptance.Distance = mechanicAcceptance.Distance;
                existingMechanicAcceptance.MechanicHandoverId = mechanicAcceptance.MechanicHandoverId;
            }
            return existingMechanicAcceptance;
        }

        public async Task DeleteMechanicAcceptance(int id)
        {
            await Task.Delay(100); 
            var mechanicAcceptanceToRemove = _mechanicAcceptances.FirstOrDefault(ma => ma.Id == id);
            if (mechanicAcceptanceToRemove != null)
            {
                _mechanicAcceptances.Remove(mechanicAcceptanceToRemove);
            }
        }
    }
}
