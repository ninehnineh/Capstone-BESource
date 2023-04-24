using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
/*            services.AddDbContext<FStoreDBContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
*/
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            /*services.AddScoped<IProductRepository, ProductRepository>();*/
            return services;
        }
    }
}
