using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Repositories;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.Infastructure.Repository
{
    public class FixedAssetRepository : BaseRepository<Asset>, IFixedAssetRepository
    {

        public FixedAssetRepository(IConfiguration configuration) : base(configuration)
        {

        }

        /// <summary>
        /// Kiểm tra mã tài sản trùng
        /// </summary>
        /// <param name="assetCode"></param>
        /// <returns>true - đã có false -chưa có</returns>
        /// Createdby: QuyenNC (11/5/2022)
        public bool CheckAssetCodeExist(Guid assetID, int mode, string assetCode)
        {

            var sqlQueryCheckDuplicateCode = "";
            // thêm mới kiểm tra trùng
            if (mode == 1)
            {
                //Kiểm tra xem mã tài sản đã tồn tại hay chưa?
                sqlQueryCheckDuplicateCode = $"SELECT AssetCode FROM Asset where AssetCode=@AssetCode";
            }

            //chỉnh sửa kiểm tra trùng
            if (mode == 2)
            {
                sqlQueryCheckDuplicateCode = $"SELECT AssetCode FROM Asset where AssetCode=@AssetCode AND AssetID <> @assetID";
            }

            var parameters = new DynamicParameters();
            parameters.Add("@assetID", assetID);
            parameters.Add("@AssetCode", assetCode);
            var assetCodeDuplicate = _sqlConnection.QueryFirstOrDefault<string>(sqlQueryCheckDuplicateCode, param: parameters);
            if (assetCodeDuplicate != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Tự sinh mã mới 
        /// </summary>
        /// <returns>Trả về mã mới tự động cộng 1 gần nhất</returns>
        /// CreatedBy NCQUYEN (16/05/2022)
        public string GetNewAssetCode()
        {
            // câu lệnh sql lấy mã tài sản theo ngày giảm dần;
            string sqlCommand = "SELECT AssetCode FROM Asset ORDER BY CreatedDate DESC";
            var sqlAssetCode = "SELECT AssetCode FROM Asset";
            // Lấy mã tài sản gần nhất
            var AssetCode = _sqlConnection.QueryFirstOrDefault<string>(sql: sqlCommand);

            // trả về mảng chuỗi và số riêng biệt.
            string[] output = Regex.Matches(AssetCode, "[0-9]+|[^0-9]+")
            .Cast<Match>()
            .Select(match => match.Value)
            .ToArray();
            
            // khai báo giá trị sau khi cộng
            int currentMax = 0;

            var valueAssetCode = "";
            var numberAssetCode = "";
            // khai báo mã tài sản mới trả vê
            string newAssetCode = "";
     
            var checkNumber = false;
            // nếu mảng trả về lớn hơn 1 phần
            if (output.Length > 1)
            {
                valueAssetCode = output[0];
                numberAssetCode = output[1];
                // kiểm tra xem chuỗi đàu tiên có phải toàn số hay không?
                checkNumber = IsNumber(valueAssetCode);
                // Nếu trả về là false nghĩa là chuỗi không có số
                if(checkNumber == false)
                {
                    //chuyển chuỗi về dạng số nếu có số 0;
                    var partNumber = int.Parse(numberAssetCode);
                    if (currentMax < partNumber)
                    {
                        // giá trị được tăng lên 1
                        currentMax = partNumber + 1;
                    }

                    // Ghép chuỗi;
                    newAssetCode = valueAssetCode + currentMax;
                    // Nếu chuỗi hiện tại mà nhỏ hơn chuỗi lấy về từ sql
                    if (newAssetCode.Length < AssetCode.Length)
                    {
                        // kiểm tra chừng nào chuỗi hiện tại mà nhỏ hơn chuỗi lấy về từ sql
                        while (newAssetCode.Length < AssetCode.Length)
                        {
                            string[] newOutput = Regex.Matches(newAssetCode, "[0-9]+|[^0-9]+")
                               .Cast<Match>()
                               .Select(match => match.Value)
                               .ToArray();
                            // chuỗi kí tự
                            valueAssetCode = newOutput[0];
                            // chuối số
                            numberAssetCode = newOutput[1];
                            // chèn 0 vào giữa chuỗi kí tự với chuỗi số
                            newAssetCode = valueAssetCode + "0" + numberAssetCode;
                        }

                    }
                }
                // nếu chuỗi đầu tiền toàn số trả về true;
                else
                {
                    //  ghép chuỗi lại và thêm 1 ở sau
                    newAssetCode = valueAssetCode + numberAssetCode + 1;
                }

            }
            
            // mảng trả về chỉ có một phần từ
            else
            {
                
                valueAssetCode = output[0];
                // Nếu chuỗi toàn chữ
                checkNumber = IsNumber(valueAssetCode);
                if(checkNumber == false)
                {
                    newAssetCode = valueAssetCode + 1;
                }
                // Nếu chuỗi toàn số
                else
                {
                    newAssetCode = (int.Parse(valueAssetCode) + 1).ToString();
                }
            }

            return newAssetCode;

        }

        /// <summary>
        /// Kiểm tra xem dữ kiệu trả về có phải số hay không
        /// </summary>
        /// <param name="pText"></param>
        /// <returns>true - nếu toàn số, false- nếu toàn chuỗi</returns>
        public bool IsNumber(string pText)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*.?[0-9]+$");
            return regex.IsMatch(pText);
        }
        public List<Asset> GetPadding(string pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Thêm mới dữ liệu vào database
        /// </summary>
        /// <param name="asset"></param>
        /// <returns>Số bản ghi thêm vào database</returns>
        /// Createdby: QuyenNC (11/5/2022)
        public int Insert(Asset asset)
        {

            // thêm mới dữ liệu
            asset.AssetId = Guid.NewGuid();

            var sqlQuery = $"INSERT INTO fixed_asset " +
                $"(AssetID,AssetCode,AssetName,DepartmentId,DepartmentCode,DepartmentName," +
                $"FixedAssetCategoryId,FixedAssetCategoryCode,FixedAssetCategoryName,PurchaseDate,Cost," +
                $"Quantity,DepreciationRate,TrackedYear,LifeTime,CreatedBy)" +
                $" VALUES(@AssetID, @AssetCode, @AssetName, @DepartmentId, @DepartmentCode, @DepartmentName," +
                $" @FixedAssetCategoryId , @FixedAssetCategoryCode,@FixedAssetCategoryName,@PurchaseDate," +
                $"@Cost,@Quantity,@DepreciationRate,@TrackedYear,@LifeTime,@CreatedBy); ";
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
            parameters.Add("@TrackedYear", asset.TrackedYear);
            parameters.Add("@LifeTime", asset.LifeTime);
            parameters.Add("@CreatedBy", asset.CreatedBy);

            //thực hiện thêm dữ liệu
            var res = _sqlConnection.Execute(sqlQuery, param: parameters);
            return res;
        }

        /// <summary>
        /// Cập nhật dữ lệu
        /// </summary>
        /// <param name="assetID"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        /// Createdby: QuyenNC (11/5/2022)
        //public int Update(Asset asset, Guid assetID)
        //{


        //    // cập nhật dữ liệu
        //    //asset.AssetId = Guid.NewGuid();

        //    var sqlQuery = $"UPDATE Asset SET AssetCode = @AssetCode," +
        //        $"AssetName = @AssetName,DepartmentId = @DepartmentId,DepartmentCode = @DepartmentCode," +
        //        $"DepartmentName = @DepartmentName,FixedAssetCategoryId = @FixedAssetCategoryId," +
        //        $"FixedAssetCategoryCode = @FixedAssetCategoryCode,FixedAssetCategoryName = @FixedAssetCategoryName," +
        //        $"PurchaseDate = @PurchaseDate,Cost = @Cost,Quantity = @Quantity,DepreciationRate = @DepreciationRate," +
        //        $"LifeTime = @LifeTime,ModifiedBy = @ModifiedBy,ModifiedDate = @ModifiedDate WHERE AssetID = @AssetID";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@AssetID", assetID.ToString());
        //    parameters.Add("@AssetCode", asset.AssetCode);
        //    parameters.Add("@AssetName", asset.AssetName);
        //    parameters.Add("@DepartmentId", asset.DepartmentId);
        //    parameters.Add("@DepartmentCode", asset.DepartmentCode);
        //    parameters.Add("@DepartmentName", asset.DepartmentName);
        //    parameters.Add("@FixedAssetCategoryId", asset.FixedAssetCategoryId);
        //    parameters.Add("@FixedAssetCategoryCode", asset.FixedAssetCategoryCode);
        //    parameters.Add("@FixedAssetCategoryName", asset.FixedAssetCategoryName);
        //    parameters.Add("@PurchaseDate", asset.PurchaseDate);
        //    parameters.Add("@Cost", asset.Cost);
        //    parameters.Add("@Quantity", asset.Quantity);
        //    parameters.Add("@DepreciationRate", asset.DepreciationRate);
        //    parameters.Add("@LifeTime", asset.LifeTime);
        //    parameters.Add("@ModifiedBy", asset.ModifiedBy);
        //    parameters.Add("@ModifiedDate", asset.ModifiedDate);



        //    //thực hiện thêm dữ liệu
        //    var res = _sqlConnection.Execute(sqlQuery, param: parameters);
        //    return res;
        //}
    }
}
