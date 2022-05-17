using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Core.Interfaces.Repositories;
using MISA.Core.Interfaces.Services;
using MISA.Core.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class FixedAssetService :BaseService<Asset>, IFixedAssetService
    {
        IFixedAssetRepository _fixedAssetRepository;
        public FixedAssetService(IFixedAssetRepository fixedAssetRepository):base(fixedAssetRepository)
        {
            _fixedAssetRepository = fixedAssetRepository;
        }

        protected override List<string> ValidateObjectCustom(Asset entity)
        {
            int mode = 1;
            //Xử lý về nghiệp vụ
            //1. Kiểm tra mã tài sản đã có hay chưa -> Chưa có thì báo lỗi.

            //2. Kiểm tra xem đã có tên tài sản chưa -> chưa có báo lỗi.

            // Sau khi kiểm tra các thông tin đã hợp lệ thì thực hiện thêm mới vào database.

            // Trả về kết quả

            //1. Validate dữ liệu: trả về mã 400(BadRequest) kèm các thông tin

            if (string.IsNullOrEmpty(entity.AssetCode))
            {
                ValidateErrorsMsg.Add(Resources.ErrorValidte_AssetCode_NotEmpty);
            }

            if (string.IsNullOrEmpty(entity.AssetName))
            {
                ValidateErrorsMsg.Add(Resources.ErrorValidate_AssetName_NotEmpty);
            }

            if (string.IsNullOrEmpty(entity.DepartmentCode))
            {
                ValidateErrorsMsg.Add(Resources.ErrorValidate_DepartmentCode_NotEmpty);
            }

            if (string.IsNullOrEmpty(entity.FixedAssetCategoryCode))
            {
                ValidateErrorsMsg.Add(Resources.ErrorValidate_FixedAssetCategoryCode_NotEmpty);
            }

            if (string.IsNullOrEmpty(entity.Quantity.ToString()))
            {
                ValidateErrorsMsg.Add(Resources.ErrorValidate_Quantity_NotEmpty);
            }

            if (string.IsNullOrEmpty(entity.Cost.ToString()))
            {
                ValidateErrorsMsg.Add(Resources.ErrorValidate_Cost_NotEmpty);
            }

            if (string.IsNullOrEmpty(entity.LifeTime.ToString()))
            {
                ValidateErrorsMsg.Add(Resources.ErrorValidate_LifeTime_NotEmpty);
            }

            //if (asset.PurchaseDate == DateTime.MinValue)
            //{
            //    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_PurchaseDate_NotEmpty);
            //}

            //Kiểm tra xem mã tài sản đã tồn tại hay chưa?

            //var assetCodeDuplicate = sqlConnection.QueryFirstOrDefault<string>(sqlQueryCheckDuplicateCode, param: asseParam);
            var isDuplicate = _fixedAssetRepository.CheckAssetCodeExist(entity.AssetId, mode, entity.AssetCode);
            if (isDuplicate == true)
            {
                ValidateErrorsMsg.Add(Resources.ErrorValidare_CheckDuplicatAssetCode);
            }

            return ValidateErrorsMsg;
        }
        public int InsertService(Asset asset)

        {
            int mode = 1;
            //Xử lý về nghiệp vụ
            //1. Kiểm tra mã tài sản đã có hay chưa -> Chưa có thì báo lỗi.

            //2. Kiểm tra xem đã có tên tài sản chưa -> chưa có báo lỗi.

            // Sau khi kiểm tra các thông tin đã hợp lệ thì thực hiện thêm mới vào database.

            // Trả về kết quả

            //1. Validate dữ liệu: trả về mã 400(BadRequest) kèm các thông tin
            var validateErrorsMsg = new List<string>();
            if (string.IsNullOrEmpty(asset.AssetCode))
            {
                validateErrorsMsg.Add(Resources.ErrorValidte_AssetCode_NotEmpty);
            }

            if (string.IsNullOrEmpty(asset.AssetName))
            {
                validateErrorsMsg.Add(Resources.ErrorValidate_AssetName_NotEmpty);
            }

            if (string.IsNullOrEmpty(asset.DepartmentCode))
            {
                validateErrorsMsg.Add(Resources.ErrorValidate_DepartmentCode_NotEmpty);
            }

            if (string.IsNullOrEmpty(asset.FixedAssetCategoryCode))
            {
                validateErrorsMsg.Add(Resources.ErrorValidate_FixedAssetCategoryCode_NotEmpty);
            }

            if (string.IsNullOrEmpty(asset.Quantity.ToString()))
            {
                validateErrorsMsg.Add(Resources.ErrorValidate_Quantity_NotEmpty);
            }

            if (string.IsNullOrEmpty(asset.Cost.ToString()))
            {
                validateErrorsMsg.Add(Resources.ErrorValidate_Cost_NotEmpty);
            }

            if (string.IsNullOrEmpty(asset.LifeTime.ToString()))
            {
                validateErrorsMsg.Add(Resources.ErrorValidate_LifeTime_NotEmpty);
            }

            //if (asset.PurchaseDate == DateTime.MinValue)
            //{
            //    validateErrorsMsg.Add(Resource.Resource.ErrorValidate_PurchaseDate_NotEmpty);
            //}

            //Kiểm tra xem mã tài sản đã tồn tại hay chưa?

            //var assetCodeDuplicate = sqlConnection.QueryFirstOrDefault<string>(sqlQueryCheckDuplicateCode, param: asseParam);
            var isDuplicate = _fixedAssetRepository.CheckAssetCodeExist(asset.AssetId, mode, asset.AssetCode);
            if (isDuplicate == true)
            {
                validateErrorsMsg.Add(Resources.ErrorValidare_CheckDuplicatAssetCode);
            }

            if (validateErrorsMsg.Count > 0)
            {
                //var validateError = new ValidateError();
                //validateError.UserMsg = Resources.ErrorValidate;
                //validateError.Data = validateErrorsMsg;
                throw new MISAValidateException(Resources.ErrorValidate, validateErrorsMsg);
            }
            var res = _fixedAssetRepository.Insert(asset);
            return res;
        }

        public int UpdateService(Guid assetID ,Asset asset)
        {
            int mode = 2;
            var validateErrorsMsg = new List<string>();
            if (string.IsNullOrEmpty(asset.AssetCode))
            {
                validateErrorsMsg.Add(Resources.ErrorValidte_AssetCode_NotEmpty);
            }

            if (string.IsNullOrEmpty(asset.AssetName))
            {
                validateErrorsMsg.Add(Resources.ErrorValidate_AssetName_NotEmpty);
            }

            if (string.IsNullOrEmpty(asset.DepartmentCode))
            {
                validateErrorsMsg.Add(Resources.ErrorValidate_DepartmentCode_NotEmpty);
            }

            if (string.IsNullOrEmpty(asset.FixedAssetCategoryCode))
            {
                validateErrorsMsg.Add(Resources.ErrorValidate_FixedAssetCategoryCode_NotEmpty);
            }

            if (string.IsNullOrEmpty(asset.Quantity.ToString()))
            {
                validateErrorsMsg.Add(Resources.ErrorValidate_Quantity_NotEmpty);
            }

            if (string.IsNullOrEmpty(asset.Cost.ToString()))
            {
                validateErrorsMsg.Add(Resources.ErrorValidate_Cost_NotEmpty);
            }

            if (string.IsNullOrEmpty(asset.LifeTime.ToString()))
            {
                validateErrorsMsg.Add(Resources.ErrorValidate_LifeTime_NotEmpty);
            }

            var isDuplicate = _fixedAssetRepository.CheckAssetCodeExist(assetID, mode ,asset.AssetCode);
            if (isDuplicate == true)
            {
                validateErrorsMsg.Add(Resources.ErrorValidare_CheckDuplicatAssetCode);
            }

            if (validateErrorsMsg.Count > 0)
            {
                //var validateError = new ValidateError();
                //validateError.UserMsg = Resources.ErrorValidate;
                //validateError.Data = validateErrorsMsg;
                throw new MISAValidateException(Resources.ErrorValidate, validateErrorsMsg);

            }
            var res = _fixedAssetRepository.Update(asset, assetID);
            return res;
        }
    }
}
