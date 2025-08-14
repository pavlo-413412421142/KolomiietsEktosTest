using CommunityToolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KolomiietsEktosTest.UI.Models
{
    public partial class HeaderParsed : ObservableObject
    {
        private const double CmToInch = 0.393700787;
        private const double InchToCm = 1 / CmToInch;
        private const double CelsiusToFahrenheitMultiplier = 9.0 / 5.0;
        private const double CelsiusToFahrenheitOffset = 32.0;

        public byte ID { get; set; }
        public byte Size { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        [ObservableProperty]
        private uint dateTimeUnix;

        public byte[] Reserv { get; set; } = new byte[8];

        [ObservableProperty] private float latitude;
        [ObservableProperty] private float longitude;

        [BsonRepresentation(BsonType.String)]
        [MaxLength(8)]
        public string SerialNumber { get; set; }

        [ObservableProperty] private uint rodSize;
        [ObservableProperty] private uint pipeSize;
        [ObservableProperty] private float? temperature;

        public ushort? Alarms { get; set; }

        [ObservableProperty]
        private bool isMetric = true;


        public double RodSizeDisplay => isMetric ? rodSize : Math.Round(rodSize * CmToInch, 3);
        public double PipeSizeDisplay => isMetric ? pipeSize : Math.Round(pipeSize * CmToInch, 3);
        public double? TemperatureDisplay => temperature.HasValue
            ? (isMetric ? temperature.Value : Math.Round(temperature.Value * CelsiusToFahrenheitMultiplier + CelsiusToFahrenheitOffset, 2))
            : null;


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

        public void LoadPreferences()
        {
            IsMetric = Preferences.Get(nameof(IsMetric), true);
        }

        public void NotifyDisplayPropertiesChanged()
        {
            OnPropertyChanged(nameof(RodSizeDisplay));
            OnPropertyChanged(nameof(PipeSizeDisplay));
            OnPropertyChanged(nameof(TemperatureDisplay));
        }
    }
}
