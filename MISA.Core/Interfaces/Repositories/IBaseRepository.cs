using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repositories
{
    public interface IBaseRepository<T>
    {
        /// <summary>
        /// Lấy toàn bộ bản ghi
        /// </summary>
        /// <returns>Trả về tất cả các bản ghi</returns>
        /// CreatedBy: NCQUYEN(12/5/2022)
        List<T> Get();

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <returns>Trả về số lượng bản ghi được thêm vào database</returns>
        /// CreatedBy: NCQUYEN(12/5/2022)
        int Insert(T entity);

        /// <summary>
        /// Chỉnh sửa bản ghi
        /// </summary>
        /// <returns>Số bản ghi đã thay đổi</returns>
        /// CreatedBy: NCQUYEN(12/5/2022)
        int Update(T entity, Guid entityID);

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <returns>Số bản ghi đã bị xóa</returns>
        /// CreatedBy: NCQUYEN(12/5/2022)
        int Delete(Guid entityID);

        /// <summary>
        /// Lấy ra 1 bản ghi
        /// </summary>
        /// <returns>Một bản ghi </returns>
        /// CreatedBy: NCQUYEN(12/5/2022)
        object GetById(Guid entityID);
    }
}
