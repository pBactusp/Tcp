using System.ComponentModel;
using System.Text;

namespace TcpUtil
{
    public static class Serialization
    {
        public static byte[] Serialize(MessageContract contract)
        {
            //string message = contract.Message;

            if (string.IsNullOrEmpty(contract.Message))
                throw new ArgumentNullException($"{nameof(contract)}.{nameof(contract.Message)} must have a value.");

            byte[] messageBytes = Encoding.UTF8.GetBytes(contract.Message);
            contract.Header = new MessageContractHeader()
            {
                Length = messageBytes.Length
            };

            using MemoryStream ms = new MemoryStream();
            using BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(contract.Header.Length);
            bw.Write(messageBytes, 0, messageBytes.Length);

            return ms.ToArray();
        }

        public static MessageContract DeserializeBytesIntoMessageContact(byte[] bytes)
        {
            //byte[] lengthBytes = new byte[sizeof(int)];
            //Buffer.BlockCopy(bytes, 0, lengthBytes, 0, lengthBytes.Length);
            int length = BitConverter.ToInt32(bytes, 0);
            string message = Encoding.UTF8.GetString(bytes, 4, length);


            return new MessageContract()
            {
                Header = new MessageContractHeader() { Length = length },
                Message = message
            };
        }
    }
}
