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
    public class CategoryFixedAssetController : MISABaseController<FixedAssetCategory>
    {
       
        ICategoryFixedAssetRepository _categoryFixedAssetRepository;
        ICategoryFixedAssetService _categoryFixedAssetService;
        public CategoryFixedAssetController(ICategoryFixedAssetRepository categoryFixedAssetRepository, ICategoryFixedAssetService categoryFixedAssetService) :base(categoryFixedAssetService, categoryFixedAssetRepository)
        {
           
            _categoryFixedAssetRepository = categoryFixedAssetRepository;
            _categoryFixedAssetService = categoryFixedAssetService;
           
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
