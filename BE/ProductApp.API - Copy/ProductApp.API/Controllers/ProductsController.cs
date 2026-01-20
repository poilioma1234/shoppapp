using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductApp.Application.DTOs.Product;
using ProductApp.Application.Interfaces.Services;
using ProductApp.Application.Validators;
using ProductApp.Domain.Entities;

namespace ProductApp.API.Controllers;

[Route("api/[controller]/[action]")] 
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        var result = _mapper.Map<IEnumerable<ProductViewDto>>(products);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound(new { message = "Không tìm thấy sản phẩm" });

        var dto = _mapper.Map<ProductViewDto>(product);
        return Ok(new ProductApp.Application.Models.Responses.Response<ProductViewDto>
        {
            Data = dto,
            Success = true
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto createDto)
    {
        var validator = new ProductValidator();
        var validationResult = await validator.ValidateAsync(createDto);

        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var product = _mapper.Map<Product>(createDto);

        await _productService.AddAsync(product);
        await _productService.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductCreateDto updateDto)
    {
        var existingProduct = await _productService.GetByIdAsync(id);
        if (existingProduct == null) return NotFound();

        _mapper.Map(updateDto, existingProduct);

        await _productService.UpdateAsync(existingProduct);
        await _productService.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();

        await _productService.DeleteAsync(id);
        await _productService.SaveChangesAsync();
        return NoContent();
    }
}