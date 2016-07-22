﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MarvelDemo.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MarvelDemo.Core.Services
{
    public class MarvelDataService : IMarvelDataService
    {
        // TODO: Add your Marvel Developer Account keys
        const string _API_PRIVATE_KEY = "";
        const string _API_PUBLIC_KEY = "";

        readonly IHashService _hashService;

        public MarvelDataService(IHashService hashService)
        {
            _hashService = hashService;
        }

        public async Task<IEnumerable<Comic>> GetComicsBySeries(int seriesId)
        {
            var ts = Guid.NewGuid().ToString();
            var hash = _hashService.CreateMd5Hash(ts + _API_PRIVATE_KEY + _API_PUBLIC_KEY);

            var url =
                $@"http://gateway.marvel.com/v1/public/series/{seriesId}/comics?orderBy=issueNumber&apikey={_API_PUBLIC_KEY}&hash={hash}&ts={ts}";
            
            var client = new HttpClient();
            var response = await client.GetStringAsync(url);
            
            var responseObject = JObject.Parse(response);

            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<Comic>>(responseObject["data"]["results"].ToString()));
        }
    }
}
