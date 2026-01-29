using AutoMapper;
using AutoMechanic.DataAccess.EF.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Repositories
{
    public class VehicleRepository(
        IDbContextFactory<AutoMechanicDbContext> dbContextFactory,
        IMapper mapper
    )
    {

    }
}
