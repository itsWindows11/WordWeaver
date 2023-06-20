using System;

namespace WordWeaver.Models
{
    public class DbObject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
