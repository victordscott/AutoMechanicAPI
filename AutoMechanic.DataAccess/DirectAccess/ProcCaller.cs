using AutoMechanic.Configuration.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AutoMechanic.DataAccess.DirectAccess;

public class ProcCaller : IProcCaller
{
    private readonly IOptions<DatabaseOptions> databaseOptions;
    private readonly string connectionString;
    private readonly int defaultTimeout = 120;

    public ProcCaller(IOptions<DatabaseOptions> databaseOptions)
    {
        this.databaseOptions = databaseOptions;
        this.connectionString = databaseOptions.Value.ConnectionString;
        if (!connectionString.Contains("CommandTimeout"))
        {
            connectionString += $";CommandTimeout={defaultTimeout}";
        }
    }

    string ReturnArray(string json)
    {
        return string.IsNullOrWhiteSpace(json) ? "[]" : json;
    }

    public async Task<int> CallProc(
        string procName,
        object param = null,
        string connection = null
    )
    {
        if (connection == null)
            connection = connectionString;

        var sql = procName;
        int result = 0;
        using (NpgsqlConnection sqlConnection = new NpgsqlConnection(connection) { })
        {
            sqlConnection.Open();
            var fullSql = $"SELECT {sql}()";
            if (param != null)
            {
                fullSql = $"SELECT {sql}(@param)";
            }
            using (NpgsqlCommand cmd = new NpgsqlCommand(fullSql, sqlConnection))
            {
                if (param != null)
                {
                    var paramType = NpgsqlTypes.NpgsqlDbType.Json;
                    if (param is int)
                    {
                        paramType = NpgsqlTypes.NpgsqlDbType.Integer;
                    }
                    else if (param is long)
                    {
                        paramType = NpgsqlTypes.NpgsqlDbType.Bigint;
                    }
                    else if (param is string)
                    {
                        paramType = NpgsqlTypes.NpgsqlDbType.Text;
                    }
                    else if (param is Guid)
                    {
                        paramType = NpgsqlTypes.NpgsqlDbType.Uuid;
                    }
                    if (paramType == NpgsqlTypes.NpgsqlDbType.Json)
                    {
                        param = JsonSerializer.Serialize(param);
                    }
                    cmd.Parameters.AddWithValue("param", paramType, param);
                }
                result = await cmd.ExecuteNonQueryAsync();
            }
        }
        return result;
    }

    //public async Task<T> CallProc<T>(
    //    string procName,
    //    object param = null,
    //    string connection = null
    //)
    //{
    //    if (connection == null)
    //        connection = defaultConnection;

    //    var sql = procName;
    //    StringBuilder sb = new StringBuilder();
    //    using (NpgsqlConnection sqlConnection = new NpgsqlConnection(connection))
    //    {
    //        sqlConnection.Open();
    //        var fullSql = $"SELECT {sql}()";
    //        if (param != null)
    //        {
    //            fullSql = $"SELECT {sql}(@param)";
    //        }
    //        using (NpgsqlCommand cmd = new NpgsqlCommand(fullSql, sqlConnection))
    //        {
    //            if (param != null)
    //            {
    //                var paramType = NpgsqlTypes.NpgsqlDbType.Json;
    //                if (param is int)
    //                {
    //                    paramType = NpgsqlTypes.NpgsqlDbType.Integer;
    //                }
    //                else if (param is long)
    //                {
    //                    paramType = NpgsqlTypes.NpgsqlDbType.Bigint;
    //                }
    //                else if (param is Guid)
    //                {
    //                    paramType = NpgsqlTypes.NpgsqlDbType.Uuid;
    //                }
    //                if (paramType == NpgsqlTypes.NpgsqlDbType.Json)
    //                {
    //                    param = JsonSerializer.Serialize(param);
    //                }
    //                cmd.Parameters.AddWithValue("param", paramType, param);
    //            }
    //            using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
    //            {
    //                while (await reader.ReadAsync())
    //                {
    //                    if (!reader.IsDBNull(0))
    //                    {
    //                        sb.Append(reader.GetString(0));
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    string json = ConvertPostgreJson(sb.ToString());
    //    if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
    //    {
    //        json = ReturnArray(json);
    //    }
    //    if (typeof(T) != typeof(string))
    //    {
    //        if (!string.IsNullOrWhiteSpace(json))
    //        {
    //            return JsonSerializer.Deserialize<T>(json);
    //        }
    //        else
    //        {
    //            return default;
    //        }
    //    }
    //    else
    //    {
    //        return (T)Convert.ChangeType(json, typeof(T)); ;
    //    }
    //}

    public T CallProc<T>(
        string procName,
        object param = null,
        string connection = null
    )
    {
        if (connection == null)
            connection = connectionString;

        var sql = procName;
        StringBuilder sb = new StringBuilder();
        using (NpgsqlConnection sqlConnection = new NpgsqlConnection(connection))
        {
            sqlConnection.Open();
            var fullSql = $"SELECT {sql}()";
            if (param != null)
            {
                fullSql = $"SELECT {sql}(@param)";
            }
            using (NpgsqlCommand cmd = new NpgsqlCommand(fullSql, sqlConnection))
            {
                if (param != null)
                {
                    var paramType = NpgsqlTypes.NpgsqlDbType.Json;
                    if (param is int)
                    {
                        paramType = NpgsqlTypes.NpgsqlDbType.Integer;
                    }
                    else if (param is long)
                    {
                        paramType = NpgsqlTypes.NpgsqlDbType.Bigint;
                    }
                    else if (param is Guid)
                    {
                        paramType = NpgsqlTypes.NpgsqlDbType.Uuid;
                    }
                    else if (param is string)
                    {
                        paramType = NpgsqlTypes.NpgsqlDbType.Text;
                    }

                    if (paramType == NpgsqlTypes.NpgsqlDbType.Json)
                    {
                        param = JsonSerializer.Serialize(param);
                    }
                    cmd.Parameters.AddWithValue("param", paramType, param);
                }
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            sb.Append(reader.GetString(0));
                        }
                    }
                }
            }
        }

        string json = ConvertPostgreJson(sb.ToString());
        if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
        {
            json = ReturnArray(json);
        }
        if (typeof(T) != typeof(string))
        {
            if (!string.IsNullOrWhiteSpace(json))
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            else
            {
                return default;
            }
        }
        else
        {
            return (T)Convert.ChangeType(json, typeof(T)); ;
        }
    }

    private string ConvertPostgreJson(string postgreJson)
    {
        var convertedJson = postgreJson;
        if (!string.IsNullOrEmpty(convertedJson))
        {
            //convertedJson = convertedJson.Replace("\\\"", "");
            //convertedJson = convertedJson.Replace("\\u0027", "'");
            //convertedJson = convertedJson.Replace("\\u0022", "\"");
            var settings = new JsonSerializerSettings { ContractResolver = new PascalCaseContractResolver() };
            if (convertedJson.StartsWith("{"))
            {
                var obj = JsonConvert.DeserializeObject<ExpandoObject>(convertedJson);
                convertedJson = JsonConvert.SerializeObject(obj, settings);
            }
            else if (convertedJson.StartsWith("["))
            {
                var array = JsonConvert.DeserializeObject<List<ExpandoObject>>(convertedJson);
                convertedJson = JsonConvert.SerializeObject(array, settings);
            }
        }
        return convertedJson;
    }
}
