﻿using CheckDrive.ApiContracts.Mechanic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckDrive.Web.Stores.Mechanics
{
    public class MockMechanicDataStore : IMechanicDataStore
    {
        private readonly List<MechanicDto> _mechanics;

        public MockMechanicDataStore()
        {
            _mechanics = new List<MechanicDto>
            {
                new MechanicDto { Id = 1, Login = "user1", Password = "password1", PhoneNumber = "123456789", FirstName = "John", LastName = "Doe", Birthdate = new DateTime(1990, 1, 1)  },
                new MechanicDto { Id = 2, Login = "user2", Password = "password2", PhoneNumber = "987654321", FirstName = "Jane", LastName = "Siu", Birthdate = new DateTime(1995, 5, 15)},
            };
        }

        public async Task<List<MechanicDto>> GetMechanics()
        {
            await Task.Delay(100); 
            return _mechanics.ToList();
        }

        public async Task<MechanicDto> GetMechanic(int id)
        {
            await Task.Delay(100); 
            return _mechanics.FirstOrDefault(m => m.Id == id);
        }

        public async Task<MechanicDto> CreateMechanic(MechanicDto mechanic)
        {
            await Task.Delay(100);
            mechanic.Id = _mechanics.Max(m => m.Id) + 1; 
            _mechanics.Add(mechanic);
            return mechanic;
        }

        public async Task DeleteMechanic(int id)
        {
            await Task.Delay(100); 
            var mechanicToRemove = _mechanics.FirstOrDefault(m => m.Id == id);
            if (mechanicToRemove != null)
            {
                _mechanics.Remove(mechanicToRemove);
            }
        }
    }
}