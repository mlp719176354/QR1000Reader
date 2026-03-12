using System;
using System.Collections.Generic;
using System.IO;
using LiteDB;

namespace QR1000Reader
{
    public class PassengerRecord
    {
        public int Id { get; set; }
        public string DeparturePortCode { get; set; }
        public string DeparturePortName { get; set; }
        public string ArrivalPortCode { get; set; }
        public string ArrivalPortName { get; set; }
        public DateTime FlightDate { get; set; }
        public string FlightTime { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string PassengerName { get; set; }
        public string TicketNumber { get; set; }
        public string RecognizedText { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedTime { get; set; }
        public string DocumentStatus { get; set; } = "";
        public DateTime? DocumentExpiryDate { get; set; }
    }

    public static class DatabaseHelper
    {
        private static string _dbPath;
        private static LiteDatabase _db;

        public static void Initialize(string dbPath)
        {
            _dbPath = dbPath;
            _db = new LiteDatabase(dbPath);
            
            // 确保集合存在
            var collection = _db.GetCollection<PassengerRecord>("PassengerRecords");
            collection.EnsureIndex(x => x.Id);
            collection.EnsureIndex(x => x.FlightDate);
            collection.EnsureIndex(x => x.DocumentNumber);
        }

        public static void Insert(PassengerRecord record)
        {
            var collection = _db.GetCollection<PassengerRecord>("PassengerRecords");
            record.Id = collection.Count() + 1;
            record.CreatedTime = DateTime.Now;
            collection.Insert(record);
        }

        public static List<PassengerRecord> GetTodayRecords()
        {
            var collection = _db.GetCollection<PassengerRecord>("PassengerRecords");
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            return collection.Find(x => x.CreatedTime >= today && x.CreatedTime < tomorrow).ToList();
        }

        public static List<PassengerRecord> GetAllRecords()
        {
            var collection = _db.GetCollection<PassengerRecord>("PassengerRecords");
            return collection.FindAll().ToList();
        }

        public static void DeleteById(int id)
        {
            var collection = _db.GetCollection<PassengerRecord>("PassengerRecords");
            collection.Delete(id);
        }

        public static void DeleteAll()
        {
            var collection = _db.GetCollection<PassengerRecord>("PassengerRecords");
            var allRecords = collection.FindAll().ToList();
            foreach (var record in allRecords)
            {
                collection.Delete(record.Id);
            }
        }

        public static void Close()
        {
            _db?.Dispose();
        }
    }
}
