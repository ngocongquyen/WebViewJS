namespace MISA.API.WEB.Entities
{
    public class FixedAsset
    {
        /// <summary>
        /// khóa chính tài sản
        /// </summary>
        public Guid AssetId { get; set; } 
        
        /// <summary>
        /// Mã loại tài sản
        /// </summary>
        public string AssetCode { get; set; }

        /// <summary>
        /// Tên tài sản
        /// </summary>
        public string AssetName { get; set; }
        
        /// <summary>
        /// khóa ngoại của đơn vị
        /// </summary>
        public Guid OrganizationId { get; set; }
        
        /// <summary>
        /// Mã đơn vị
        /// </summary>
        public string ?OrganizationCode { get; set; }

        /// <summary>
        /// Tên của đơn vị
        /// </summary>

        public string ?OrganizationName { get; set; }
        
        /// <summary>
        /// Khóa ngoại phong ban
        /// </summary>
        public Guid DepartmentId { get; set; }  

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string ?DepartmentName { get; set; }

        /// <summary>
        /// Khóa ngoại loại tài sản
        /// </summary>
        public Guid FixedAssetCategoryId { get; set; }  

        /// <summary>
        /// Mã loại tài sản
        /// </summary>
        public string FixedAssetCategoryCode { get; set; }

        /// <summary>
        /// Tên loại tài sản
        /// </summary>
        public string ?FixedAssetCategoryName { get; set; }

        /// <summary>
        /// Ngày mua
        /// </summary>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Nguyên giá
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Tỷ lệ hao mòn (%)
        /// </summary>
        public float DepreciationRate { get; set; }

        /// <summary>
        /// Năm bắt đầu theo dõi tài sản trên phần mềm
        /// </summary>
        public int TrackedYear { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>
        public int LifeTime { get; set; }

        /// <summary>
        /// Năm sử dụng
        /// </summary>
        public DateTime ProductionYear { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string ?CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime ?CreatedDate { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public string ?ModifiedBy { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime ?ModifiedDate { get; set; }



    }
}
