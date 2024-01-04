using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Application.Authentication.Seller.Commands
{
    public class UpdateSellerCommandHandler : IRequestHandler<UpdateSellerCommand, ErrorOr<SellerAuthResult>>
    {
        private readonly IMediator mediator;
        private readonly ISellerProfileRepository repository;

        public UpdateSellerCommandHandler(IMediator mediator, ISellerProfileRepository repository)
        {
            this.mediator = mediator;
            this.repository = repository;
        }

        public async Task<ErrorOr<SellerAuthResult>> Handle(UpdateSellerCommand command, CancellationToken cancellationToken)
        {
            var seller = repository.GetSellerProfileByEmail(command.Email);
            if (seller is null)
            {
                return Domain.Errors.SellerProfile.NotFound;
            }
            seller.FirstName = command.FirstName;
            seller.LastName = command.LastName;
            seller.PhoneNumber = command.PhoneNumber;
            seller.Email = command.Email;
            seller.Country = command.Country;
            seller.State = command.State;
            seller.Address = command.Address;
            seller.Experience = command.Experience;
            seller.Occupation = command.Occupation;

            repository.Update(seller);
            repository.SaveChanges();

            string token = "";
            return new SellerAuthResult(seller, token);

        }
    }
}
