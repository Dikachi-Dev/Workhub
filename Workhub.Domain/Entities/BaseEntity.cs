using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workhub.Domain.Entities;

public abstract class BaseEntity
{
    public DateTime CreatedOn { get; set; }
}