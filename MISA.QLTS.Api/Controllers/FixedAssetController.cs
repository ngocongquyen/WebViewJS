using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Core.Interfaces.Repositories;
using MISA.Core.Interfaces.Services;
using MISA.Core.Resource;
using MISA.Infastructure.Repository;

namespace MISA.QLTS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FixedAssetController : MISABaseController<Asset>
    {
      
        IFixedAssetRepository _fixedAssetRepository;
        IFixedAssetService _fixedAssetService;
        public FixedAssetController( IFixedAssetRepository fixedAssetRepository, IFixedAssetService fixedAssetService):base(fixedAssetService, fixedAssetRepository)
        {
            _fixedAssetRepository = fixedAssetRepository;

        }

        [HttpGet("newAssetCode")]
        public IActionResult GetNewAssetCode()
        {
            try
            {
                var newAssetCode = _fixedAssetRepository.GetNewAssetCode();
                return Ok(newAssetCode);
            }
            catch (Exception ex)
            {

                return HanleException(ex);
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
