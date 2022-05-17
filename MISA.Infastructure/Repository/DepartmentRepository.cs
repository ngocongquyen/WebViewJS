using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Repositories;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infastructure.Repository
{
    public class DepartmentRepository :BaseRepository<Department>, IDepartmentRepository
    {
       
        public DepartmentRepository(IConfiguration configuration):base(configuration)
        {
           
        }

        public int Delete(Guid entityID)
        {
            throw new NotImplementedException();
        }

       

        public object GetById(Guid entityID)
        {
            throw new NotImplementedException();
        }

        public int Insert(Department department)
        {
            throw new NotImplementedException();
        }

        public int Update(Guid entityID, Department entity)
        {
            throw new NotImplementedException();
        }
    }
}
