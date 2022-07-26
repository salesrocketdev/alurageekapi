using alurageekapi.Models;
using alurageekapi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace alurageekapi.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductsService _productsService;

    public ProductsController(ProductsService productsService) =>
        _productsService = productsService;    

    [HttpGet]
    public async Task<CommandResult> Get() =>
        await _productsService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Product>> Get(string id)
    {   
        var product = await _productsService.GetAsync(id);

        if (product is null)
        {
            return NotFound("Produto não encontrado.");
        }

        return product;
    }

    [HttpGet("Filter/Title/{title}")]
    public async Task<List<Product>> GetByTitle(string title) =>
        await _productsService.GetByTitleAsync(title);

    [HttpGet("Filter/Category/{category}")]
    public async Task<List<Product>> GetByCategory(string category) =>
        await _productsService.GetCategoryAsync(category);    

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> Post(Product newProduct)
    {
        await _productsService.CreateAsync(newProduct);

        return CreatedAtAction(nameof(Get), new { id = newProduct.Id }, newProduct);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Product updatedProduct)
    {
        var product = await _productsService.GetAsync(id);

        if (product is null)
        {
            return NotFound("Produto não encontrado.");
        }

        updatedProduct.Id = product.Id;

        await _productsService.UpdateAsync(id, updatedProduct);

        return Content("Produto atualizado com sucesso.");
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var product = await _productsService.GetAsync(id);

        if (product is null)
        {
            return NotFound("Produto não encontrado.");
        }

        await _productsService.RemoveAsync(id);

        return Content("Produto deletado com sucesso.");
    } 
}