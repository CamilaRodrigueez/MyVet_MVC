using Infraestructure.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyVetDomain.Services.Interface
{
    public interface IRolServices
    {
        List<RolEntity> GetAll();
    }
}
