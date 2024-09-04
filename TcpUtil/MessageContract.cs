using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUtil
{
    public class MessageContractHeader
    {
        public int Length { get; internal set; }
    }

    public class MessageContract
    {
        public MessageContractHeader Header;
        public string Message { get; set; }
    }
}
