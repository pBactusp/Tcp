using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpUtil
{
    public static class TcpUtils
    {
        public static string ReadMessageContract(NetworkStream stream, int maxStreamReadBufferSize = 1000)
        {
            byte[] data = new byte[4];
            stream.Read(data, 0, data.Length);

            int length = BitConverter.ToInt32(data, 0);

            StringBuilder messageBuilder = new StringBuilder();

            do
            {
                data = new byte[Math.Min(length, maxStreamReadBufferSize)];
                stream.Read(data, 0, data.Length);

                messageBuilder.Append(Encoding.UTF8.GetString(data));

                length -= data.Length;
            }
            while (length > 0);

            return messageBuilder.ToString();
        }

        public static void SendMessage(string message, NetworkStream stream)
        {
            var contract = new MessageContract()
            {
                Message = message
            };
            byte[] data = Serialization.Serialize(contract);

            stream.Write(data, 0, data.Length);
        }
    }
}
