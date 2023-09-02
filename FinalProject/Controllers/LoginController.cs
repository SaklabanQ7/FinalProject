using FinalProject.Application.Repositories;
using FinalProject.Domain.Entities;
using FinalProject.Domain.Entities.ViewModel;
using FinalProject.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMemberReadRepository _memberReadRepository;
        private readonly IMemberWriteRepository _memberWriteRepository;
        private readonly ICompanyReadRepository _companyReadRepository;
        private readonly ICompanyWriteRepository _companyWriteRepository;
        private readonly OnlineEventsDbContext _context;
        private readonly TokenOption tokenOption;
        

        public LoginController(IMemberReadRepository memberReadRepository, IMemberWriteRepository memberWriteRepository, OnlineEventsDbContext context, ICompanyReadRepository companyReadRepository, ICompanyWriteRepository companyWriteRepository, IOptions<TokenOption> options)
        {
            _companyReadRepository= companyReadRepository;
            _companyWriteRepository= companyWriteRepository;
            _context = context;
            tokenOption = options.Value;
            _memberReadRepository = memberReadRepository;
            _memberWriteRepository = memberWriteRepository;
        }
        
        [HttpPost("MemberLogin")]
        public IActionResult MemberLogin(LoginModel loginModel)
        {
            Member member = _context.Members.FirstOrDefault(a=> a.Password==loginModel.Password && a.MailAddress==loginModel.MailAddress);
            if(member!=null)
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, member.MailAddress));
                claims.Add(new Claim(ClaimTypes.Name, member.Name));
                claims.Add(new Claim(ClaimTypes.Role, member.Title));



                JwtSecurityToken securityToken = new JwtSecurityToken(
                    issuer:tokenOption.Issuer,
                    audience:tokenOption.Audience,
                    claims:claims,
                    expires:DateTime.Now.AddDays(tokenOption.Expiration),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOption.SecretKey)),SecurityAlgorithms.HmacSha256)
                    );
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                string userToken = tokenHandler.WriteToken(securityToken);

                return Ok(userToken);
            }
            else
            {
                return NotFound();

            }
        }

        [HttpPost("CompanyLogin")]
        public IActionResult CompanyLogin(LoginModel loginModel)
        {
            Company company = _context.Companies.FirstOrDefault(a => a.Password == loginModel.Password && a.MailAddress == loginModel.MailAddress);
            if (company != null)
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, company.MailAddress));
                claims.Add(new Claim(ClaimTypes.Name, company.CompanyName));
                claims.Add(new Claim(ClaimTypes.Role, "Company"));



                JwtSecurityToken securityToken = new JwtSecurityToken(
                    issuer: tokenOption.Issuer,
                    audience: tokenOption.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(tokenOption.Expiration),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOption.SecretKey)), SecurityAlgorithms.HmacSha256)
                    );
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                string userToken = tokenHandler.WriteToken(securityToken);

                return Ok(userToken);
            }
            else
            {
                return NotFound();

            }
        }
        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount(Member member)
        {
            Member member2 = _context.Members.FirstOrDefault(a => a.MailAddress == member.MailAddress);

            if (!ModelState.IsValid || member.Title != null || member2 != null)
            {
                if (member.Title != null)
                    return BadRequest("Title cannot be entered ");
                else if (member2 != null)
                    return BadRequest("This email already taken");
                else
                    return BadRequest();
            }
            else
            {
                member.Title = "Member";
                await _memberWriteRepository.AddAsync(member);
                _memberWriteRepository.Save();

                return Ok("  👍 ");
            }
        }
        [HttpPost("CreateCompany")]
        public async Task<IActionResult> CreateCompany(Company company)
        {
            Company company1 = _context.Companies.FirstOrDefault(a => a.MailAddress == company.MailAddress);
            if (!ModelState.IsValid ||  company1 != null)
            {
               
                if (company1 != null)
                    return BadRequest("This email already taken");
                else
                    return BadRequest();
            }
            else
            {
                
                await _companyWriteRepository.AddAsync(company);
                _companyWriteRepository.Save();

                return Ok("  👍 ");
            }
        }


    }
}
