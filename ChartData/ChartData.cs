using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace ChartData
{
    public class ChartData
    {
        static object GetColumnData(DbDataRecord record, int ord, bool isDate = false)
        {
            if (!record.IsDBNull(ord))
            {
                if (isDate)
                    return record.GetDateTime(ord);
                else
                {
                    var res = record.GetDouble(ord);
                    return res;
                }
            }
            else return null;
        }

        public static List<MarketData> GetData()
        {

            List<MarketData> result = new List<MarketData>();

            using (OleDbConnection cn = new
           OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" +
                AppDomain.CurrentDomain.BaseDirectory +
                "\";Extended Properties='text;HDR=yes;FMT=Delimited(,)'"))
            {
                cn.Open();
                using (OleDbCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM [EURUSD.csv]";
                    cmd.CommandType = CommandType.Text;
                    using (OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        string[] fields = new string[] { "date", "open", "high", "low", "close", "volume" };
                        Dictionary<string, int> ordinals = new Dictionary<string, int>();
                        foreach (var field in fields)
                            ordinals.Add(field, reader.GetOrdinal(field));
                        MarketData mData1 = new MarketData();
                        foreach (DbDataRecord record in reader)
                        {

                            MarketData mData = new MarketData();
                            mData.Date = (DateTime)GetColumnData(record, ordinals["date"], true);
                            mData.Open = (double)GetColumnData(record, ordinals["open"]);
                            mData.Close = (double)GetColumnData(record, ordinals["close"]);
                            mData.High = (double)GetColumnData(record, ordinals["high"]);
                            mData.Low = (double)GetColumnData(record, ordinals["low"]);
                            result.Add(mData);
                        }
                    }
                }
                return result;
            }

        }

        public class MarketData
        {
            double close;
            DateTime date;
            double high;
            double low;
            double open;

            void RaiseAndSetIfChanged(ref double field, double value)
            {
                field = value;
            }
            void RaiseAndSetIfChanged(ref DateTime field, DateTime value)
            {
                field = value;
            }
            public double Close {
                get {
                    return close;
                }
                set => this.RaiseAndSetIfChanged(ref this.close, value);
            }
            public DateTime Date {
                get => date;
                set => this.RaiseAndSetIfChanged(ref this.date, value);
            }
            public double High {
                get {
                    return high;
                }
                set => this.RaiseAndSetIfChanged(ref this.high, value);
            }
            public double Low {
                get {
                    return low;
                }
                set => this.RaiseAndSetIfChanged(ref this.low, value);
            }
            public double Open {
                get {
                    return open;
                }
                set => this.RaiseAndSetIfChanged(ref this.open, value);
            }

        }
    }
}