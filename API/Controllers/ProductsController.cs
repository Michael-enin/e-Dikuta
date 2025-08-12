using Microsoft.AspNetCore.Mvc;

namespace API
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public string GetProducts()
        {
            return "these are products";
        }
        [HttpGet("{id}")]
        public string GetProduct(int id)
        {
            return "this is a product";
        }


    }

}