using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Core.Interfaces.Repositories;
using MISA.Core.MISAAttribute;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infastructure.Repository
{
    public class BaseRepository<T>:IBaseRepository<T>
    {
        IConfiguration _configuration;
        readonly string _connectionString = string.Empty;
        protected MySqlConnection _sqlConnection;
        string _tableName;
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("NCQUYEN");
            _sqlConnection = new MySqlConnection(_connectionString);
            _tableName = typeof(T).Name;
        }

        public List<T> Get()
        {
            // khai báo câu lệnh truy vấn SQL :
            var sqlCommand = $"SELECT * FROM {_tableName} ORDER BY CreatedDate DESC";
            var entities = _sqlConnection.Query<T>(sql: sqlCommand);
            return entities.ToList();
        }

        /// <summary>
        /// Thêm mới dữ liệu vào database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Số bản ghi thêm vào database</returns>
        /// Createdby: QuyenNC (13/5/2022)
        public int Insert(T entity)
        {
            // khai báo câu lệnh SQL thực hiện thêm mới;
            // khai báo string các cột dữ liệu của table:
            var columnNames = "";
            var columnParams = "";
          
            // Lấy ra tất cả các properties của class:
            var properties = typeof(T).GetProperties();
            foreach(var prop in properties)
            {
                // Tên của prop:
                var propName = prop.Name;
                // Giá trị của prop:
                var propValue = prop.GetValue(entity);
                // Kiểu dữ liệu của prop:
                var propType = prop.PropertyType;
                // Kiểm tra prop hiện tại có phải là khóa chính hay không, nếu đúng thì gán lại giá trị mới cjo prop
                var isPrimaryKey = prop.IsDefined(typeof(PrimaryKey), true);
                if(isPrimaryKey == true && propType == typeof(Guid))
                {
                    prop.SetValue(entity, Guid.NewGuid());
                }
                
                //if(propName == $"{_tableName}Id" && propType == typeof(Guid))
                //{
                //    prop.SetValue(entity, Guid.NewGuid());
                //}
                // Bổ sung cột hiện tại vào chuỗi câu truy vấn cột dữ liệu:
                columnNames += $"{propName},";
                columnParams += $"@{propName},";
            }
            columnNames = columnNames.Remove(columnNames.Length - 1, 1);
            columnParams = columnParams.Remove(columnParams.Length - 1, 1);
            var sqlCommand = $"INSERT INTO {_tableName}({columnNames}) VALUES ({columnParams})";
            var rowAffects = _sqlConnection.Execute(sqlCommand, param: entity);
            return rowAffects;
            
        }

        public int Update(T entity, Guid entityID)
        {
            // khai báo câu lệnh SQL thực hiện thêm mới;
            // khai báo string các cột dữ liệu của table:
            var columnNames = "";

            // Lấy ra tất cả các properties của class:
            var properties = typeof(T).GetProperties();
            var parameters = new DynamicParameters();
            foreach (var prop in properties)
            {
                // Tên của prop:
                var propName = prop.Name;
                // Giá trị của prop:
                var propValue = prop.GetValue(entity);
                // Kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                // Bổ sung cột hiện tại vào chuỗi câu truy vấn cột dữ liệu:
                columnNames += $"{propName} = @{propName},";

                parameters.Add($"@{propName}", propValue);
            }
            parameters.Add("@entityID", entityID.ToString());
            columnNames = columnNames.Remove(columnNames.Length - 1, 1);

            var sqlCommand = $"UPDATE {_tableName} SET {columnNames} WHERE {_tableName}Id = @entityID";

            var rowAffects = _sqlConnection.Execute(sqlCommand, param: parameters);
            return rowAffects;
        
        }

        public int Delete(Guid entityID)
        {
            //// Câu lệnh sql
            //var sqlQuery = $"DELETE FROM Asset WHERE AssetID = @AssetID";
            ////khai báo parameter dạng động
            //var parameters = new DynamicParameters();
            ////Gán giá trị truyền vào từ client vào param
            //parameters.Add("@AssetID", assetID.ToString());
            // thực hiện câu lệnh truy vấn
            //var res = _sqlConnection.Execute(sqlQuery, param: parameters);
            //thực hiện thêm dữ liệu
            var sqlCommand = $"DELETE FROM {_tableName} WHERE {_tableName}Id = @entityID";
            var parameters = new DynamicParameters();
            parameters.Add("@entityID", entityID.ToString());
            var res = _sqlConnection.Execute(sql:sqlCommand, param:parameters);
            return res;
        }

        public object GetById(Guid entityID)
        {
            var sqlCommand = $"SELECT * FROM {_tableName} WHERE {_tableName}Id = @entityID";
            var parameters = new DynamicParameters();
            parameters.Add("@entityID", entityID.ToString());
            var res = _sqlConnection.QueryFirstOrDefault<T>(sql:sqlCommand, param:parameters);
            return res;
        }
    }
}
