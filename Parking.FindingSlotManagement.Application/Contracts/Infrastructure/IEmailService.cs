using Parking.FindingSlotManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendMail(EmailModel emailModel);
    }
}
