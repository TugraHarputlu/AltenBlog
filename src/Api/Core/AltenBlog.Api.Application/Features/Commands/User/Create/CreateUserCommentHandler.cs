using AltenBlog.Api.Application.Interfaces.Repositories;
using AltenBlog.Common;
using AltenBlog.Common.Events.User;
using AltenBlog.Common.Infrastructore;
using AltenBlog.Common.Infrastructore.Exceptions;
using AltenBlog.Common.Models.RequestModels;
using AutoMapper;
using MediatR;

namespace AltenBlog.Api.Application.Features.Commands.User.Create
{
    //Burada bir user olusturduktan sonra confirm edilmesi gerekiyor, bunu direk yapmak yerine workerler üserinden
    // RabbitMQ ya gönderim onu arka planda hellediyor olacagiz
    public class CreateUserCommentHandler : IRequestHandler<CreateUserComment, Guid> // Guid rückgabe degeri
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public CreateUserCommentHandler(IMapper mapper, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateUserComment request, CancellationToken cancellationToken)
        {
            var existUser = await userRepository.GetSingleAsync(i => i.Email == request.EmailAddress);

            if (existUser is not null)
            {
                throw new DatabaseValidationException("User already exists!");
            }
            var dbUser = mapper.Map<AtenBlog.Api.Domain.Models.User>(request);//Bana bir tane User olustur ve bunuda request obj den olusutr.

            var rows = await userRepository.AddAsync(dbUser);


            // Email Change//Createt RabbitMQ ya gidip 
            if (rows > 0) //Bu sifirdan büyük ise gercekten veri tabanina kayit edilmistir.
            {
                var @event = new UserEmailChangedEvent()
                {
                    OldEmailAdress = null,
                    NewEmailAdress = dbUser.Email

                };
                //Buraya bir Klass gönderecegiz, bu klas bir event olacak, bu projenin kotexleri icindeki evetlere denk gelecek
                //o olusturmus oldugum evet buraya gönderecem , eventin icinde hem eski hemde yeni email olach, cunku ayni que yu user
                //create edildiginde hem de update edildiginde kullanacam, burada hem eski hemde yeni emaile ihtiyacim olacak
                QueueFactory.SendMessageToExchange(exchangeName: SozlukConstans.UserExchangeName, exchangeType: SozlukConstans.DefaultExchangeType, queueName: SozlukConstans.UserEmailChangedQueueName, obj: @event);
            }
            return dbUser.Id;
        }
    }
}
