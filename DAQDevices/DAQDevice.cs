using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAQDevices
{
    public abstract class DAQDevice
    {
        public abstract Task SendAsync();
        public abstract Task<string> ReadParamAsync(string paramName);
        public abstract Task<string[]> ReadAllParamsAsync();

        /// <summary>
        /// CRC8 method for checking data integriy
        /// </summary>
        /// <param name="data"></param>
        /// <param name="polynom"></param>
        /// <returns></returns>
        internal byte CRC8(byte[] data, byte polynom = 0xD5)
        {
            byte crc = 0;
            for (int i = 0; i < data.Length; i++)
            {
                crc ^= data[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x80) != 0)
                    {
                        crc = (byte)((crc << 1) ^ polynom);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }
            return crc;
        }        

    }
}
