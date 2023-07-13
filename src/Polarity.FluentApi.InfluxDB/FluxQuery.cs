using System;
using System.Linq;
using System.Text;
using Polarity.FluentApi.InfluxDB.Interfaces;

namespace Polarity.FluentApi.InfluxDB;

internal class FluxQuery : IFluxQuery, ISortedFluxQuery
{
    private readonly StringBuilder _builder;

    public FluxQuery()
    {
        _builder = new StringBuilder();
    }

    public FluxQuery(string query)
    {
        _builder = new StringBuilder().AppendLine(query);
    }

    public FluxQuery(StringBuilder builder)
    {
        _builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }

    /// <inheritdoc />
    public IFluxQuery AppendClause(string clause)
    {
        _builder.AppendLine(clause);

        return this;
    }

    /// <inheritdoc />
    public ISortedFluxQuery Sort(bool isDescending = false, params string[] columns)
    {
        string isDescendingString = isDescending.ToString().ToLowerInvariant();

        if (columns.Length == 0)
        {
            AppendClause($"|> sort(desc: {isDescendingString})");

            return this;
        }

        string columnArgs = string.Join(", ", columns.Select(column => $"\"{column}\""));
        AppendClause($"|> sort(columns: [{columnArgs}], desc: {isDescendingString})");

        return this;
    }

    /// <inheritdoc />
    public IBuildableFluxQuery Limit(int limit)
    {
        AppendClause($"|> limit(n:{limit})");

        return this;
    }

    /// <inheritdoc />
    public string Build() => _builder.ToString();
}