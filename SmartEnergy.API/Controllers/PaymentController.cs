using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartEnergy.Domain.Entities;
using SmartEnergy.Infrastructure.Persistence;

namespace SmartEnergy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly SmartEnergyDbContext _context;

    public PaymentController(SmartEnergyDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> MakePayment([FromBody] PaymentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.BillNumber))
        {
            return BadRequest("Fatura numarası zorunludur.");
        }

        var bill = await _context.Bills
            .Include(b => b.Subscription)
            .FirstOrDefaultAsync(b => b.BillNumber == request.BillNumber);

        if (bill == null)
        {
            return NotFound("Belirtilen fatura bulunamadı.");
        }

        // Mark Bill as Paid
        bill.MarkAsPaid();

        // Create Payment record
        var payment = new Payment(
            bill.Id,
            request.Amount > 0 ? request.Amount : bill.TotalAmount,
            request.PaymentMethod ?? "Online Kredi Kartı",
            $"TXN-{Random.Shared.Next(100000, 999999)}"
        );
        _context.Payments.Add(payment);

        // Add Notification to Customer
        var customerId = bill.Subscription?.User?.CustomerId;
        if (customerId.HasValue && customerId.Value != Guid.Empty)
        {
            var notif = new Notification(
                title: "Fatura Ödemeniz Alındı",
                message: $"{bill.BillNumber} numaralı faturanıza ait {payment.Amount:N2} ₺ ödeme online olarak başarıyla gerçekleşmiştir.",
                type: "Ödeme",
                customerId: customerId.Value
            );
            _context.Notifications.Add(notif);
        }

        await _context.SaveChangesAsync();

        return Ok(new
        {
            Success = true,
            Message = "Ödeme başarıyla gerçekleştirildi.",
            TransactionId = payment.TransactionId,
            PaymentDate = payment.PaymentDate
        });
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetPaymentHistory([FromQuery] Guid? customerId)
    {
        var query = _context.Payments
            .Include(p => p.Bill)
            .ThenInclude(b => b.Subscription)
            .ThenInclude(s => s.User)
            .AsQueryable();

        if (customerId.HasValue && customerId.Value != Guid.Empty)
        {
            query = query.Where(p => p.Bill.Subscription.User.CustomerId == customerId.Value);
        }

        var list = await query
            .OrderByDescending(p => p.PaymentDate)
            .Select(p => new
            {
                p.Id,
                p.BillId,
                BillNumber = p.Bill.BillNumber,
                p.Amount,
                p.PaymentMethod,
                p.TransactionId,
                p.Status,
                p.PaymentDate
            })
            .ToListAsync();

        return Ok(list);
    }
}

public class PaymentRequest
{
    public string BillNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? CardNumber { get; set; }
    public string? PaymentMethod { get; set; }
}
