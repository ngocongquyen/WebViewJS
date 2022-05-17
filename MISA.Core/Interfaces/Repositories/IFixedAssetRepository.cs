
using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repositories
{
    public interface IFixedAssetRepository:IBaseRepository<Asset>
    {
     
    
        List<Asset> GetPadding(string pageIndex, int pageSize);

        string GetNewAssetCode();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetCode"></param>
        /// <returns>- true - đã tồn tại false - chưa tồn tại</returns>
        bool CheckAssetCodeExist(Guid assetID , int mode ,string assetCode);
    }
}
