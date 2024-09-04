using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUtil
{
    public abstract class ContractRecievedData
    {
    }
    public class ContractTextData : ContractRecievedData
    {
        public string Data { get; internal set; }
    }

    public class ContractFileData : ContractRecievedData
    {
        public byte[] Data { get; internal set; }
        public string Name { get; internal set; }
}
