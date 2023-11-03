using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.Persistance;
    public interface ISellerProfileRepository :IGenericRepository<SellerProfile>
    {
        SellerProfile? GetSellerProfileByEmail(string email);
        SellerProfile? GetSellerProfileById(string id);
        SellerProfile? GetSellerProfileByIdAllWithCollections(string id);
        IQueryable<SellerProfile?> GetQueryableSellerProfiles();
    }

