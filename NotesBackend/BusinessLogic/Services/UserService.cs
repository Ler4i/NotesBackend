using DataAccess.Models;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IRubicContext _context;

        public UserService(IMapper mapper, IRubicContext context)
        {
            _mapper = mapper;
            _context = context;//ходит в контекст
        }

        public Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            throw new NotImplementedException();
        }

        public Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            throw new NotImplementedException();
        }

        public async Task<UserInformationBlo> AuthWithEmail(string email, string password)
        {
            UserRto user = await _context.Users.FirstOrDefaultAsync(p => p.Email == email && p.Password == password);//асинхронно

            if (user == null)
            {
                throw new NotFoundException($"Пользователь с почтой {email} не найден");
            }
            return await ConvertToUserInformationAsync(user);//await освобождает от Task
        }

        public async Task<UserInformationBlo> AuthWithLogin(string login, string password)
        {

            UserRto user = await _context.Users.FirstOrDefaultAsync(p => p.Login == login && p.Password == password);//асинхронно

            if (user == null)
            {
                throw new NotFoundException($"Пользователь с логином {login} не найден");
            }
            return await ConvertToUserInformationAsync(user);//await освобождает от Task
        }

        public async Task<UserInformationBlo> AuthWithPhone(string numberPrefix, string number, string password)
        {

            UserRto user = await _context.Users.FirstOrDefaultAsync(p => p.PhoneNumberPrefix == numberPrefix && p.PhoneNumber == number && p.Password == password);//асинхронно

            if (user == null)
            {
                throw new NotFoundException($"Пользователь с телефоном {numberPrefix}{number} не найден");
            }

            //
            return token;
        }

        public async Task<bool> DoesExist(string numberPrefix, string number)//не занят
        {
            bool result = await _context.Users.AnyAsync(y => y.PhoneNumber == number && y.PhoneNumberPrefix == numberPrefix);
            return result;
        }

        public async Task<UserInformationBlo> Get(int userId)
        {
            UserRto user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) throw new NotFoundException("пользователь не найден");

            return await ConvertToUserInformationAsync(user);
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            throw new NotImplementedException();
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            throw new NotImplementedException();
        }

        public Task PostAuthenticateAsync(PostAuthenticationContext context)
        {
            throw new NotImplementedException();
        }

        public Task PreAuthenticateAsync(PreAuthenticationContext context)
        {
            throw new NotImplementedException();
        }

        public async Task<UserInformationBlo> RegisterWithPhone(string numberPrefix, string number, string password)
        {

            bool result = await _context.Users.AnyAsync(y => y.PhoneNumber == number && y.PhoneNumberPrefix == numberPrefix);//сущ-ет ли 
            if (result == true) throw new BadRequestException("Такой пльзователь уже есть");

            CreatePasswordHash(UserInformationBlo.Password!, out byte[] passwordHash, out byte[] passwordSalt);

            UserRto user = new UserRto()
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PhoneNumber = number,
                PhoneNumberPrefix = numberPrefix
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();
            UserInformationBlo userInfoBlo = await ConvertToUserInformationAsync(user);
            return userInfoBlo;
        }

        public Task SignOutAsync(SignOutContext context)
        {
            throw new NotImplementedException();
        }

        public async Task<UserInformationBlo> Update(UserUpdateBlo userUpdateBlo)
        {
            UserRto user = await _context.Users.FirstOrDefaultAsync(y => y.PhoneNumberPrefix == userUpdateBlo.CurrentPhoneNumber && y.PhoneNumber == userUpdateBlo.CurrentPhoneNumderPrefix && y.Password == userUpdateBlo.CurrentPassword);

            if (user == null) throw new NotFoundException("Такого пользователя нет");

            user.Password = userUpdateBlo.Password;
            user.Email = userUpdateBlo.Email;
            user.Login = userUpdateBlo.Login;
            user.IsBoy = userUpdateBlo.IsBoy;
            user.PhoneNumber = userUpdateBlo.PhoneNumder;
            user.PhoneNumberPrefix = userUpdateBlo.PhoneNumderPrefix;
            user.FirstName = userUpdateBlo.FirstName;
            user.LastName = userUpdateBlo.LastName;
            user.Patronymic = userUpdateBlo.Patronymic;
            user.Birthday = userUpdateBlo.Birthday;
            user.AvatarUrl = userUpdateBlo.AvatarUrl;

            UserInformationBlo userInfoBlo = await ConvertToUserInformationAsync(user);
            return userInfoBlo;

        }

        private async Task<UserInformationBlo> ConvertToUserInformationAsync(UserRto userRto)
        {
            if (userRto == null) throw new ArgumentNullException(nameof(userRto));

            UserInformationBlo userInformationBlo = _mapper.Map<UserInformationBlo>(userRto);

            return userInformationBlo;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(UserRto user, byte[] securityToken)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Nickname)
            };

            var key = new SymmetricSecurityKey(securityToken);

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
