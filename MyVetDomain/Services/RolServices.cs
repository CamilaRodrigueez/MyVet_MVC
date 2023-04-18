using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Models;
using MyVetDomain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyVetDomain.Services
{
    public class RolServices: IRolServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<RolEntity> GetAll() => _unitOfWork.RolRepository.GetAll().ToList();
    }
}
