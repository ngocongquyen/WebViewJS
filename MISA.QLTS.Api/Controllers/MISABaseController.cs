using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Core.Interfaces.Repositories;
using MISA.Core.Interfaces.Services;
using MISA.Core.Resource;

namespace MISA.QLTS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public abstract class MISABaseController<T> : ControllerBase
    {
        IBaseService<T> _baseService;
        IBaseRepository<T> _baseRepository;

        public MISABaseController(IBaseService<T> baseService, IBaseRepository<T> baseRepository)
        {
            _baseService = baseService;
            _baseRepository = baseRepository;
        }

        /// <summary>
        /// Lấy tất cả dữ liệu
        /// </summary>
        /// <returns>danh sách các dữ liệu</returns>
        /// Createdby: QuyenNC (11/5/2022)

        [HttpGet]
        public IActionResult GetData()
        {
            try
            {
                var entities = _baseRepository.Get();
                return Ok(entities);
            }
            catch (Exception ex)
            {

                return HanleException(ex);
            }
        }

        /// <summary>
        /// Lấy dữ liệu theo assetID
        /// </summary>
        /// <param name="assetID"></param>
        /// <returns></returns>
        /// Createdby: QuyenNC (11/5/2022)

        [HttpGet("{entityId}")]
        public IActionResult GetDataByID(Guid entityId)
        {
            try
            {
                var entity = _baseRepository.GetById(entityId);
                return Ok(entity);
            }
            catch (Exception ex)
            {

                return HanleException(ex);
            }
        }

        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        /// Createdby: QuyenNC (11/5/2022)
        [HttpPost]
        public IActionResult PostData(T entity)
        {
            try
            {
                var res = _baseService.InsertService(entity);
                return StatusCode(201, res);
            }
            catch (Exception ex)
            {

                return HanleException(ex);
            }
        }

        /// <summary>
        /// Chỉnh sửa dữ liệu
        /// </summary>
        /// <param name="assetID"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        /// Createdby: QuyenNC (11/5/2022)

        [HttpPut("{entityId}")] 
        public IActionResult PutData(T entity, Guid entityId)
        {
            try
            {
                var res = _baseService.UpdateService(entity, entityId);
                return StatusCode(200, res);
            }
            catch (Exception ex)
            {

                return HanleException(ex);
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="assetID"></param>
        /// <returns></returns>
        /// Createdby: QuyenNC (11/5/2022)
        [HttpDelete("{entityId}")]

        public IActionResult DeleteDate(Guid entityId)
        {
            try
            {
                var res = _baseRepository.Delete(entityId);
                return StatusCode(200, res);
            }
            catch (Exception ex)
            {
                return HanleException(ex);
                throw;
            }
        }
        /// <summary>
        /// Xử lý lỗi
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>500 - lỗi server 400 - lỗi client</returns>
        /// Createdby: QuyenNC (11/5/2022)
        public IActionResult HanleException(Exception ex)
        {
            var error = new ValidateError();
            error.DevMsg = ex.Message;
            error.UserMsg = Resources.Error_Exception;
            error.Data = ex.Data;
            if (ex is MISAValidateException)
            {
                return StatusCode(400, error);
            }
            else
            {
                return StatusCode(500, error);
            }

        }
    }
}
