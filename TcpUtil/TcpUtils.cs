using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TcpUtil
{
    public enum MessageDataType
    {
        Text = 0,
        Json = 1,
        File = 2
    }
    public static class TcpUtils
    {
        public static ContractRecievedData ReadMessageContract(NetworkStream stream, int maxStreamReadBufferSize = 1000)
        {
            // read length of message
            byte[] data = new byte[4];
            stream.Read(data, 0, data.Length);
            int length = BitConverter.ToInt32(data, 0);

            // read type of message
            stream.Read(data, 0, data.Length);
            MessageDataType type = (MessageDataType)BitConverter.ToInt32(data, 4);

            switch (type)
            {
                case MessageDataType.Text:
                    string message = Encoding.UTF8.GetString(ReadMessageBody(stream, length, maxStreamReadBufferSize));
                    return new ContractTextData()
                    {
                        Data = message
                    };
                case MessageDataType.Json:
                    throw new NotImplementedException("Reading Json messages is not implemented.");
                case MessageDataType.File:
                    string name = Encoding.UTF8.GetString(ReadMessageBody(stream, 4, 4));
                    byte[] body = ReadMessageBody(stream, length, maxStreamReadBufferSize);

                    return new ContractFileData()
                    {
                        Name = name,
                        Data = body
                    };
                default:
                    throw new Exception($"Type of {nameof(type)} is not defined.");
            }

            //StringBuilder messageBuilder = new StringBuilder();

            //do
            //{
            //    data = new byte[Math.Min(length, maxStreamReadBufferSize)];
            //    stream.Read(data, 0, data.Length);

            //    messageBuilder.Append(Encoding.UTF8.GetString(data));

            //    length -= data.Length;
            //}
            //while (length > 0);

            //return messageBuilder.ToString();
        }
        private static byte[] ReadMessageBody(Stream stream, int length, int maxStreamReadBufferSize)
        {
            MemoryStream ms = new MemoryStream();
            byte[] data;
            do
            {
                data = new byte[Math.Min(length, maxStreamReadBufferSize)];
                stream.Read(data, 0, data.Length);

                ms.Write(data, 0, data.Length);
                //messageBuilder.Append(Encoding.UTF8.GetString(data));

                length -= data.Length;
            }
            while (length > 0);

            return ms.ToArray();
        }



        public static void SendData(string data, NetworkStream stream, MessageDataType type)
        {
            byte[] bytes = Serialization.Serialize(data, type);
            stream.Write(bytes, 0, bytes.Length);
        }

        [Obsolete("Use \"SendData\" instead.")]
        public static void SendMessage(string message, NetworkStream stream)
        {
            var contract = new MessageContract()
            {
                Message = message
            };
            byte[] data = Serialization.Serialize(contract);

            stream.Write(data, 0, data.Length);
        }
        [Obsolete("Use \"SendData\" instead.")]
        public static void SendFile(string path, NetworkStream stream)
        {
            var contract = new FileContract()
            {
                Path = path
            };
            byte[] data = Serialization.Serialize(contract);

            stream.Write(data, 0, data.Length);
        }
    }
}
