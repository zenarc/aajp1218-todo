using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace todo.Models
{
    public class Item
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name"), Required]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description"), Required]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "isComplete")]
        public bool Completed { get; set; }
    }
}
