using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Services
{
    public interface IBaseService<T>
    {
        /// <summary>
        /// Xử lý nghiệp vụ khi thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Số lượng bản ghi thêm mới vào Database</returns>
        /// CreatedBy: QuyenNC(10/5/2022)
        int InsertService(T entity);

        /// <summary>
        /// Xử lý nghiệp vụ khi sửa tài sản
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="entity"></param>
        /// <returns>Bản ghi đã được chỉnh sửa</returns>
        ///  CreatedBy: QuyenNC(10/5/2022)
        int UpdateService(T entity, Guid entityID);
    }
}
