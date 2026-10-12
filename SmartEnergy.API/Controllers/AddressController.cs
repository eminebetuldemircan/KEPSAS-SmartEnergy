using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Application.DTOs.Addresses;
using SmartEnergy.Application.Services;

namespace SmartEnergy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var address = await _addressService.GetByIdAsync(id);

        if (address is null)
            return NotFound();

        return Ok(address);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var addresses = await _addressService.GetByUserIdAsync(userId);

        return Ok(addresses);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAddressRequest request)
    {
        var createdAddress = await _addressService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdAddress.Id },
            createdAddress);
    }
}