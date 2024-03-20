using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020085.BusinessLayers
{
    public static class Configuration
    {
        /// <summary>
        /// Chuỗi thông số kết nối đến CSDL
        /// </summary>
        public static string ConnectionString { get; private set; } = "";
        /// <summary>
        /// Khởi tạo cấu hình cho BusinessLayer
        /// (Hàm này phải được gọi trước khi ứng dungh  chạu)
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize (string connectionString) {  
            Configuration.ConnectionString = connectionString;
        }
    }
}
//static class là gì ? khác với class ở chỗ nào?