using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AristotelisThesis.Domain.Models
{
    public class Account : DomainObject
    {
        public Student AccountHolder { get; set; }
    }
}
