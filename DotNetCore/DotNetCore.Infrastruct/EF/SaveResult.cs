using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Infrastruct.EF
{
   public class SaveResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 受影响的数据行数
        /// </summary>
        public int Rows { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }
    }
}
