using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Stock.Dtos;
using Stock.Entities;
using Stock.Repository.IRepository;


namespace Stock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductsRepository _productsRepository;

        public StockController(IMapper mapper, IProductsRepository productsRepository)
        {
            _mapper = mapper;
            _productsRepository = productsRepository;
        }

       

        [HttpPost]
        public async Task<ActionResult<Products>> CreateProduct([FromBody] ProductsDto productDto)
        {
            var productModel = _mapper.Map<Products>(productDto);
            await _productsRepository.CreateProduct(productModel);
            await _productsRepository.SaveChanges();
            var createdProduct = _mapper.Map<ProductsDto>(productModel);

            return Ok(createdProduct);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductsDto>>> GetProducts()
        {
            var products = await _productsRepository.GetProducts();
            return Ok(_mapper.Map<IEnumerable<ProductsDto>>(products));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductsDto>> GetProductById(int id)
        {
            var product = await _productsRepository.GetProductById(id);
            return Ok(_mapper.Map<ProductsDto>(product));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductsDto>> UpdateProduct(int id, ProductsDto productsDto)
        {
            var entity = await _productsRepository.GetProductById(id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Id = id;

            await _productsRepository.UpdateProduct(entity);
            await _productsRepository.SaveChanges();

            return Ok(_mapper.Map<ProductsDto>(entity));

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productsRepository.DeleteProduct(id);
            await _productsRepository.SaveChanges();
            return NoContent();
        }

    }
}
