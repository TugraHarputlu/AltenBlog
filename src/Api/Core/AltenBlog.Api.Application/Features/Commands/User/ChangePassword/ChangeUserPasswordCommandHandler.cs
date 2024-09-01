using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common.Infrastructore.Exceptions;
using BlazorSozluk.Common.Models.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.User.ChangePassword
{
    public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public ChangeUserPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId.HasValue)  
                throw new ArgumentNullException(nameof(request.UserId));

            var dbUser=await _userRepository.GetByIdAsync(request.UserId.Value,true);

            if (dbUser == null)
                throw new DatabaseValidationException("User not found!");

            if (dbUser.Password != request.OldPassword)
                throw new DatabaseValidationException("User not found!");

            return true;
            
        }
    }
}
