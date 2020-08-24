using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.Models
{
    public class BaseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
    [Table("Book")]
    public class Book : BaseModel
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string Id { get; set; }

        //#region snippet_BookNameProperty
        //[BsonElement("Name")]
        //[JsonProperty("Name")]
        public string Name { get; set; }
        //#endregion

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }
    }
}
