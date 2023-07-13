using Polarity.FluentApi.InfluxDB.Interfaces;

namespace Polarity.FluentApi.InfluxDB.Extensions;

public static class FluxQueryExtensions
{
    public static IFluxQuery FilterMeasurement(this IFluxQuery fluxQuery, string measurement)
    {
        return fluxQuery.AppendClause($"|> filter(fn: (r) => r._measurement == \"{measurement}\")");
    }

    public static IFluxQuery Filter(this IFluxQuery fluxQuery, string filter)
    {
        return fluxQuery.AppendClause($"|> filter(fn: (r) => {filter})");
    }

    public static IFluxQuery IncludeValue(this IFluxQuery fluxQuery, string name, object value)
    {
        return fluxQuery.AppendClause($"|> filter(fn: (r) => r.{name} == {value})");
    }

    public static IFluxQuery IncludeTag(this IFluxQuery fluxQuery, string name, object value)
    {
        return fluxQuery.AppendClause($"|> filter(fn: (r) => r.{name} == \"{value}\")");
    }

    public static IFluxQuery ExcludeValue(this IFluxQuery fluxQuery, string name, object value)
    {
        return fluxQuery.AppendClause($"|> filter(fn: (r) => r.{name} != {value})");
    }

    public static IFluxQuery ExcludeTag(this IFluxQuery fluxQuery, string name, object value)
    {
        return fluxQuery.AppendClause($"|> filter(fn: (r) => r.{name} != \"{value}\")");
    }

    public static ISortedFluxQuery Sort(this IFluxQuery fluxQuery, params string[] columns)
    {
        return fluxQuery.Sort(false, columns);
    }

    public static ISortedFluxQuery SortDescending(this IFluxQuery fluxQuery, params string[] columns)
    {
        return fluxQuery.Sort(true, columns);
    }
}