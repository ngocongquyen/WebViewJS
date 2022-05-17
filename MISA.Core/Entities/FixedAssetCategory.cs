using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class FixedAssetCategory:BaseEntity
    {
        /// <summary>
        /// khóa chính
        /// </summary>
        public Guid FixedAssetCategoryId { get; set; }

        /// <summary>
        /// Mã loại tài sản
        /// </summary>
        public string FixedAssetCategoryCode { get; set; }

        /// <summary>
        /// Tên loại tài sản
        /// </summary>
        public string? FixedAssetCategoryName { get; set; }

        /// <summary>
        /// Khóa phụ id đơn vị
        /// </summary>
        public Guid? organization_id { get; set; }

        /// <summary>
        /// Tỷ lệ hao mòn năm (%)
        /// </summary>
        public float DepreciationRate { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>
        public int LifeTime { get; set; }

        /// <summary>
        /// ghi chú
        /// </summary>
        public string? description { get; set; }
    }
}
