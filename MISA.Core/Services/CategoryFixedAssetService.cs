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
    public class CategoryFixedAssetService :BaseService<FixedAssetCategory>,ICategoryFixedAssetService
    {
        ICategoryFixedAssetRepository _categoryFixedAssetRepository;
        public CategoryFixedAssetService(ICategoryFixedAssetRepository categoryFixedAssetRepository) :base(categoryFixedAssetRepository)
        {
           
        }

        public int InsertService(FixedAssetCategory entity)
        {
            throw new NotImplementedException();
        }

        public int UpdateService(FixedAssetCategory entity, Guid entityID)
        {
            throw new NotImplementedException();
        }
    }
}
