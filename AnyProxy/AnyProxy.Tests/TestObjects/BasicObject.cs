using System;
using System.Collections.Generic;
using System.Text;

namespace AnyProxy.Tests.TestObjects
{
    public class BasicObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }

        public BasicObject()
        {

        }

        public BasicObject(int id, string name, DateTime dateCreated)
        {
            Id = id;
            Name = name;
            DateCreated = dateCreated;
        }
    }
}
