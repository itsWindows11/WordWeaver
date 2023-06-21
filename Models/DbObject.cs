using SQLite;
using System;

namespace WordWeaver.Models
{
    public class DbObject
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
