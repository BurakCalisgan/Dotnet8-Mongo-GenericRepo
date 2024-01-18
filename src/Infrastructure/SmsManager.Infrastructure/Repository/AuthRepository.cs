using Microsoft.Extensions.Options;
using SmsManager.Domain.Documents;
using SmsManager.Domain.Repository;
using SmsManager.Infrastructure.Repository.Common;
using SmsManager.Infrastructure.Settings;

namespace SmsManager.Infrastructure.Repository;

public class AuthRepository : BaseRepository<Auth>, IAuthRepository
{
    public AuthRepository(IOptions<MongoDbSettings> options) : base(options)
    {
    }
}