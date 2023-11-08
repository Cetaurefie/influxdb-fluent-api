// See https://aka.ms/new-console-template for more information

using Polarity.FluentApi.InfluxDB;
using Polarity.FluentApi.InfluxDB.Extensions;
using Polarity.FluentApi.InfluxDB.Interfaces;

IFluxQueryBuilder queryBuilder = new FluxQueryBuilder();

string fluxQuery = queryBuilder.CreateQuery("realtime", end: DateTime.UtcNow.AddDays(-1))
    .FilterMeasurement("System.Time.Second")
    .Filter("r.DeviceId != 0 and r._value > 9 and r._value <= 53")
    .Include("_value", "9")
    .Include("DeviceId", 0)
    .Distinct("DeviceId")
    .Quantile(new TimeSpan(1, 7, 53, 16), 0.9f)
    .SortDescending()
    .Limit(10)
    .Build();

Console.WriteLine(fluxQuery);

string otherFluxQuery = FluxQueryBuilder.Create("myOtherBucket")
    .Exclude("_value", 53)
    .Build();

Console.WriteLine(otherFluxQuery);
