using NArchitecture.Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Report:Entity<Guid>
    {
        public Guid CounterId { get; set; }
        public DateTime RequestedDate { get; set; }
        public string StatusCode { get; set; }
    }
}
