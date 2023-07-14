using System;
using System.Collections.Generic;
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
        if (string.IsNullOrWhiteSpace(clause))
            throw new ArgumentNullException(nameof(clause), "Cannot append an empty Flux clause");

        _builder.AppendLine(clause);

        return this;
    }

    /// <inheritdoc />
    public ISortedFluxQuery Sort(bool isDescending, params string[] columns)
    {
        string isDescendingString = isDescending.ToString().ToLowerInvariant();

        if (columns.Length == 0)
        {
            AppendClause($"|> sort(desc: {isDescendingString})");

            return this;
        }

        IEnumerable<string> formattedColumns = columns
            .Where(column => !string.IsNullOrWhiteSpace(column))
            .Select(column => $"\"{column}\"");

        string columnArgs = string.Join(", ", formattedColumns);
        AppendClause($"|> sort(columns: [{columnArgs}], desc: {isDescendingString})");

        return this;
    }

    /// <inheritdoc />
    public IBuildableFluxQuery Limit(uint limit)
    {
        AppendClause($"|> limit(n:{limit})");

        return this;
    }

    /// <inheritdoc />
    public string Build() => _builder.ToString();
}