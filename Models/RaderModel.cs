﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace productMonitor.Models
{
    /// <summary>
    /// 雷达图数据模型
    /// </summary>
    public class RaderModel
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public double Value { get; set; }
    }
}
