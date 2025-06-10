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
        public async Task<ActionResult<IEnumerable<GetAccountListItemDto>>> GetAccounts()
        {
            return await _context.Account.Select(a => new GetAccountListItemDto
            {
                Id = a.Id,
                Username = a.Username,
            }).ToListAsync();
        }

        // GET: api/AccountController1/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GetAccountDto>> GetAccount(int id)
        {
            try
            {
                var account = await _context.Account
                    .Include(a => a.Role)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (account == null)
                {
                    return NotFound();
                }

                if (!User.IsInRole("Admin"))
                    if (!User.Claims.Any(c => c.Type == JwtRegisteredClaimNames.Name
                                              && c.Value == account.Username))
                        return Forbid();

                return new GetAccountDto()
                {
                    Username = account.Username,
                    Role = account.Role.Name
                };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            } 

        }

        // PUT: api/AccountController1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAccount(int id, UpdateAccountDto dto)
        {
            try
            {
                var account = _context.Account.FirstOrDefault(a => a.Id == id);
                if (account == null)
                {
                    return NotFound();
                }

                if (!User.IsInRole("Admin"))
                {

                    if (!User.Claims.Any(c => c.Type == JwtRegisteredClaimNames.Name
                                              && c.Value == account.Username))
                        return Forbid();
                }

                account.Username = dto.Username;
                account.Password = _passwordHasher.HashPassword(account, dto.Password);

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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/AccountController1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetAccountDto>> PostAccount(RegisterRequestDto dto)
        {
            try
            {
                var account = new Account()
                {
                    Username = dto.Username,
                    RoleId = dto.RoleId,
                    EmployeeId = dto.EmployeeId
                };

                string passwrod = _passwordHasher.HashPassword(account, dto.Password);
                account.Password = passwrod;

                _context.Account.Add(account);

                await _context.SaveChangesAsync();

                var resultDto = new GetAccountDto
                {
                    Username = account.Username,
                    Role = account.Role.Name
                };

                return CreatedAtAction("GetAccount", new { id = account.Id }, resultDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/AccountController1/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                var account = await _context.Account.FindAsync(id);
                if (account == null)
                {
                    return NotFound();
                }

                _context.Account.Remove(account);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.Id == id);
        }
    }
}
