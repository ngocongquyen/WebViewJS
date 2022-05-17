using MISA.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class Asset:BaseEntity
    {
        [PrimaryKey]
        /// <summary>
        /// khóa chính tài sản
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Mã loại tài sản
        /// </summary>
        [MaxLength(20)]
        [PropertyNameFriendly("Mã tài sản")]
        [IsNotNullOrEmpty]
        public string AssetCode { get; set; }

        /// <summary>
        /// Tên tài sản
        /// </summary>
        [IsNotNullOrEmpty]
        [PropertyNameFriendly("Tên tài sản")]
        public string AssetName { get; set; }

        /// <summary>
        /// khóa ngoại của đơn vị
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// Mã đơn vị
        /// </summary>
        public string? OrganizationCode { get; set; }

        /// <summary>
        /// Tên của đơn vị
        /// </summary>

        public string? OrganizationName { get; set; }

        /// <summary>
        /// Khóa ngoại phong ban
        /// </summary>
        public Guid? DepartmentId { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        [IsNotNullOrEmpty]
        [PropertyNameFriendly("Mã bộ phận sử dụng")]
        public string DepartmentCode { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Khóa ngoại loại tài sản
        /// </summary>
        public Guid? FixedAssetCategoryId { get; set; }

        /// <summary>
        /// Mã loại tài sản
        /// </summary>
        [IsNotNullOrEmpty]
        [PropertyNameFriendly("Tên loại tài sản")]
        public string FixedAssetCategoryCode { get; set; }

        /// <summary>
        /// Tên loại tài sản
        /// </summary>
        public string FixedAssetCategoryName { get; set; }

        /// <summary>
        /// Ngày mua
        /// </summary>
        [IsNotNullOrEmpty]
        [PropertyNameFriendly("Ngày mua")]
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// Nguyên giá
        /// </summary>
        [IsNotNullOrEmpty]
        [PropertyNameFriendly("Nguyên giá")]
        public decimal Cost { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        [IsNotNullOrEmpty]
        [PropertyNameFriendly("Số lượng")]
        public int Quantity { get; set; }

        /// <summary>
        /// Tỷ lệ hao mòn (%)
        /// </summary>
        [IsNotNullOrEmpty]
        [PropertyNameFriendly("Tỷ lệ hao mòn")]
        public float DepreciationRate { get; set; }

        /// <summary>
        /// Năm bắt đầu theo dõi tài sản trên phần mềm
        /// </summary>
        
        public int? TrackedYear { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>
        [IsNotNullOrEmpty]
        [PropertyNameFriendly("Số năm sử dụng")]
        public int? LifeTime { get; set; }

        /// <summary>
        /// Năm sử dụng
        /// </summary>
        public DateTime? ProductionYear { get; set; }

    }
}
