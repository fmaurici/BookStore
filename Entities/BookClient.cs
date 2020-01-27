using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Entities
{
    public class BookClient
    {
        public Guid BookId { get; set; }
        [JsonIgnore]
        public Book Book { get; set; }
        public Guid ClientId { get; set; }
        [JsonIgnore]
        public Client Client { get; set; }
    }
}
