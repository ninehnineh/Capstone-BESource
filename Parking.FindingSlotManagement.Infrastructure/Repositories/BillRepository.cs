﻿using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories
{
    public class BillRepository : GenericRepository<Bill>, IBillRepository
    {
        public BillRepository(ParkZDbContext dbContext) : base(dbContext)
        {
        }
    }
}