using MISA.Core.Entities;
using MISA.Core.Interfaces.Repositories;
using MISA.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class DepartmentService :BaseService<Department>, IDepartmentService
    {
        IDepartmentRepository _departmentRepository;
        public DepartmentService(IDepartmentRepository departmentRepository):base(departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        protected override List<string> ValidateObjectCustom(Department entity)
        {
            // Thực hiện validate dữ liệu
            //Kiểm tra tên phòng ban đã có hay chưa?
            return base.ValidateObjectCustom(entity);
        }

        public int UpdateService(Guid id, Department asset)
        {
            throw new NotImplementedException();
        }
    }
}
