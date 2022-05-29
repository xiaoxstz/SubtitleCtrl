using System;
using System.Runtime.InteropServices;
using System.Text;
using Serilog;

namespace Tanzi.File
{
    /// <summary>
    /// INI文件读写
    /// </summary>
    public class Ini
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);

        private string IniPath;

        public Ini(string iniPath)
        {
            this.IniPath = iniPath;
        }

        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="Section">分组节点</param>
        /// <param name="Key">关键字</param>
        /// <param name="Value">值</param>
        /// <returns>if succeed,returns true</returns>
        public bool WriteValue(string Section, string Key, string Value)
        {
            long returnCode = WritePrivateProfileString(Section, Key, Value, this.IniPath);
            bool bSucceed = returnCode == 0 ? false : true;
            if (!bSucceed)
                Log.Error($"Failed to write '{Section}-{Key}' to file '{this.IniPath}'");
            return bSucceed;
        }


        public bool WriteValue(string Section, string Key, int nValue)
        {
            string strValue = nValue.ToString();
            return this.WriteValue(Section, Key, strValue);
        }

        public bool WriteValue(string Section, string Key, double dbValue)
        {
            string strValue = dbValue.ToString("F4");
            return this.WriteValue(Section, Key, strValue);
        }

        public bool WriteValue(string Section, string Key, bool bValue)
        {
            string strValue = "1";
            strValue = bValue ? "1" : "0";
            return this.WriteValue(Section, Key, strValue);
        }

        /// <summary>
        ///  读取INI文件
        /// </summary>
        /// <param name="section">分组节点</param>
        /// <param name="key">关键字</param>
        /// <param name="bytes">读取的内容</param>
        /// <returns>byte数组</returns>
        /// <returns>if succeed,returns true</returns>
		public bool ReadBytes(string section, string key, out byte[] bytes)
        {
            bytes = new byte[255];
            int returnCode = GetPrivateProfileString(section, key, "", bytes, 255, this.IniPath);
            bool bSucceed = returnCode == 0x0 ? false : true; // if succeed,it's 0x4;if not,it's 0x0.
            if (!bSucceed)
                Log.Error($"Failed to read '{section}-{key}' from file '{this.IniPath}'");
            return true;
        }

        /// <summary>
        /// 读ini字符串数据带默认值
        /// </summary>
        /// <param name="section">区</param>
        /// <param name="key">关键字</param>
        /// <param name="strValue">值</param>
        /// <param name="strDefault">默认值</param>
        /// <returns>if succeed,returns true</returns>
        public bool ReadValue(string section, string key, out string strValue, string strDefault)
        {
            System.Text.StringBuilder temp = new System.Text.StringBuilder(255);
            int returnCode = GetPrivateProfileString(section, key, strDefault, temp, 255, this.IniPath);
            bool bSucceed = returnCode == 0x0 ? false : true; // if succeed,it's 0x4;if not,it's 0x0.
            strValue = temp.ToString();

            if (!bSucceed)
                Log.Error($"Failed to read '{section}-{key}' from file '{this.IniPath}'");
            return bSucceed;
        }

        /// <summary>
        /// 读ini整形数据带默认值
        /// </summary>
        /// <param name="section">区</param>
        /// <param name="key">关键字</param>
        /// <param name="nValue">值</param>
        /// <param name="nDefault">默认值</param>
        /// <returns>if succeed,returns true</returns>
        public bool ReadValue(string section, string key, out int nValue, int nDefault)
        {
            nValue = nDefault;
            string strDefault = "";
            bool bSucceed = false;
            if (this.ReadValue(section, key, out string strRlt, strDefault))
            {
                if (int.TryParse(strRlt, out nValue))
                    bSucceed = true;
                else
                    Log.Error($"Failed to convert '{strRlt}' into int");
            }
            return bSucceed;
        }
        /// <summary>
        /// 读ini double形数据带默认值
        /// </summary>
        /// <param name="section">区</param>
        /// <param name="key">关键字</param>
        /// <param name="dbValue">值</param>
        /// <param name="dbDefault">默认值</param>
        /// <returns>if succeed,returns true</returns>
        public bool ReadValue(string section, string key, out double dbValue, double dbDefault)
        {
            dbValue = dbDefault;
            bool bSucceed = false;
            string strDefault = "";
            if (this.ReadValue(section, key, out string strRlt, strDefault))
            {
                if (double.TryParse(strRlt, out dbValue))
                    bSucceed = true;
                else
                    Log.Error($"Failed to convert '{strRlt}' into double");
            }
            return bSucceed;
        }

        /// <summary>
        /// 读ini bool形数据带默认值
        /// </summary>
        /// <param name="section">区</param>
        /// <param name="key">关键字</param>
        /// <param name="bValue">值</param>
        /// <param name="bDefault">默认值</param>
        /// <returns>if succeed,returns true</returns>
        public bool ReadValue(string section, string key, ref bool bValue, bool bDefault)
        {
            bValue = bDefault;
            string strDefault = "";
            bool bSucceed = false;
            if (this.ReadValue(section, key, out string strRlt, strDefault))
            {
                if (strRlt == "1" || strRlt == "0")
                {
                    bValue = strRlt == "1" ? true : false;
                    bSucceed = true;
                }
                else if (bool.TryParse(strRlt, out bValue))
                    bSucceed = true;
                else
                    Log.Error($"Failed to convert '{strRlt}' into bool");
            }
            return bSucceed;
        }

        /// <summary>
        /// 删除ini文件下指定段落下的所有键
        /// </summary>
        /// <param name="Section"></param>
        /// <returns>if succeed,returns true</returns>
        public bool ClearSection(string Section)
        {
            return WriteValue(Section, null, null);
        }
    }
}