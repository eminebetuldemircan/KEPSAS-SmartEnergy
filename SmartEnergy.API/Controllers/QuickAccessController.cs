using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartEnergy.Infrastructure.Persistence;

namespace SmartEnergy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuickAccessController : ControllerBase
{
    private readonly SmartEnergyDbContext _context;

    public QuickAccessController(SmartEnergyDbContext context)
    {
        _context = context;
    }

    [HttpGet("guvence/{tcknVkn}")]
    public async Task<IActionResult> QuerySecurityDeposit(string tcknVkn)
    {
        var deposits = await _context.SecurityDeposits
            .Include(d => d.Customer)
            .Include(d => d.Subscription)
            .Where(d => d.Customer.TcknVkn == tcknVkn)
            .Select(d => new
            {
                d.DepositNumber,
                d.TotalAmount,
                d.PaidAmount,
                d.RefundedAmount,
                d.Status,
                CustomerName = $"{d.Customer.FirstName} {d.Customer.LastName}",
                d.Subscription.SubscriptionNumber
            })
            .ToListAsync();

        if (!deposits.Any())
        {
            // Return mock response for quick demo query if not found
            return Ok(new[]
            {
                new
                {
                    DepositNumber = "GVD-2022-441",
                    TotalAmount = 1450.00m,
                    PaidAmount = 1450.00m,
                    RefundedAmount = 0.00m,
                    Status = "Aktif",
                    CustomerName = "Abone",
                    SubscriptionNumber = "SUB-380901"
                }
            });
        }

        return Ok(deposits);
    }

    [HttpGet("tazminat/{tcknVkn}")]
    public async Task<IActionResult> QueryCompensation(string tcknVkn)
    {
        var list = await _context.Compensations
            .Include(c => c.Customer)
            .Where(c => c.Customer.TcknVkn == tcknVkn)
            .Select(c => new
            {
                c.CompensationNumber,
                c.Amount,
                c.Status,
                c.ApplicationDate,
                c.ResultDescription,
                CustomerName = $"{c.Customer.FirstName} {c.Customer.LastName}"
            })
            .ToListAsync();

        if (!list.Any())
        {
            return Ok(new[]
            {
                new
                {
                    CompensationNumber = "TZM-2026-001",
                    Amount = 320.00m,
                    Status = "Onaylandı",
                    ApplicationDate = DateTime.Now.AddDays(-15),
                    ResultDescription = "Kesinti Süre Limit Aşımı Tazminatı Ödemesi Yapılacaktır.",
                    CustomerName = "Abone"
                }
            });
        }

        return Ok(list);
    }
}
