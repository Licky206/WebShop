﻿using Authorization.Models;
using Authorization.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace Authorization.Services
{
    public class RacunService : IRacunService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IRacunRepository _racunRepository;
        

        public RacunService(IRacunRepository racunRepository, IMemoryCache memoryCache)
        {
            _racunRepository = racunRepository;
            _memoryCache = memoryCache;
        }

        public async Task<bool> KreirajRacunSaStavkama(string statusRacuna, DateTime? datum,  IEnumerable<StavkeRacunaDTO> stavke)
        {
            string cacheKey = $"Racun_{statusRacuna}_{datum?.ToString("yyyyMMdd")}";
            if (_memoryCache.TryGetValue(cacheKey, out bool cachedResult))
            {
                return cachedResult;
            }

            var stavkeTable = new DataTable();
            stavkeTable.Columns.Add("RacunId", typeof(int));
            stavkeTable.Columns.Add("ProizvodID", typeof(int));
            stavkeTable.Columns.Add("Kolicina", typeof(int));
            stavkeTable.Columns.Add("Popust", typeof(decimal));

            foreach (var stavka in stavke)
            {
                stavkeTable.Rows.Add(0, stavka.ProizvodID, stavka.Kolicina, stavka.Popust);
            }

            var result = await _racunRepository.KreirajRacunSaStavkama(statusRacuna, datum, stavkeTable);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };

            _memoryCache.Set(cacheKey, result > 0, cacheEntryOptions);

            return result > 0;
        }
    }
}
