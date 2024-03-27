using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Workhub.Application.Jobber.Command;
public record DeclineJobCommand(string jobId): IRequest;