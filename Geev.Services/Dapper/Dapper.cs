using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Geev.Services.Dapper
{
    public class Dapper<T> : IDapper<T> where T : class
    {
        private readonly IDbConnector _conStr;

        public Dapper(IDbConnector conStr)
        {
            _conStr = conStr;
        }
        public async Task Execute(string query, List<InputParameters> parameters = null, ExecutionType executionType = ExecutionType.storeProcedure)
        {
            using (var connection = new SqlConnection(_conStr.ConnectionString))
            {
                if (parameters != null)
                {
                    DynamicParameters parameter = new DynamicParameters();
                    foreach (var p in parameters)
                    {
                        if (p.ParameterType == ParameterTypes.String)
                        {
                            parameter.Add($"@{p.Name}"
                           , p.Value
                           , DbType.String
                           , (p.ParameterDirection == ParameterDirections.ReturnValue) ? ParameterDirection.ReturnValue : ParameterDirection.Input
                           );
                        }
                        else
                        {
                            parameter.Add($"@{p.Name}"
                         , p.Value
                         , DbType.Int32
                         , (p.ParameterDirection == ParameterDirections.ReturnValue) ? ParameterDirection.ReturnValue : ParameterDirection.Input
                         );
                        }
                        query += $" @{p.Name},";

                    }
                    query = query.Substring(0, query.Length - 1);
                    await connection.ExecuteAsync(query
                   , parameter
                   , commandType: (executionType == ExecutionType.storeProcedure) ? CommandType.StoredProcedure : CommandType.Text);
                }
                else
                {
                    await connection.ExecuteAsync(query
                        , commandType: (executionType == ExecutionType.storeProcedure) ? CommandType.StoredProcedure : CommandType.Text);
                }

            }
        }

        public List<T> ExecuteReader(string query, List<InputParameters> parameters = null)
        {
            using (var connection = new SqlConnection(_conStr.ConnectionString))
            {
                if (parameters != null)
                {
                    DynamicParameters parameter = new DynamicParameters();
                    foreach (var p in parameters)
                    {
                        if (p.ParameterType == ParameterTypes.String)
                        {
                            parameter.Add($"@{p.Name}"
                           , p.Value
                           , DbType.String
                           , (p.ParameterDirection == ParameterDirections.ReturnValue) ? ParameterDirection.ReturnValue : ParameterDirection.Input
                           );
                        }
                        else
                        {
                            parameter.Add($"@{p.Name}"
                         , p.Value
                         , DbType.Int32
                         , (p.ParameterDirection == ParameterDirections.ReturnValue) ? ParameterDirection.ReturnValue : ParameterDirection.Input
                         );
                        }
                    }
                    return connection.QueryAsync<T>(query, parameter, commandType: CommandType.StoredProcedure).Result.ToList();

                }
                else
                {
                    return connection.QueryAsync<T>(query, commandType: CommandType.StoredProcedure).Result.ToList();
                }

            }
        }

        public List<T> Query(string query, List<InputParameters> parameters = null)
        {
            using (var connection = new SqlConnection(_conStr.ConnectionString))
            {
                if (parameters != null)
                {
                    DynamicParameters parameter = new DynamicParameters();
                    foreach (var p in parameters)
                    {
                        if (p.ParameterType == ParameterTypes.String)
                        {
                            parameter.Add($"@{p.Name}"
                           , p.Value
                           , DbType.String
                           , (p.ParameterDirection == ParameterDirections.ReturnValue) ? ParameterDirection.ReturnValue : ParameterDirection.Input
                           );
                        }
                        else
                        {
                            parameter.Add($"@{p.Name}"
                         , p.Value
                         , DbType.Int32
                         , (p.ParameterDirection == ParameterDirections.ReturnValue) ? ParameterDirection.ReturnValue : ParameterDirection.Input
                         );
                        }
                    }
                    return connection.QueryAsync<T>(query, parameter, commandType: CommandType.Text).Result.ToList();
                }
                else
                {
                    return connection.QueryAsync<T>(query, commandType: CommandType.Text).Result.ToList();
                }

            }
        }
    }

}
