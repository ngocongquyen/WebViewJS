using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Core.Interfaces.Repositories;
using MISA.Core.Interfaces.Services;
using MISA.Core.MISAAttribute;
using MISA.Core.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class BaseService<T> :IBaseService<T>
    {
        IBaseRepository<T> _baseRepository;
        protected List<string> ValidateErrorsMsg;
        public BaseService(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
            ValidateErrorsMsg = new List<string>();
        }
        public int InsertService(T entity)
        {
            return _baseRepository.Insert(entity);
            // Xử lý validate
            //var isValid = ValidateObject(entity);



            //if (isValid ==true &&(ValidateErrorsMsg == null || ValidateErrorsMsg.Count()==0))
            //{
            //    return _baseRepository.Insert(entity);
            //}

            //var validateError = new ValidateError();
            //validateError.UserMsg = Resources.ErrorValidate;
            //validateError.Data = ValidateErrorsMsg;
            //throw new MISAValidateException(Resources.ErrorValidate, ValidateErrorsMsg);
        }

        public int UpdateService(T entity, Guid entityID)
        {
            return _baseRepository.Update(entity, entityID);
        }

        /// <summary>
        /// Thực hiện validate dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng cần validate</param>
        /// <returns>List string các lỗi validate</returns>
        /// CreatedBy: NCQUYEN(15/5/2022)
        protected virtual List<string> ValidateObjectCustom(T entity)
        {
            return null;
        }

        /// <summary>
        /// Validate chung
        /// </summary>
        /// <param name="enity">Đối tượng cần validate</param>
        /// <returns>true - nếu dữ liệu đã hợp lệ; falase - dữ liệu không hợp lệ</returns>
        /// CreatedBy: NCQUYEN(15/5/2022)
        private bool ValidateObject(T entity)
        {
            var isValid = true;
           
            var properties = typeof(T).GetProperties();
            foreach (var prop in properties)
            {
                // Tên của prop:
                var propName = prop.Name;
                var propFriendlyName = propName;
                // Giá trị của prop:
                var propValue = prop.GetValue(entity);
                // Kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                // Kiểm tra xem prop hiện tại có gán attribute PropertyNameFriendly hay không:
                var isFriendlyName = prop.IsDefined(typeof(PropertyNameFriendly), true);
                if(isFriendlyName)
                {
                    // Lấy ra tên hiện thị của prop
                     propFriendlyName = (prop.GetCustomAttributes(typeof(PropertyNameFriendly), true)[0] as PropertyNameFriendly).Name;
                }

                var isNotNullOrEmpty = prop.IsDefined(typeof(IsNotNullOrEmpty), true);
                //1. Thông tin bắt buộc nhập:
                if(isNotNullOrEmpty == true && (propValue == null || propValue.ToString() == ""))
                {
                    isValid = false;        
                    ValidateErrorsMsg.Add(string.Format(Resources.ErrorValidate_PropertyNotNull, propFriendlyName));
                    
                    
                }
                // 2. Các thông tin là chuỗi có yêu cầu giới hạn về độ dài(VD: Mã tài sản không được vượt quá 20 ký tự)

                var isMaxLength = prop.IsDefined(typeof(MaxLength), true);
                if(isMaxLength)
                {
                    // Lấy ra maxLength
                     var maxLength = (prop.GetCustomAttributes(typeof(MaxLength), true)[0] as MaxLength).Length;
                    if(propValue.ToString().Length > maxLength)
                    {
                        isValid = false;
                        ValidateErrorsMsg.Add(string.Format(Resources.ErrorValidate_PropertyMaxLength, propFriendlyName,maxLength));
                    }
                }
                // 3. Ngày tháng không được vượt quá ngày hiện tại
                
            }
            // thực hiện validate đặc thù cho từng đối tượng khác nhau:
            ValidateObjectCustom(entity);
            return isValid; ;

        }
    }
}
