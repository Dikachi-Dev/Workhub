using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Workhub.Application.Jobber.Command;
public record CanCelJobCommand(string jobId, string userId) :IRequest;