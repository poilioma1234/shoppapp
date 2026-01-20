using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductApp.Application.DTOs.Category;
using ProductApp.Application.Interfaces.Services;
using ProductApp.Domain.Entities;

namespace ProductApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CategoriesController(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CategoryViewDto>>(categories);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound(new { message = "Không tìm thấy danh mục" });

        var dto = _mapper.Map<CategoryViewDto>(category);
        return Ok(new ProductApp.Application.Models.Responses.Response<CategoryViewDto>
        {
            Data = dto,
            Success = true
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateDto createDto)
    {
        var category = _mapper.Map<Category>(createDto);
        await _categoryService.AddAsync(category);
        await _categoryService.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto updateDto)
    {
        var existingCategory = await _categoryService.GetByIdAsync(id);
        if (existingCategory == null) return NotFound();

        _mapper.Map(updateDto, existingCategory);
        await _categoryService.UpdateAsync(existingCategory);
        await _categoryService.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound();

        await _categoryService.DeleteAsync(id);
        await _categoryService.SaveChangesAsync();
        return NoContent();
    }
}
