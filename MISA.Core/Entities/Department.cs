using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class Department:BaseEntity
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid DepartmentId { get; set; } 

        /// <summary>
        /// Mã của phòng ban
        /// </summary>
        public string DepartmentCode { get; set; }
        
        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string? description { get; set; }

        /// <summary>
        /// ID phòng ban cha
        /// </summary>
        public string? parent_id { get; set; }

        /// <summary>
        /// ID của đơn vị
        /// </summary>
        public Guid? organization_id { get; set; }

            
    }


}
