using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.API.WEB.Entities;
using MySqlConnector;
using Dapper;

namespace MISA.API.WEB.Controllers
{
    [Route("api/v1/[controller]")] //---> api/vi/Assets
    [ApiController]
    public class FixedAssetsCategoryController : ControllerBase
    {
        IConfiguration _configuration;
        public FixedAssetsCategoryController(IConfiguration configuration)
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
        /// CreatedBy: NCQUyen(5/5/2022)
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
                var sqlCommand = "SELECT * from fixed_asset_category";
                var assets = sqlConnection.Query<FixedAssetCategory>(sql: sqlCommand);
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
        /// CreatedBy: NCQUyen(5/5/2022)
        //[HttpGet("{assetId}")]
        //public IActionResult GetById(string assetId)
        //{
        //    try
        //    {
        //        // khai báo thông tin DataBase
        //        var connectionString = "Host=3.0.89.182; Port=3306; Database=MISA.WEB03.QUYENNC; User id=dev; Password=12345678";
        //        //1 khởi tạo kết nối với MariaDB

        //        var sqlConnection = new MySqlConnection(connectionString);
        //        //2. Lấy dữ liệu
        //        //2.1 Câu lệnh truy vấn lấy dữ liệu
        //        var sqlCommand = $"SELECT * from fixed_asset_category where fixed_asset_category_id = @assetId";
        //        // Lưu ý nếu có tham số truyền cho câu lệnh truy vấn sql thì phải sử dụng DynamicParameters
        //        DynamicParameters parameters = new DynamicParameters();
        //        parameters.Add("@assetId", assetId);
        //        var asset = sqlConnection.QueryFirstOrDefault<FixedAssetCategory>(sql: sqlCommand, param: parameters);

        //        return Ok(asset);
        //    }
        //    catch (Exception ex)
        //    {
        //        var error = new ErrorService();
        //        error.DevMsg = ex.Message;
        //        error.UserMsg = Resource.Resource.Error_Exception;
        //        error.Data = ex.Data;
        //        return StatusCode(500, error);
        //    }
        //}

        /// <summary>
        /// Thêm mới asset
        /// </summary>
        /// <param name="fixedAssetCategory">Thông tin asset</param>
        /// <returns>
        ///     201 - Thêm mới thành công
        ///     400 - Dữ liệu đầu vào không hợp lệ
        ///     500 - Có exception
        /// </returns>
        [HttpPost]
        public IActionResult PostData([FromBody]FixedAssetCategory category)
        {
            try
            {
                var connectionString = "Host=3.0.89.182; Port=3306; Database=MISA.WEB03.QUYENNC; User id=dev; Password=12345678";
                var sqlConnection = new MySqlConnection(connectionString);
                
                //1. Validate dữ liệu: trả về mã 400(BadRequest) kèm các thông tin
                var validateErrorsMsg = new List<string>();
                if(string.IsNullOrEmpty(category.fixed_asset_category_code))
                {
                    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_FixedAssetCategoryCode_NotEmpty);
                }

             

                if(validateErrorsMsg.Count > 0)
                {
                    var validateError = new ErrorService();
                    validateError.UserMsg = Resource.Resource.ErrorValidate;
                    validateError.Data = validateErrorsMsg;
                    return StatusCode(400, validateError);

                }
                //1.1 Thông tin asset bắt buộc nhập

                //1.2 thông tin không được phép để trống
                // khai báo thông tin DataBase
                category.fixed_asset_category_id = Guid.NewGuid();
                //khởi tạo kết nối với MariaDB
                var sqlQuery = $"INSERT INTO fixed_asset_category (fixed_asset_category_id, fixed_asset_category_code, fixed_asset_category_name,created_by,description,modified_by) VALUES(@Fixed_asset_category_id, @Fixed_asset_category_code, @Fixed_asset_category_name,@Created_by,@Description,@Modified_by); ";
                var parameters = new DynamicParameters();
                parameters.Add("@Fixed_asset_category_id", category.fixed_asset_category_id);
                parameters.Add("@Fixed_asset_category_code", category.fixed_asset_category_code);
                parameters.Add("@Fixed_asset_category_name", category.fixed_asset_category_name);
                parameters.Add("@Created_by", category.created_by);
                parameters.Add("@Description", category.description);
                parameters.Add("@Modified_by", category.modified_by);

                //Kiểm tra xem mã tài sản đã tồn tại hay chưa?
                var sqlQueryCheckDuplicateCode = $"SELECT fixed_asset_category_code FROM fixed_asset_category where fixed_asset_category_code=@Fixed_asset_category_code";
               //  var asseParam = new DynamicParameters();
              //  asseParam.Add("@Fixed_asset_category_code",category.fixed_asset_category_code);
             var assetCodeDuplicate = sqlConnection.QueryFirstOrDefault<string>(sqlQueryCheckDuplicateCode, param: new { fixed_asset_category_code = category.fixed_asset_category_code });
                //var assetCodeDuplicate = sqlConnection.QueryFirstOrDefault<string>(sqlQueryCheckDuplicateCode, param: asseParam);
                if (assetCodeDuplicate != null)
                {
                    var validateError = new
                    {
                        userMsg = "Mã tài sản đã trùng",
                        data= "fixed_asset_category_code"
                    };
                    return StatusCode(400, validateError);
                }
                //thực hiện thêm dữ liệu
                var res = sqlConnection.Execute(sqlQuery, param: parameters);
                return StatusCode(201, res);
              
            }
            catch (Exception ex)
            {
                return HanleException(ex);
            }

        }
        
        [HttpDelete("{categoryId}")]
        public IActionResult DeleteData(string categoryId) {
            try
            {
                var connectionString = "Host=3.0.89.182; Port=3306; Database=MISA.WEB03.QUYENNC; User id=dev; Password=12345678";
                var sqlConnection = new MySqlConnection(connectionString);
                var error = new ErrorService();
                var sqlQuery = $"DELETE FROM fixed_asset_category WHERE fixed_asset_category_id = @CategoryId";
                var parameters = new DynamicParameters();
                parameters.Add("@CategoryId", categoryId);
                var res = sqlConnection.Execute(sqlQuery, param: parameters);
                //thực hiện thêm dữ liệu
                return Ok(res);

            }
            catch(Exception ex)
            {
                return HanleException(ex);
            }
            
        }

        [HttpPut("{categoryId}")]
        public IActionResult PutData(string categoryId, FixedAssetCategory category)
        {
            try
            {
                var connectionString = "Host=3.0.89.182; Port=3306; Database=MISA.WEB03.QUYENNC; User id=dev; Password=12345678";
                var sqlConnection = new MySqlConnection(connectionString);
                var error = new ErrorService();

                if (string.IsNullOrEmpty(category.fixed_asset_category_code))
                {
                    error.UserMsg = Resource.Resource.ErrorValidate_FixedAssetCategoryCode_NotEmpty;
                }

                var sqlQuery = $"UPDATE fixed_asset_category SET fixed_asset_category_code = @CategoryCode, fixed_asset_category_name = @CategoryName WHERE fixed_asset_category_id = @CategoryId; ";
                
                var parameters = new DynamicParameters();
                parameters.Add("@CategoryId", categoryId);
                parameters.Add("@CategoryCode", category.fixed_asset_category_code);
                parameters.Add("@CategoryName", category.fixed_asset_category_name);
                var res = sqlConnection.Execute(sqlQuery, param: parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex);
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
