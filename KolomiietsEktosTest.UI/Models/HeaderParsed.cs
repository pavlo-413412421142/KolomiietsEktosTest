using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KolomiietsEktosTest.UI.Models
{
    public class HeaderParsed
    {
        public byte ID { get; set; }
        public byte Size { get; set; }
        [BsonRepresentation(BsonType.Int32)]
        public uint DateTimeUnix { get; set; }
        public byte[] Reserv { get; set; } = new byte[8];
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        [BsonRepresentation(BsonType.String)]
        [MaxLength(8)]
        public string SerialNumber { get; set; }
        public uint RodSize { get; set; }
        public uint PipeSize { get; set; }
        public float? Temperature { get; set; }
        public ushort? Alarms { get; set; }

        public static HeaderParsed ParseFromBinary(byte[] data)
        {
            var header = new HeaderParsed();
            int index = 0;

            header.ID = data[index++];
            header.Size = data[index++];
            header.DateTimeUnix = BitConverter.ToUInt32(data, index);
            index += 4;
            header.Reserv = data.Skip(index).Take(8).ToArray();
            index += 8;
            header.Latitude = BitConverter.ToSingle(data, index);
            index += 4;
            header.Longitude = BitConverter.ToSingle(data, index);
            index += 4;
            header.SerialNumber = Encoding.ASCII.GetString(data, index, 8).TrimEnd('\0');
            index += 8;
            header.RodSize = BitConverter.ToUInt32(data, index);
            index += 4;
            header.PipeSize = BitConverter.ToUInt32(data, index);
            index += 4;

            if (header.ID == 2)
            {
                header.Temperature = BitConverter.ToSingle(data, index);
                index += 4;
            }

            if (index + 2 <= data.Length)
            {
                header.Alarms = BitConverter.ToUInt16(data, index);
            }

            return header;
        }
    }
}
