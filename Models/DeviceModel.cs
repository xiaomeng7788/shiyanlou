using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace productMonitor.Models
{
    /// <summary>
    /// 设备监控名称
    /// </summary>
    internal class DeviceModel
    {
        public string DeviceItem { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public double Value { get; set; }
    }
}
