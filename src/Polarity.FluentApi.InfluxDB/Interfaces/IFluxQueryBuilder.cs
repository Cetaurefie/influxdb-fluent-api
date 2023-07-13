
using System;

namespace Polarity.FluentApi.InfluxDB.Interfaces;

public interface IFluxQueryBuilder
{
    IFluxQuery CreateQuery(string bucket, DateTime? start, DateTime? end);
}