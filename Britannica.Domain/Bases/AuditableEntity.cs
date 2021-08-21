using System;

namespace Britannica.Domain.Bases
{
    public abstract class AuditableEntity
    {
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
