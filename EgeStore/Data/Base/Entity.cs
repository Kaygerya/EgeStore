﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgeStore.Data.Base
{
    [BsonIgnoreExtraElements]
    public class Entity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonIgnore]
        public string Error { get; set; }
        [BsonIgnore]
        public bool IsSuccess { get { return string.IsNullOrEmpty(Error); } }
    }
}
