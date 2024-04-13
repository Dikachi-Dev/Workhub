using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Workhub.Contracts.Profileing;
public record VendorRequest(string Description, IFormFile Image1, IFormFile Image2, string Instagram);