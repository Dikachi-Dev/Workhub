using ErrorOr;
using MediatR;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Seller.Commands;

public class CreateSellerCommandHandler : IRequestHandler<CreateSellerCommand, ErrorOr<SellerAuthResult>>
{
    private readonly IMediator mediator;
    private readonly ISellerProfileRepository repository;

    public CreateSellerCommandHandler(ISellerProfileRepository repository, IMediator mediator)
    {
        this.repository = repository;
        this.mediator = mediator;
    }


    public async Task<ErrorOr<SellerAuthResult>> Handle(CreateSellerCommand command, CancellationToken cancellationToken)
    {
        if (repository.GetSellerProfileByEmail(command.Email) != null)
        {
            return Domain.Errors.SellerProfile.DuplicateEmail;
        }
        var seller = new SellerProfile
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
            Country = command.Country,
            State = command.State,
            Address = command.Address,
            Occupation = command.Occupation,
            Gender = command.Gender,
            Experience = command.Experience

        };
        repository.Add(seller);
        repository.SaveChanges();
        string token = "";
        return new SellerAuthResult(seller, token);
    }

}
