using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUtil
{
    public class ContractHeader
    {
        public int Length { get; internal set; }
        public MessageDataType Type { get; internal set; }
    }

    public abstract class Contract
    {
        public ContractHeader Header;
    }

    public class MessageContract : Contract
    {
        public string Message { get; set; }
    }

    public class FileContract : Contract
    {
        public string Path { get; set; }
    }
    
}
