using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        IConfiguration Configuration;

        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims, int companyId)
        {
            AccessToken token = new AccessToken();

            //Security Key'in simetriğini alalım
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:SecurityKey"]));

            //Şifrelenmiş kimliği oluşturuyoruz
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //bu kısımda bir hata varmış ama tam anlamadım benim başka jwt kodlarıyla değiştirdim şu anda sağlamca token veriyor. aşağıdaki kısmıda düzeltirsek işlem tmm 

            //Token ayarlarını yapıyoruz
            token.Expiration = DateTime.Now.AddMinutes(60);
            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: Configuration["Token:Issuer"],
                audience: Configuration["Token:Audience"],
                expires: token.Expiration,
                claims: SetClaims(user, operationClaims, companyId),
                notBefore: DateTime.Now,
                signingCredentials: signingCredentials
                );

            //Token oluşturucu sınıfından bir örnek alalım
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            //Token üretelim
            token.Token = jwtSecurityTokenHandler.WriteToken(securityToken);

            //Refresh token üretelim
            //token = CreateRefreshToken();
            return token;
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                return Convert.ToBase64String(number);
            }
        }

        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims, int companyId)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentitfier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.Name}");
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());
            claims.AddCompany(companyId.ToString());
            //claims.AddCompanyName(companyName);
            //sanırım daha buraya gelmediğinden bu kızardı buraya gleince bunu açarsın. bir kere daha token alalım sonra bir kayıt daha yapalım işlem tmm 
            return claims;
        }
    }
}
