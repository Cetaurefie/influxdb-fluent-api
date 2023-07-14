// See https://aka.ms/new-console-template for more information

using Polarity.FluentApi.InfluxDB;
using Polarity.FluentApi.InfluxDB.Extensions;

string fluxQuery = new FluxQueryBuilder().CreateQuery("realtime", null, DateTime.UtcNow)
    .FilterMeasurement("System.Time.Second")
    .Include("_value", "9")
    .Include("DeviceId", 0)
    .SortDescending()
    .Limit(10)
    .Build();


Console.WriteLine(fluxQuery);