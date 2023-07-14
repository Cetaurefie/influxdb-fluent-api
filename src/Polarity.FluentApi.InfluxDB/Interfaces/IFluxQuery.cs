namespace Polarity.FluentApi.InfluxDB.Interfaces;

public interface IFluxQuery : IBuildableFluxQuery
{
    /// <summary>
    /// Appends a string to the <see cref="IFluxQuery"/>
    /// </summary>
    /// <param name="clause">Flux clause to append</param>
    /// <exception cref="System.ArgumentNullException"><paramref name="clause"/> is NULL or white space</exception>
    /// <returns></returns>
    IFluxQuery AppendClause(string clause);

    /// <summary>
    /// Append a Flux sort by the specified <paramref name="columns"/>. This is equivalent to the Flux:
    /// <code>|> sort(columns: ["columns[0]", "columns[1]", ...], desc: {isDescending})</code>
    /// </summary>
    /// <param name="isDescending">True if should sort in descending order</param>
    /// <param name="columns">Columns to sort</param>
    /// <returns></returns>
    ISortedFluxQuery Sort(bool isDescending, params string[] columns);
}