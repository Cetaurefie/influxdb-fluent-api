// See https://aka.ms/new-console-template for more information

using Polarity.FluentApi.InfluxDB;
using Polarity.FluentApi.InfluxDB.Extensions;

var fluxQuery = new FluxQueryBuilder().CreateQuery("realtime", DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow)
    .FilterMeasurement("Vehicles[100].PathId")
    .SortDescending()
    .Limit(10)
    .Build();


Console.WriteLine(fluxQuery);