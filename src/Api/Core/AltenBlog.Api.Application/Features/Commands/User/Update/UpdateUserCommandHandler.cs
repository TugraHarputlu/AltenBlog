using AltenBlog.Api.Application.Interfaces.Repositories;
using AltenBlog.Common;
using AltenBlog.Common.Events.User;
using AltenBlog.Common.Infrastructore;
using AltenBlog.Common.Infrastructore.Exceptions;
using AltenBlog.Common.Models.RequestModels;
using AutoMapper;
using MediatR;

namespace AltenBlog.Api.Application.Features.Commands.User.Update;

internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid> //rückgabe degeri Guid
{
    private readonly IMapper mapper;
    private readonly IUserRepository userRepository;

    public UpdateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
    {
        this.mapper = mapper;
        this.userRepository = userRepository;
    }

    public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var dbUser = await userRepository.GetByIdAsync(request.Id, true, null);

        if (dbUser is null)
            throw new DatabaseValidationException("User not found!");

        var dbEmailAddress = dbUser.Email;
        // Bu 0 dan fakli ise ikisi de biribirinden farkli oldugu anlamana gelir,büyük kücük harf problemi olmamasi icin CompareOrdinal
        var emailChanged = string.CompareOrdinal(dbEmailAddress, request.EmailAddress) != 0;

        mapper.Map(request, dbUser);//burada requesten yeni bir user olusturmak yerine mevcut user objesinin icine überschreiben yapacak

        //rows veri tabaninda basariyla yapilip yapilmadigini veriyor
        var rows = await userRepository.UpdateAsync(dbUser);
        // Chek if email changed 
        if (rows > 0 && emailChanged)
        {
            var @event = new UserEmailChangedEvent()
            {
                OldEmailAdress = null,
                NewEmailAdress = dbUser.Email

            };

            QueueFactory.SendMessageToExchange(exchangeName: SozlukConstans.UserExchangeName,
                                               exchangeType: SozlukConstans.DefaultExchangeType,
                                               queueName: SozlukConstans.UserEmailChangedQueueName,
                                               obj: @event);
            // emaili degistirdiginde yeniden confirm etmesi gerekiyor
            dbUser.EmailConfirmed = false;
            await userRepository.UpdateAsync(dbUser);
        }
        return dbUser.Id;

    }
}
