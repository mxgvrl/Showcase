using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AppleShowcase.Data.Models {
    public class Product {
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [Display(Name="name")]
        public string name { get; set; }
        
        [Display(Name="description")]
        public string description { get; set; }
        
        [Display(Name="price")]
        public string price { get; set; }

        [Display(Name="imageID")]
        public string imageID { get; set; }
 
        public bool HasImage()
        {
            return !string.IsNullOrWhiteSpace(imageID);
        }
    }
}