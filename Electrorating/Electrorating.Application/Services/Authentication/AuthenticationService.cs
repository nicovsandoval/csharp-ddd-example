using Electrorating.Application.Common.Interfaces.Authentication;
using Electrorating.Application.Common.Interfaces.Persistence;
using Electrorating.Domain.Entities;

namespace Electrorating.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public AuthenticationResult Login(string email, string password)
    {
        return new AuthenticationResult(Guid.NewGuid(), "firstName", "lastName", email, "token");
    }

    public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    {
        // 1, Validate the user doesn't exist
        if (_userRepository.GetUserByEmail(email) is not null) 
        {
            throw new Exception("User with given email already exist");
;        }

        // 2. Create user (generate unique ID) & Persist to DB
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password,
        };

        _userRepository.Add(user);

        // 3. Create JWT Token
        var token = _jwtTokenGenerator.GenerateToken(user.Id, firstName, lastName);

        return new AuthenticationResult(user.Id, firstName, lastName, email, token);
    }
}
