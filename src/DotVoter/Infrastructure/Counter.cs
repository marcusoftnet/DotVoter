using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoRepository;

namespace DotVoter.Infrastructure
{
    public class Counter :Entity
    {
        public int Seq { get; set; }
        public int Name { get; set; }
    }
}