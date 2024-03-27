using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workhub.Contracts.Profileing;
    public record MyProfileResponse(string FirstName,
 string LastName,
 string Email,
 string PhoneNumber,
 string ProfileImage,
 string Country,
 string Address,
 string State,
 string Occupation,
 string Gender,
 string Experience,
 double Rating,
 int JobCount,
 string Token,
 string Id,
 string LongLat,
 string UserType);