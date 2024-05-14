using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sale.Dtos;
using Sale.Entities;
using Sale.Repository.IRepository;

namespace Sale.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISalesRepository _salesRepository;

        public SalesController(IMapper mapper, ISalesRepository salesRepository)
        {
            _mapper = mapper;
            _salesRepository = salesRepository;
        }

        [HttpPost]
        public async Task<ActionResult<Sales>> CreateSale([FromBody] CreateSalesDto salesDto) 
        {
            var saleModel = _mapper.Map<Sales>(salesDto);
            await _salesRepository.CreateSale(saleModel);
            await _salesRepository.SaveChanges();
            var createdSale = _mapper.Map<SalesDto>(saleModel);

            return Ok(createdSale);
        }

        [HttpGet]
        public async Task<ActionResult<SalesDto>> GetSales()
        {
            var sales = await _salesRepository.GetSales();
            return Ok(_mapper.Map<IEnumerable<SalesDto>>(sales));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalesDto>> GetSaleById(int id)
        {
            var sale = await _salesRepository.GetSaleById(id);
            if(sale == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<SalesDto>(sale));
        }

        [HttpGet("Product/id")]
        public async Task<ActionResult<SalesDto>> GetSaleByProduct(int id)
        {
            var sales = await _salesRepository.GetSalesByProduct(id);
            return Ok(_mapper.Map<IEnumerable<SalesDto>>(sales));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SalesDto>> UpdateSale(int id, SalesDto salesDto)
        {
            var entity = await _salesRepository.GetSaleById(id);
            
            if(entity == null)
            {
                return NotFound();
            }

            entity.Id = id;

            await _salesRepository.UpdateSale(entity);
            await _salesRepository.SaveChanges();
            return Ok(_mapper.Map<SalesDto>(entity));

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSale(int id)
        {
            await _salesRepository.DeleteSale(id);
            await _salesRepository.SaveChanges();
            return NoContent();
        }
    }
}
