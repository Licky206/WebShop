﻿using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Authorization.Models;
using System.Data.Common;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Authorization.Repositories
{
    public class RacunRepository : IRacunRepository
    {
        private readonly string _connectionString;

        public RacunRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Racun>> GetAllRacuniAsync()
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT RacunId, StatusRacuna, Datum FROM Racun ";
                return await connection.QueryAsync<Racun>(query);
            }
        }
        public async Task<IEnumerable<StavkeRacuna>> GetStavkeByRacunIdAsync(int racunId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM StavkeRacuna WHERE RacunId = @RacunId";
                return await connection.QueryAsync<StavkeRacuna>(query, new { RacunId = racunId });
            }
        }

        public async Task<bool> UpdateRacunStatusAsync(int racunId, string noviStatus)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                UPDATE Racun
                SET StatusRacuna = @StatusRacuna
                WHERE RacunId = @RacunId";

                var affectedRows = await connection.ExecuteAsync(query, new { StatusRacuna = noviStatus, RacunId = racunId });
                return affectedRows > 0;
            }
        }

        public async Task<bool> DeleteRacunAsync(int racunId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Racun WHERE RacunId = @RacunId";
                var affectedRows = await connection.ExecuteAsync(query, new { RacunId = racunId });
                return affectedRows > 0;
            }
        }


        public async Task<Racun> GetRacunByIdAsync(int racunId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT RacunId, StatusRacuna, Datum FROM Racun WHERE RacunId = @RacunId";
                return await connection.QuerySingleOrDefaultAsync<Racun>(query, new { RacunId = racunId });
            }
        }

        public async Task<int> KreirajRacunSaStavkama(string statusRacuna, DateTime? datum, DataTable stavkeTable)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@StatusRacuna", statusRacuna);
                parameters.Add("@Datum", datum);
                parameters.Add("@Stavke", stavkeTable.AsTableValuedParameter("StavkeRacunaTableType"));

                return await connection.ExecuteAsync(
                    "BulkInsertRacunAndStavke",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

    }
}