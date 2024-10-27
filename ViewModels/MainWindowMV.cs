﻿using productMonitor.Models;
using productMonitor.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace productMonitor.ViewModels
{
    internal class MainWindoVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 视图模型构造函数
        /// </summary>
        public MainWindoVM()
        {
            #region 初始化环境监控数据
            /// <summary>
            ///初始化环境监控数据
            /// </summary>
            EnviromentList = new List<EnviromentModel>();

            EnviromentList.Add(new EnviromentModel { EnItemName = "光照(Lux)", EnItemValue = 123 });
            EnviromentList.Add(new EnviromentModel { EnItemName = "噪音(db)", EnItemValue = 55 });
            EnviromentList.Add(new EnviromentModel { EnItemName = "温度(℃)", EnItemValue = 80 });
            EnviromentList.Add(new EnviromentModel { EnItemName = "湿度(%)", EnItemValue = 43 });
            EnviromentList.Add(new EnviromentModel { EnItemName = "PM2.5(m³)", EnItemValue = 20 });
            EnviromentList.Add(new EnviromentModel { EnItemName = "硫化氢(PPM)", EnItemValue = 15 });
            EnviromentList.Add(new EnviromentModel { EnItemName = "氮气(PPM)", EnItemValue = 18 });
            #endregion 初始化环境监控数据

            #region 初始化报警列表
            AlarmList = new List<AlarmModel>();
            AlarmList.Add(new AlarmModel { Num = "01", Msg = "设备温度过高", Time = "2023-11-23 18:34:56", Duration = 7 });
            AlarmList.Add(new AlarmModel { Num = "02", Msg = "车间温度过高", Time = "2023-12-08 20:40:59", Duration = 10 });
            AlarmList.Add(new AlarmModel { Num = "03", Msg = "设备转速过快", Time = "2024-01-05 12:24:34", Duration = 12 });
            AlarmList.Add(new AlarmModel { Num = "04", Msg = "设备气压偏低", Time = "2024-02-04 19:58:00", Duration = 90 });
            #endregion

            #region 初始化设备监控
            DeviceList = new List<DeviceModel>();
            DeviceList.Add(new DeviceModel { DeviceItem = "电能(Kw.h)", Value = 60.8 });
            DeviceList.Add(new DeviceModel { DeviceItem = "电压(V)", Value = 390 });
            DeviceList.Add(new DeviceModel { DeviceItem = "电流(A)", Value = 5 });
            DeviceList.Add(new DeviceModel { DeviceItem = "压差(kpa)", Value = 13 });
            DeviceList.Add(new DeviceModel { DeviceItem = "温度(℃)", Value = 36 });
            DeviceList.Add(new DeviceModel { DeviceItem = "振动(mm/s)", Value = 4.1 });
            DeviceList.Add(new DeviceModel { DeviceItem = "转速(r/min)", Value = 2600 });
            DeviceList.Add(new DeviceModel { DeviceItem = "气压(kpa)", Value = 0.5 });
            #endregion

            #region 初始化雷达数据 
            RaderList = new List<RaderModel>();

            RaderList.Add(new RaderModel { ItemName = "排烟风机", Value = 90 });
            RaderList.Add(new RaderModel { ItemName = "客梯", Value = 30.00 });
            RaderList.Add(new RaderModel { ItemName = "供水机", Value = 34.89 });
            RaderList.Add(new RaderModel { ItemName = "喷淋水泵", Value = 69.59 });
            RaderList.Add(new RaderModel { ItemName = "稳压设备", Value = 20 });

            #endregion

            #region 初始化缺岗员工数据
            StuffOutWorkList = new List<StuffOutWorkModel>();
            StuffOutWorkList.Add(new StuffOutWorkModel { StuffName = "张晓婷", Position = "技术员", OutWorkCount = 123 });
            StuffOutWorkList.Add(new StuffOutWorkModel { StuffName = "李晓", Position = "操作员", OutWorkCount = 23 });
            StuffOutWorkList.Add(new StuffOutWorkModel { StuffName = "王克俭", Position = "技术员", OutWorkCount = 134 });
            StuffOutWorkList.Add(new StuffOutWorkModel { StuffName = "陈家栋", Position = "统计员", OutWorkCount = 143 });
            StuffOutWorkList.Add(new StuffOutWorkModel { StuffName = "杨过", Position = "技术员", OutWorkCount = 12 });

            #endregion

            #region 初始化车间列表
            WorkShopList = new List<WorkShopModel>();
            WorkShopList.Add(new WorkShopModel { WorkShopName = "贴片车间", WorkingCount = 32, WaitCount = 8, WrongCount = 4, StopCount = 0 });
            WorkShopList.Add(new WorkShopModel { WorkShopName = "封装车间", WorkingCount = 20, WaitCount = 8, WrongCount = 4, StopCount = 0 });
            WorkShopList.Add(new WorkShopModel { WorkShopName = "焊接车间", WorkingCount = 68, WaitCount = 8, WrongCount = 4, StopCount = 0 });
            WorkShopList.Add(new WorkShopModel { WorkShopName = "贴片车间", WorkingCount = 68, WaitCount = 8, WrongCount = 4, StopCount = 0 });
            #endregion

            #region 初始化机台列表
            MachineList = new List<MachineModel>();
            Random random = new Random();
            for(int i = 0;i<20;i++)
            {
                int plan = random.Next(100, 1000);//计划量 随机数
                int finished = random.Next(0, plan);//已完成量
                MachineList.Add(new MachineModel
                {
                    MachineName = "焊接机-" + (i + 1),
                    FinishedCount = finished,
                    PlanCount = plan,
                    Status = "工作中",
                    OrderNo = "H202410130001"
                });
            }
            #endregion

            //从设备读取数据(异步) 如果您没有学习到上位机，该区域代码报错，直接注释该区域代码
            #region 从设备读取环境数据
            Task.Run(() =>
                      {
                          while (true)
                          {
                              using (SerialPort serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One))
                              {
                                  serialPort.Open();
                                  Modbus.Device.IModbusSerialMaster master = Modbus.Device.ModbusSerialMaster.CreateRtu(serialPort);

                                  //功能码03
                                  ushort[] value = master.ReadHoldingRegisters(1, 0, 7);//从设备地址，寄存器起始地址，寄存器个数

                                  for (int i = 0; i < 7; i++)
                                  {
                                      EnviromentList[i].EnItemValue = value[i];
                                  }
                              }
                          }
                      });
            #endregion


        }


        #region 监控用户控件

        ///<summary>
        ///监控用户控件
        ///</summary>
        private UserControl _MonitorUC;
        //_MonitorUC 存储了 MonitorUC 属性的实例。在属性的 getter 方法中，
        //如果 _MonitorUC 是 null，则会创建一个新的 MonitorUC 实例并将其赋值给 _MonitorUC，然后返回该实例。

        public UserControl MonitorUC
        {
            get
            {
                if (_MonitorUC == null)//懒加载：如果是 null，懒加载：MonitorUC 只在第一次访问时创建，避免不必要的开销。
                {
                    _MonitorUC = new MonitorUC();
                }
                return _MonitorUC;
            }
            set
            {
                _MonitorUC = value;
                if (PropertyChanged != null)//事件改变不为空时
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MonitorUC"));///通知绑定到该属性的 UI 组件（例如 WPF 控件）进行更新。
                }
            }
        }
        #endregion

        #region 时间
        ///<summary>
        ///时间
        ///</summary>
        public string TimStr
        {
            get
            {
                return DateTime.Now.ToString("HH:mm");
            }
        }

        ///<summary>
        ///年月日
        ///</summary>
        public string DateStr
        {
            get
            {
                return DateTime.Now.ToString("yyy-MM-dd");
            }
        }

        ///<summary>
        ///星期
        ///</summary>
        public string WeekStr
        {
            get
            {
                int index = (int)DateTime.Now.DayOfWeek;

                string[] week = new string[7] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

                return week[index];
            }
        }
        #endregion

        #region 计数
        ///<summary>
        ///机台数
        ///</summary>
        private string _MachineCount = "0298";

        public string MachineCount
        {
            get { return _MachineCount; }
            set
            {
                _MachineCount = value;
                if (PropertyChanged != null)//事件改变不为空时
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MachineCount"));///通知绑定到该属性的 UI 组件（例如 WPF 控件）进行更新。
                }
            }
        }

        ///<summary>
        ///生产计数
        ///</summary>
        private string _ProductCount = "1643";

        public string ProductCount
        {
            get { return _ProductCount; }
            set
            {
                _ProductCount = value;
                if (PropertyChanged != null)//事件改变不为空时
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ProductCount"));///通知绑定到该属性的 UI 组件（例如 WPF 控件）进行更新。
                }
            }
        }

        ///<summary>
        ///不良计数
        ///</summary>
        private string _BadCount = "023";

        public string BadCount
        {
            get { return _BadCount; }
            set
            {
                _BadCount = value;
                if (PropertyChanged != null)//事件改变不为空时
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("BadCount"));///通知绑定到该属性的 UI 组件（例如 WPF 控件）进行更新。
                }
            }
        }
        #endregion

        #region  环境监控数据
        private List<EnviromentModel> _EnviromentList;

        /// <summary>
        /// 环境监控数据
        /// </summary>
        public List<EnviromentModel> EnviromentList
        {
            get { return _EnviromentList; }
            set
            {
                _EnviromentList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("EnviromentList"));
                }
            }
        }
        #endregion

        #region 报警属性
        private List<AlarmModel> _AlarmList;
        /// <summary>
        /// 报警集合
        /// </summary>
        public List<AlarmModel> AlarmList
        {
            get { return _AlarmList; }
            set
            {
                _AlarmList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AlarmList"));
                }
            }
        }
        #endregion

        #region 设备集合属性
        private List<DeviceModel> _DeviceList;

        public List<DeviceModel> DeviceList
        {
            get { return _DeviceList; }
            set
            {
                _DeviceList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DeviceList"));
                }
            }
        }
        #endregion

        #region 雷达数据属性
        private List<RaderModel> _RaderList;

        public List<RaderModel> RaderList
        {
            get { return _RaderList; }
            set
            {
                _RaderList = value;

            }
        }
            #endregion

        #region 缺岗员工数据属性
        private List<StuffOutWorkModel> _StuffOutWorkList;
        //// <summary>
        /// 缺岗员工
        /// </summary>
        public List<StuffOutWorkModel> StuffOutWorkList
        {
            get { return _StuffOutWorkList; }
            set
            {
                _StuffOutWorkList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("StuffOutWorkList"));
                }
            }
        }
        #endregion

        #region 缺岗属性
        private List<WorkShopModel> _WorkShopList;
        public List<WorkShopModel> WorkShopList
        {
            get { return _WorkShopList; }
            set { _WorkShopList = value; }
        }
        #endregion

        #region 机台集合属性
        ///<summary>
        ///机台集合属性
        /// </summary>
        private List<MachineModel> _MachineList;//定义了一个私有字段 _MachineList，它的类型是 List<MachineModel>，用于存储机台的集合

        public List<MachineModel> MachineList//MachineList是一个公共的属性，用于访问和设置 _MachineList。属性包含 get 和 set 方法：
        {
            get { return _MachineList; }
            set
            {
                _MachineList = value;
                if(PropertyChanged!=null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MachineList"));
                }
            }
        }
        #endregion
    }
}