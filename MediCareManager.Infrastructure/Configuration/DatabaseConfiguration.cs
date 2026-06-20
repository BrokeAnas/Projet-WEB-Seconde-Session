using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace MediCareManager.Infrastructure.Configuration;

/// <summary>
/// Centralise la création des connexions MySQL et la configuration globale de Dapper.
/// </summary>
public static class DatabaseConfiguration
{
    public const string ConnectionStringName = "DefaultConnection";

    static DatabaseConfiguration()
    {
        // Permet de mapper les colonnes snake_case (id_nat) sur les propriétés PascalCase (IdNat).
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        // MySql.Data renvoie DATE -> DateTime et TIME -> TimeSpan : on convertit vers DateOnly/TimeOnly.
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        SqlMapper.AddTypeHandler(new TimeOnlyTypeHandler());
    }

    public static MySqlConnection CreateConnection(string connectionString)
        => new MySqlConnection(connectionString);
}

public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override DateOnly Parse(object value) => DateOnly.FromDateTime(Convert.ToDateTime(value));

    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }
}

public class TimeOnlyTypeHandler : SqlMapper.TypeHandler<TimeOnly>
{
    public override TimeOnly Parse(object value) => value switch
    {
        TimeSpan ts => TimeOnly.FromTimeSpan(ts),
        DateTime dt => TimeOnly.FromDateTime(dt),
        string s => TimeOnly.Parse(s),
        _ => TimeOnly.FromTimeSpan((TimeSpan)value)
    };

    public override void SetValue(IDbDataParameter parameter, TimeOnly value)
    {
        parameter.DbType = DbType.Time;
        parameter.Value = value.ToTimeSpan();
    }
}
