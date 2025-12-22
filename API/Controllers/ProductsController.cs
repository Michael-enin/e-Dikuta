using Core.Entities;
using Core.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController(IProductRepository productRepository) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            return Ok(await productRepository.GetProductsAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return Ok(await productRepository.GetProdctByIdAsync(id));
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            productRepository.AddProduct(product);
            if (await productRepository.SaveChangesAsync())
            {
                return CreatedAtAction("Get Product", new { id = product.Id }, product);
            }
            return BadRequest("Problem in creating product");
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id))
                return BadRequest("Cannot update product with Id  ");
            productRepository.updateProduct(product);
            if (await productRepository.SaveChangesAsync())
            {
                return NoContent();
            }
            return BadRequest("problem occured updating product");
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await productRepository.GetProdctByIdAsync(id);
            if (product == null)
                return NotFound();

            else
            {
                productRepository.deleteProduct(product);
            }
            if (await productRepository.SaveChangesAsync())
            {
                return NoContent();
            }
            return BadRequest("problem occured deleting product");
        }
        bool ProductExists(int id)
        {
            return productRepository.ProductExists(id);
        }

    }

}