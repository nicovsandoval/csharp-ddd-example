using Electrorating.Domain.Entities;

namespace Electrorating.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    User? GetUserByEmail(string email);
    void Add(User user);
}
