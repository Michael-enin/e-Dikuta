using System.Data.Entity;
using API.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly RepositoryContext _productRepository;
        public ProductsController(RepositoryContext repositoryContext)
        {
            _productRepository = repositoryContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            return await _productRepository.Products.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return Ok(await _productRepository.Products.FindAsync(id));
        }


    }

}