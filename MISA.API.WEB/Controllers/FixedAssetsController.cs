using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.API.WEB.Entities;
using MySqlConnector;
using Dapper;

namespace MISA.API.WEB.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FixedAssetsController : ControllerBase
    {
        IConfiguration _configuration;
        public FixedAssetsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// Lấy danh sách toàn bộ tài sản
        /// </summary>
        /// <returns>
        /// 200 - danh sách tài sản
        /// 204 - không có dữ liệu
        /// </returns>
        /// CreatedBy: NCQUyen(9/5/2022)
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // khai báo thông tin DataBase
                var connectionString = _configuration.GetConnectionString("NCQUYEN");
                //1 khởi tạo kết nối với MariaDB

                var sqlConnection = new MySqlConnection(connectionString);
                //2. Lấy dữ liệu
                //2.1 Câu lệnh truy vấn lấy dữ liệu
                var sqlCommand = "SELECT * from fixed_asset";
                var assets = sqlConnection.Query<FixedAsset>(sql: sqlCommand);
                return Ok(assets);
            }
            catch (Exception ex)
            {
                return HanleException(ex);
            }
        }


        /// <summary>
        /// Lấy tài sản theo ID
        /// </summary>
        /// <returns>
        /// 200 - tài sản theo ID
        /// 204 - không có dữ liệu
        /// </returns>
        /// CreatedBy: NCQUyen(9/5/2022)
        [HttpGet("{assetId}")]
        public IActionResult GetById(Guid assetId)
        {
            try
            {
                // khai báo thông tin DataBase
                var connectionString = _configuration.GetConnectionString("NCQUYEN");
                //1 khởi tạo kết nối với MariaDB

                var sqlConnection = new MySqlConnection(connectionString);
                //2. Lấy dữ liệu
                //2.1 Câu lệnh truy vấn lấy dữ liệu
                var sqlCommand = $"SELECT * from fixed_asset where AssetID = @assetId";
                // Lưu ý nếu có tham số truyền cho câu lệnh truy vấn sql thì phải sử dụng DynamicParameters
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@assetId", assetId.ToString());
                var asset = sqlConnection.QueryFirstOrDefault<FixedAsset>(sql: sqlCommand, param: parameters);

                return Ok(asset);
            }
            catch (Exception ex)
            {
                var error = new ErrorService();
                error.DevMsg = ex.Message;
                error.UserMsg = Resource.Resource.Error_Exception;
                error.Data = ex.Data;
                return StatusCode(500, error);
            }
        }


        /// <summary>
        /// Thêm mới asset
        /// </summary>
        /// <param name="asset">Thông tin asset</param>
        /// <returns>
        ///     201 - Thêm mới thành công
        ///     400 - Dữ liệu đầu vào không hợp lệ
        ///     500 - Có exception
        /// </returns>
        [HttpPost]
        public IActionResult PostData([FromBody] FixedAsset asset)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("NCQUYEN");
                // khởi tạo kết nối với MariaDB
                var sqlConnection = new MySqlConnection(connectionString);

                //1. Validate dữ liệu: trả về mã 400(BadRequest) kèm các thông tin
                var validateErrorsMsg = new List<string>();
                if (string.IsNullOrEmpty(asset.AssetCode))
                {
                    validateErrorsMsg.Add(Resource.Resource.ErrorValidte_AssetCode_NotEmpty);
                }

                if (string.IsNullOrEmpty(asset.AssetName))
                {
                    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_AssetName_NotEmpty);
                }

                if (string.IsNullOrEmpty(asset.DepartmentCode))
                {
                    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_DepartmentCode_NotEmpty);
                }

                if (string.IsNullOrEmpty(asset.FixedAssetCategoryCode))
                {
                    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_FixedAssetCategoryCode_NotEmpty);
                }

                if (string.IsNullOrEmpty(asset.Quantity.ToString()))
                {
                    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_Quantity_NotEmpty);
                }

                if (string.IsNullOrEmpty(asset.Cost.ToString()))
                {
                    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_Cost_NotEmpty);
                }

                if (string.IsNullOrEmpty(asset.LifeTime.ToString()))
                {
                    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_LifeTime_NotEmpty);
                }

                //if (asset.PurchaseDate == DateTime.MinValue)
                //{
                //    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_PurchaseDate_NotEmpty);
                //}

                //Kiểm tra xem mã tài sản đã tồn tại hay chưa?
                var sqlQueryCheckDuplicateCode = $"SELECT AssetCode FROM fixed_asset where AssetCode=@AssetCode";
                //  var asseParam = new DynamicParameters();
                //  asseParam.Add("@Fixed_asset_category_code",category.fixed_asset_category_code);
                var assetCodeDuplicate = sqlConnection.QueryFirstOrDefault<string>(sqlQueryCheckDuplicateCode, param: new { AssetCode = asset.AssetCode });
                //var assetCodeDuplicate = sqlConnection.QueryFirstOrDefault<string>(sqlQueryCheckDuplicateCode, param: asseParam);
                if (assetCodeDuplicate != null)
                {
                    validateErrorsMsg.Add(Resource.Resource.ErrorValidare_CheckDuplicatAssetCode);
                }

                if (validateErrorsMsg.Count > 0)
                {
                    var validateError = new ErrorService();
                    validateError.UserMsg = Resource.Resource.ErrorValidate;
                    validateError.Data = validateErrorsMsg;
                    return StatusCode(400, validateError);

                }
                //1.1 Thông tin asset bắt buộc nhập

                //1.2 thông tin không được phép để trống
                // khai báo thông tin DataBase
                asset.AssetId = Guid.NewGuid();
   
                var sqlQuery = $"INSERT INTO fixed_asset " +
                    $"(AssetID,AssetCode,AssetName,DepartmentId,DepartmentCode,DepartmentName," +
                    $"FixedAssetCategoryId,FixedAssetCategoryCode,FixedAssetCategoryName,PurchaseDate,Cost," +
                    $"Quantity,DepreciationRate,LifeTime,CreatedBy,CreatedDate)" +
                    $" VALUES(@AssetID, @AssetCode, @AssetName, @DepartmentId, @DepartmentCode, @DepartmentName," +
                    $" @FixedAssetCategoryId , @FixedAssetCategoryCode,@FixedAssetCategoryName,@PurchaseDate," +
                    $"@Cost,@Quantity,@DepreciationRate,@LifeTime,@CreatedBy,@CreatedDate); ";
                var parameters = new DynamicParameters();
                parameters.Add("@AssetID", asset.AssetId);
                parameters.Add("@AssetCode", asset.AssetCode);
                parameters.Add("@AssetName", asset.AssetName);
                parameters.Add("@DepartmentId", asset.DepartmentId);
                parameters.Add("@DepartmentCode", asset.DepartmentCode);
                parameters.Add("@DepartmentName", asset.DepartmentName);
                parameters.Add("@FixedAssetCategoryId", asset.FixedAssetCategoryId);
                parameters.Add("@FixedAssetCategoryCode", asset.FixedAssetCategoryCode);
                parameters.Add("@FixedAssetCategoryName", asset.FixedAssetCategoryName);
                parameters.Add("@PurchaseDate", asset.PurchaseDate);
                parameters.Add("@Cost", asset.Cost);
                parameters.Add("@Quantity", asset.Quantity);
                parameters.Add("@DepreciationRate", asset.DepreciationRate);
                parameters.Add("@LifeTime", asset.LifeTime);
                parameters.Add("@CreatedBy", asset.CreatedBy);
                parameters.Add("@CreatedDate", asset.CreatedDate);
             


                //thực hiện thêm dữ liệu
                var res = sqlConnection.Execute(sqlQuery, param: parameters);
                return StatusCode(201, res);

            }
            catch (Exception ex)
            {
                return HanleException(ex);
            }

        }


        /// <summary>
        /// Thêm mới asset
        /// </summary>
        /// <param name="asset">Thông tin asset</param>
        /// <returns>
        ///     201 - Thêm mới thành công
        ///     400 - Dữ liệu đầu vào không hợp lệ
        ///     500 - Có exception
        /// </returns>
        [HttpPut("{assetId}")]
        public IActionResult PutData([FromBody] FixedAsset asset, Guid assetId)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("NCQUYEN");
                // khởi tạo kết nối với MariaDB
                var sqlConnection = new MySqlConnection(connectionString);

                //1. Validate dữ liệu: trả về mã 400(BadRequest) kèm các thông tin
                var validateErrorsMsg = new List<string>();
                if (string.IsNullOrEmpty(asset.AssetCode))
                {
                    validateErrorsMsg.Add(Resource.Resource.ErrorValidte_AssetCode_NotEmpty);
                }

                if (string.IsNullOrEmpty(asset.AssetName))
                {
                    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_AssetName_NotEmpty);
                }

                if (string.IsNullOrEmpty(asset.DepartmentCode))
                {
                    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_DepartmentCode_NotEmpty);
                }

                if (string.IsNullOrEmpty(asset.FixedAssetCategoryCode))
                {
                    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_FixedAssetCategoryCode_NotEmpty);
                }

                if (string.IsNullOrEmpty(asset.Quantity.ToString()))
                {
                    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_Quantity_NotEmpty);
                }

                if (string.IsNullOrEmpty(asset.Cost.ToString()))
                {
                    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_Cost_NotEmpty);
                }

                if (string.IsNullOrEmpty(asset.LifeTime.ToString()))
                {
                    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_LifeTime_NotEmpty);
                }

                //if (asset.PurchaseDate == DateTime.MinValue)
                //{
                //    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_PurchaseDate_NotEmpty);
                //}

                //Kiểm tra xem mã tài sản đã tồn tại hay chưa?
                var getAssetCodeFromAssetId = $"SELECT AssetCode FROM fixed_asset where AssetID=@AssetID";

                var result = sqlConnection.QueryFirstOrDefault<string>(getAssetCodeFromAssetId, param: new { AssetID = assetId.ToString() });
                
                if(result != asset.AssetCode)
                {
                    var sqlQueryCheckDuplicateCode = $"SELECT AssetCode FROM fixed_asset where AssetCode=@AssetCode";
                    //  var asseParam = new DynamicParameters();
                    //  asseParam.Add("@Fixed_asset_category_code",category.fixed_asset_category_code);
                    var assetCodeDuplicate = sqlConnection.QueryFirstOrDefault<string>(sqlQueryCheckDuplicateCode, param: new { AssetCode = asset.AssetCode });


                    //var assetCodeDuplicate = sqlConnection.QueryFirstOrDefault<string>(sqlQueryCheckDuplicateCode, param: asseParam);
                    if (assetCodeDuplicate != null)
                    {
                        validateErrorsMsg.Add(Resource.Resource.ErrorValidare_CheckDuplicatAssetCode);
                    }
                }

                if (validateErrorsMsg.Count > 0)
                {
                    var validateError = new ErrorService();
                    validateError.UserMsg = Resource.Resource.ErrorValidate;
                    validateError.Data = validateErrorsMsg;
                    return StatusCode(400, validateError);

                }
                //1.1 Thông tin asset bắt buộc nhập

                //1.2 thông tin không được phép để trống
                // khai báo thông tin DataBase
                asset.AssetId = Guid.NewGuid();

                var sqlQuery = $"UPDATE fixed_asset SET AssetCode = @AssetCode," +
                    $"AssetName = @AssetName,DepartmentId = @DepartmentId,DepartmentCode = @DepartmentCode," +
                    $"DepartmentName = @DepartmentName,FixedAssetCategoryId = @FixedAssetCategoryId," +
                    $"FixedAssetCategoryCode = @FixedAssetCategoryCode,FixedAssetCategoryName = @FixedAssetCategoryName," +
                    $"PurchaseDate = @PurchaseDate,Cost = @Cost,Quantity = @Quantity,DepreciationRate = @DepreciationRate," +
                    $"LifeTime = @LifeTime,ModifiedBy = @ModifiedBy,ModifiedDate = @ModifiedDate WHERE AssetID = @AssetID";
                var parameters = new DynamicParameters();
                parameters.Add("@AssetID", assetId.ToString());
                parameters.Add("@AssetCode", asset.AssetCode);
                parameters.Add("@AssetName", asset.AssetName);
                parameters.Add("@DepartmentId", asset.DepartmentId);
                parameters.Add("@DepartmentCode", asset.DepartmentCode);
                parameters.Add("@DepartmentName", asset.DepartmentName);
                parameters.Add("@FixedAssetCategoryId", asset.FixedAssetCategoryId);
                parameters.Add("@FixedAssetCategoryCode", asset.FixedAssetCategoryCode);
                parameters.Add("@FixedAssetCategoryName", asset.FixedAssetCategoryName);
                parameters.Add("@PurchaseDate", asset.PurchaseDate);
                parameters.Add("@Cost", asset.Cost);
                parameters.Add("@Quantity", asset.Quantity);
                parameters.Add("@DepreciationRate", asset.DepreciationRate);
                parameters.Add("@LifeTime", asset.LifeTime);
                parameters.Add("@ModifiedBy", asset.ModifiedBy);
                parameters.Add("@ModifiedDate", asset.ModifiedDate);

              

                //thực hiện thêm dữ liệu
                var res = sqlConnection.Execute(sqlQuery, param: parameters);
                return StatusCode(200);

            }
            catch (Exception ex)
            {
                return HanleException(ex);
            }

        }

        [HttpDelete("{assetID}")] 
        public IActionResult DeleteData(Guid assetID)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("NCQUYEN");
                // khởi tạo kết nối với MariaDB
                var sqlConnection = new MySqlConnection(connectionString);
                // khởi tạo lloix
                var error = new ErrorService();
                // Câu lệnh sql
                var sqlQuery = $"DELETE FROM fixed_asset WHERE AssetID = @AssetID";
                //khai báo parameter dạng động
                var parameters = new DynamicParameters();
                //Gán giá trị truyền vào từ client vào param
                parameters.Add("@AssetID", assetID.ToString());
                // thực hiện câu lệnh truy vấn
                var res = sqlConnection.Execute(sqlQuery, param: parameters);
                //thực hiện thêm dữ liệu
                return Ok(res);

            }
            catch (Exception ex)
            {

                return HanleException(ex);
            }
        }

        public IActionResult HanleException(Exception ex)
        {
            var error = new ErrorService();
            error.DevMsg = ex.Message;
            error.UserMsg = Resource.Resource.Error_Exception;
            error.Data = ex.Data;
            return StatusCode(500, error);
        }
    }
}
