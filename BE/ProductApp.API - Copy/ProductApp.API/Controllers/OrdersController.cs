using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductApp.Application.DTOs.Order;
using ProductApp.Application.Interfaces.Services;
using ProductApp.Domain.Entities;

namespace ProductApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrdersController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderService.GetAllAsync();
        var result = _mapper.Map<IEnumerable<OrderViewDto>>(orders);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null) return NotFound(new { message = "Không tìm thấy đơn hàng" });

        var dto = _mapper.Map<OrderViewDto>(order);
        return Ok(new ProductApp.Application.Models.Responses.Response<OrderViewDto>
        {
            Data = dto,
            Success = true
        });
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var orders = await _orderService.GetByUserIdAsync(userId);
        var result = _mapper.Map<IEnumerable<OrderViewDto>>(orders);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderCreateDto createDto)
    {
        try
        {
            var order = _mapper.Map<Order>(createDto);
            var orderItems = _mapper.Map<List<OrderItem>>(createDto.OrderItems);

            var createdOrder = await _orderService.CreateOrderAsync(order, orderItems);
            await _orderService.SaveChangesAsync();

            var orderDto = await _orderService.GetByIdAsync(createdOrder.Id);
            var result = _mapper.Map<OrderViewDto>(orderDto);
            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] OrderUpdateDto updateDto)
    {
        var existingOrder = await _orderService.GetByIdAsync(id);
        if (existingOrder == null) return NotFound();

        _mapper.Map(updateDto, existingOrder);
        await _orderService.UpdateAsync(existingOrder);
        await _orderService.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null) return NotFound();

        await _orderService.DeleteAsync(id);
        await _orderService.SaveChangesAsync();
        return NoContent();
    }
}
