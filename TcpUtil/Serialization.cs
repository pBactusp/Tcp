using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Text;

namespace TcpUtil
{
    public static class Serialization
    {
        public  static byte[] Serialize(string data, MessageDataType type)
        {
            if (string.IsNullOrEmpty(data))
                throw new ArgumentNullException($"{nameof(data)} must have a value.");

            switch (type)
            {
                case MessageDataType.Text:
                    return SerializeText(data);
                case MessageDataType.Json:
                    throw new NotImplementedException("Serializing Json is not implemented.");
                case MessageDataType.File:
                    return SerializeFile(data);
                default:
                    throw new Exception($"Type of {nameof(type)} is not defined.");
            }
        }

        private static byte[] SerializeText(string text)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(text);
            int length = messageBytes.Length;

            using MemoryStream ms = new MemoryStream();
            using BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(length);
            bw.Write((int)MessageDataType.File);

            bw.Write(messageBytes, 0, messageBytes.Length);

            return ms.ToArray();
        }
        private static byte[] SerializeFile(string path)
        {
            FileInfo info = new FileInfo(path);
            int length = (int)info.Length;

            using MemoryStream ms = new MemoryStream();
            using BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(length);
            bw.Write((int)MessageDataType.File);

            // write length of name of file (with extension) and name of file to the MemoryStream.
            string fileName = Path.GetFileName(path);
            byte[] nameBytes = Encoding.UTF8.GetBytes(fileName);
            bw.Write(nameBytes.Length);
            bw.Write(fileName);

            // write bytes of file.
            bw.Write(File.ReadAllBytes(path));

            return ms.ToArray();
        }



        // Trying to phase out:
        private static void SerializeHeader(BinaryWriter bw, ContractHeader header)
        {
            bw.Write(header.Length);
            bw.Write((int)header.Type);
        }

        public static byte[] Serialize(MessageContract contract)
        {
            if (string.IsNullOrEmpty(contract.Message))
                throw new ArgumentNullException($"{nameof(contract)}.{nameof(contract.Message)} must have a value.");

            byte[] messageBytes = Encoding.UTF8.GetBytes(contract.Message);
            contract.Header = new ContractHeader()
            {
                Length = messageBytes.Length,
                Type = MessageDataType.Text
            };

            using MemoryStream ms = new MemoryStream();
            using BinaryWriter bw = new BinaryWriter(ms);

            SerializeHeader(bw, contract.Header);
            bw.Write(messageBytes, 0, messageBytes.Length);

            return ms.ToArray();
        }

        public static byte[] Serialize(FileContract contract)
        {
            FileInfo info = new FileInfo(contract.Path);
            contract.Header = new ContractHeader()
            {
                Length = (int)info.Length,
                Type = MessageDataType.File
            };

            using MemoryStream ms = new MemoryStream();
            using BinaryWriter bw = new BinaryWriter(ms);
            SerializeHeader(bw, contract.Header);

            // write length of name of file (with extension) and name of file to the MemoryStream.
            string fileName = Path.GetFileName(contract.Path);
            byte[] nameBytes = Encoding.UTF8.GetBytes(fileName);
            bw.Write(nameBytes.Length);
            bw.Write(fileName);

            // write bytes of file.
            bw.Write(File.ReadAllBytes(contract.Path));

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
                Header = new ContractHeader() { Length = length },
                Message = message
            };
        }
        

    }
}
