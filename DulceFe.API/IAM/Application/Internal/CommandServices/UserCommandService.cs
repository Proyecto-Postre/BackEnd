using DulceFe.API.IAM.Application.Internal.OutboundServices;
using DulceFe.API.IAM.Domain.Model.Aggregates;
using DulceFe.API.IAM.Domain.Model.Commands;
using DulceFe.API.IAM.Domain.Repositories;
using DulceFe.API.IAM.Domain.Services;
using DulceFe.API.Shared.Domain.Repositories;

namespace DulceFe.API.IAM.Application.Internal.CommandServices;

public class UserCommandService : IUserCommandService
{
    private readonly IUserRepository _userRepository;
    private readonly IHashingService _hashingService;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public UserCommandService(IUserRepository userRepository, IHashingService hashingService, ITokenService tokenService, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _hashingService = hashingService;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<User?> Handle(SignUpCommand command)
    {
        if (_userRepository.ExistsByUsername(command.Username))
            throw new Exception($"Username {command.Username} is already taken");

        var hashedPassword = _hashingService.HashPassword(command.Password);
        var user = new User(command.Username, hashedPassword);
        
        await _userRepository.AddAsync(user);
        await _unitOfWork.CompleteAsync();
        
        return user;
    }

    public async Task<(User user, string token)> Handle(SignInCommand command)
    {
        var user = await _userRepository.FindByUsernameAsync(command.Username);
        
        if (user == null || !_hashingService.VerifyPassword(command.Password, user.PasswordHash))
            throw new Exception("Invalid username or password");

        var token = _tokenService.GenerateToken(user);
        
        return (user, token);
    }

    public async Task<User?> Handle(UpdateUserCommand command)
    {
        var user = await _userRepository.FindByIdAsync(command.Id);
        if (user == null) throw new Exception("User not found");

        user.FirstName = command.FirstName;
        user.LastName = command.LastName;
        user.Email = command.Email;
        user.Phone = command.Phone;
        user.Address = command.Address;
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);
        await _unitOfWork.CompleteAsync();

        return user;
    }

    public async Task Handle(UpdateUserPasswordCommand command)
    {
        var user = await _userRepository.FindByIdAsync(command.Id);
        if (user == null) throw new Exception("User not found");

        if (!_hashingService.VerifyPassword(command.CurrentPassword, user.PasswordHash))
            throw new Exception("Incorrect current password");

        user.PasswordHash = _hashingService.HashPassword(command.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Handle(ForgotPasswordCommand command)
    {
        var user = await _userRepository.FindByEmailAsync(command.Email);
        if (user == null) 
        {
            // Don't reveal that the user does not exist
            return; 
        }

        var token = Guid.NewGuid().ToString();
        user.PasswordResetToken = token;
        user.PasswordResetTokenExpiration = DateTime.UtcNow.AddHours(24);
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);
        await _unitOfWork.CompleteAsync();

        // TODO: Send email with token
        Console.WriteLine($"Password reset token for {command.Email}: {token}");
    }

    public async Task Handle(ResetPasswordCommand command)
    {
        var user = await _userRepository.FindByPasswordResetTokenAsync(command.Token);
        if (user == null || user.PasswordResetTokenExpiration < DateTime.UtcNow)
            throw new Exception("Invalid or expired token");

        user.PasswordHash = _hashingService.HashPassword(command.NewPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiration = null;
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);
        await _unitOfWork.CompleteAsync();
    }
}
