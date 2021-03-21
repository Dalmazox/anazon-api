using Anazon.Domain.Entities.Base;
using System;

namespace Anazon.Domain.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public DateTime Birthdate { get; set; }
        public char Sex { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
