using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Infrastruct.EF
{
    public class SaveResult
    {
        ///// <summary>
        ///// 是否成功
        ///// </summary>
        //public bool Success { get; set; }

        public SaveResult()
        {
            this.Status = SaveStatus.Unsaved;
        }


        /// <summary>
        /// 操作结果
        /// </summary>
        public SaveStatus Status { get; set; }
        /// <summary>
        /// 受影响的数据行数
        /// </summary>
        public int Rows { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }
    }

    public enum SaveStatus
    {
        /// <summary>
        /// 未保存
        /// </summary>
        Unsaved = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Success =1,
        /// <summary>
        /// 不存在
        /// </summary>
        NonExist=2,
        /// <summary>
        /// 没有任何记录受影响
        /// </summary>
        NoImpact=3,
        /// <summary>
        /// 操作失败,报错
        /// </summary>
        Error=4
    }
}
