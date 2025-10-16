using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Core.Dto;
using PersonalFinanceTracker.Core.Interfaces;

namespace PersonalFinanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {

        public readonly ITransactionService _service;

        public TransactionsController(ITransactionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAll()
        {
            var transactions = await _service.GetAllAsync();

            return Ok(transactions);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TransactionDto>> GetById(int id)
        {
            var transaction = await _service.GetByIdAsync(id);
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> Post(CreateTransactionDto dto)
        {
            var result = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new {id = result.TransactionId}, result);

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TransactionDto>> Put(int id, UpdateTransactionDto dto)
        {
            try
            {
                var updatedTx = await _service.UpdateAsync(id, dto);
                return Ok(updatedTx);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent(); // 204 response is more standard for DELETE
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

    }
}
