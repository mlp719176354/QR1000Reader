using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace QR1000Reader
{
    public class PortInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class DocumentTypeInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class CountryInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class FlightTimeConfig
    {
        public int StartHour { get; set; } = 6;
        public int EndHour { get; set; } = 23;
        public int IntervalMinutes { get; set; } = 30;
        public int DefaultHour { get; set; } = 9;
        public int DefaultMinute { get; set; } = 0;
    }

    public class ConfigData
    {
        public List<PortInfo> Ports { get; set; }
        public List<DocumentTypeInfo> DocumentTypes { get; set; }
        public List<CountryInfo> PopularCountries { get; set; }
        public string WebSocketUri { get; set; } = "ws://127.0.0.1:90/echo";
        public FlightTimeConfig FlightTimes { get; set; } = new FlightTimeConfig();
    }

    public static class ConfigHelper
    {
        private static ConfigData _config;
        private static string _configPath;

        public static void Load(string configPath)
        {
            _configPath = configPath;
            if (File.Exists(configPath))
            {
                var yaml = File.ReadAllText(configPath);
                var deserializer = new Deserializer();
                _config = deserializer.Deserialize<ConfigData>(yaml);
            }
            else
            {
                _config = new ConfigData
                {
                    Ports = new List<PortInfo>(),
                    DocumentTypes = new List<DocumentTypeInfo>(),
                    PopularCountries = new List<CountryInfo>(),
                    WebSocketUri = "ws://127.0.0.1:90/echo"
                };
            }
        }

        public static List<PortInfo> GetPorts() => _config?.Ports ?? new List<PortInfo>();
        public static List<DocumentTypeInfo> GetDocumentTypes() => _config?.DocumentTypes ?? new List<DocumentTypeInfo>();
        public static List<CountryInfo> GetPopularCountries() => _config?.PopularCountries ?? new List<CountryInfo>();
        public static string GetWebSocketUri() => _config?.WebSocketUri ?? "ws://127.0.0.1:90/echo";
        public static FlightTimeConfig GetFlightTimeConfig() => _config?.FlightTimes ?? new FlightTimeConfig();

        public static PortInfo GetPortByCode(string code)
        {
            return _config?.Ports.Find(p => p.Code == code);
        }

        public static CountryInfo GetCountryByCode(string code)
        {
            return _config?.PopularCountries.Find(c => c.Code == code);
        }
    }
}
