using SmsManager.Domain.Documents;
using SmsManager.Domain.Repository.Common;

namespace SmsManager.Domain.Repository;

public interface IAuthRepository : IBaseRepository<Auth, string>
{
}