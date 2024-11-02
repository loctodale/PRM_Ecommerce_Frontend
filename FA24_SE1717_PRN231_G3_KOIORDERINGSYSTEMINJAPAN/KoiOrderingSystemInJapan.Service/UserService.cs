using KoiOrderingSystemInJapan.Common;
using KoiOrderingSystemInJapan.Data;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data.Repositories;
using KoiOrderingSystemInJapan.Data.Request.Auths;
using KoiOrderingSystemInJapan.Service.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KoiOrderingSystemInJapan.Service
{
    public interface IUserService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(Guid id);
        Task<IBusinessResult> GetByRole(int roleNum);
        Task<IBusinessResult> Create(User user);
        Task<IBusinessResult> Update(User user);
        Task<IBusinessResult> Save(User user);
        Task<IBusinessResult> DeleteById(Guid id);
        IBusinessResult DecodeToken(string token);
        Task<IBusinessResult> Login(string usernameOrEmail, string password);
        Task<IBusinessResult> AddUser(User user);
    }
    public class UserService : IUserService
    {
        private readonly IConfiguration configuration;
        private readonly UnitOfWork unitOfWork;
        private readonly DateTime _expirationTime = DateTime.Now.AddHours(1);

        public UserService(IConfiguration _configuration)
        {
            configuration = _configuration;

            unitOfWork ??= new UnitOfWork();
        }
        public Task<IBusinessResult> Create(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IBusinessResult> DeleteById(Guid id)
        {
            try
            {
                var u = await unitOfWork.User.GetByIdAsync(id);

                if (u == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }
                else
                {
                    bool rs = await unitOfWork.User.RemoveAsync(u);

                    if (rs)
                    {
                        return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                    }
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetAll()
        {
            var users = await unitOfWork.User.GetAllAsync();
            if (users == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<User>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, users);
            }
        }

        public async Task<IBusinessResult> GetById(Guid id)
        {
            try
            {
                var u = await unitOfWork.User.GetByIdAsync(id);

                if (u == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new User());
                }
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, u);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetByRole(int role)
        {
            try
            {
                var u =  await unitOfWork.User.GetByRole(role);

                if (u == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<User>());
                }
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, u);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> Save(User user)
        {
            try
            {
                int result = -1;

                var u = unitOfWork.User.GetById(user.Id);

                if (u != null)
                {
                    result = await unitOfWork.User.UpdateAsync(user);

                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                    }
                }
                else
                {
                    result = await unitOfWork.User.CreateAsync(user);

                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                    }
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public Task<IBusinessResult> Update(User user)
        {
            throw new NotImplementedException();
        }


        public IBusinessResult DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            // Kiểm tra nếu token không hợp lệ
            if (!handler.CanReadToken(token))
            {
                throw new ArgumentException("Token không hợp lệ", nameof(token));
            }

            // Giải mã token
            var jwtToken = handler.ReadJwtToken(token);

            // Truy xuất các thông tin từ token
            var id = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            var name = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            var role = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            var exp = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Expiration).Value;


            // Tạo đối tượng DecodedToken
            var decodedToken = new DecodedToken
            {
                Id = id,
                Name = name,
                Role = role,
                Exp = long.Parse(exp),
            };

            return new BusinessResult(Const.SUCCESS_READ_CODE, "Decoded to get user", decodedToken);
        }
        private (string token, string expiration) CreateToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.Expiration, new DateTimeOffset(_expirationTime).ToUnixTimeSeconds().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                configuration.GetSection("JWT:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


            var token = new JwtSecurityToken(
                claims: claims,
                expires: _expirationTime,
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return (jwt, _expirationTime.ToString("o")); // Trả về token và thời gian hết hạn
        }
        public async Task<IBusinessResult> Login(string usernameOrEmail, string password)
        {
            var user = await unitOfWork.User.FindByEmailOrUsername(usernameOrEmail);

            //check username 
            if (user == null) return new BusinessResult(Const.SUCCESS_READ_CODE, "Not find account", new User());

            //check password
            if (!user.Password.Equals(password))
                return new BusinessResult(Const.SUCCESS_READ_CODE, "Not match password", new User());

            var (token, expiration) = CreateToken(user);

            return new BusinessResult(Const.SUCCESS_READ_CODE, "Welcom, KOITHE SYSTEM IN JAPANKOREAN", new LoginResponse(token, expiration));
        }

        public async Task<IBusinessResult> AddUser(User user)
        {
            var username = await unitOfWork.User.FindByEmailOrUsername(user.Username);
            if (username == null)
            {
                user.Role = ConstEnum.Role.Customer;
                return await Save(user);
            }
            else
            {
                return new BusinessResult(Const.FAIL_CREATE_CODE, "Username đã tồn tại");
            }
        }
    }
}
