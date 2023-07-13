namespace Polarity.FluentApi.InfluxDB.Interfaces;

public interface IFluxQuery : IBuildableFluxQuery
{
    IFluxQuery AppendClause(string clause);
    ISortedFluxQuery Sort(bool isDescending, params string[] columns);
}