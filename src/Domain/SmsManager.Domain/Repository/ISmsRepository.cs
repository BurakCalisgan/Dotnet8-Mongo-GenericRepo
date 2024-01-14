using SmsManager.Domain.Documents;
using SmsManager.Domain.Repository.Common;

namespace SmsManager.Domain.Repository;

public interface ISmsRepository : IBaseRepository<Sms, string>
{
}