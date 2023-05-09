﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class OTP
    {
        public int OTPID { get; set; }
        public string Code { get; set; }
        public DateTime ExpirationTime { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}

