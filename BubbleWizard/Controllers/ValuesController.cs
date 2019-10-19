using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BubbleWizard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        const string filepath = @"C:\Users\aholmes\source\repos\BubbleWizard\scores.json";

        private List<Player> GetPlayers()
        {
            using (StreamReader file = System.IO.File.OpenText(filepath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<List<Player>>(reader);
            }
        }

        private void SavePlayers(IEnumerable<Player> players)
        {
            using (StreamWriter file = System.IO.File.CreateText(filepath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, players);
            }
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Player>> Get()
        {
            return Ok(GetPlayers());
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Player player)
        {
            var players = GetPlayers();

            player.Drinks = 0;

            players.Add(player);

            SavePlayers(players);
        }

        // PUT api/values/5
        [HttpPut]
        public void Put(string name, [FromBody] Player player)
        {
            var players = GetPlayers();

            var existingPlayer = players.First(o => o.Name == player.Name);

            existingPlayer.Drinks = player.Drinks;

            SavePlayers(players);
        }
    }
}
