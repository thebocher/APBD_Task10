using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using APBD_Task10.App.DTOs.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APBD_Task10.Infrastructure;
using APBD_Task10.Infrastructure.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NuGet.Packaging.Signing;

namespace APBD_Task10.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly MasterContext _context;
        private readonly PasswordHasher<Account> _passwordHasher = new ();

        public AccountController(MasterContext context)
        {
            _context = context;
        }

        // GET: api/AccountController1
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        // GET: api/AccountController1/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            
            if (account == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin"))
                if (!User.Claims.Any(c => c.Type == JwtRegisteredClaimNames.Name
                                          && c.Value == account.Username))
                    return Forbid();

            return account;
        }

        // PUT: api/AccountController1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAccount(int id, UpdateAccountDto dto)
        {
            if (!User.IsInRole("Admin"))
                if (!User.Claims.Any(c => c.Type == JwtRegisteredClaimNames.Name
                                          && c.Value == dto.Username))
                    return Forbid();

            var account = new Account
            {
                Id = id,
                Username = dto.Username,
                Password = dto.Password,
            };
            
            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AccountController1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetAccountDto>> PostAccount(RegisterRequestDto dto)
        {
            
            var account = new Account()
            {
                Username = dto.Username,
                RoleId = dto.RoleId,
                EmployeeId = dto.EmployeeId
            };
            
            string passwrod = _passwordHasher.HashPassword(account, dto.Password);
            account.Password = passwrod;
            
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var resultDto = new GetAccountDto
            {
                Id = account.Id,
                Username = account.Username,
                RoleId = account.RoleId,
                EmployeeId = account.EmployeeId,
                Password = account.Password
            };

            return CreatedAtAction("GetAccount", new { id = account.Id }, resultDto);
        }

        // DELETE: api/AccountController1/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }

        // [HttpPost("create-admin")]
        // public IResult createAdmin()
        // {
        //     var person = new Person()
        //     {
        //
        //     };
        //     var account = new Account()
        //     {
        //         Username = "admin",
        //
        //     };
        //     
        //     
        // }
    }
}
