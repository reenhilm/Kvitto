using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kvitto.Core.Entities;
using Kvitto.Data.Data;
using AutoMapper;
using Kvitto.Core.Repositories;
using Kvitto.Common.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace Kvitto.Api.Controllers
{
    [Route("api/Receipts")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private readonly IUoW uow;
        private readonly IMapper mapper;

        public ReceiptsController(IUoW uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        // GET: api/Receipts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptDto>>> GetReceipts()
        {
            var receipts = await uow.ReceiptRepository.GetAllReceipts();
            var dto = mapper.Map<IEnumerable<ReceiptDto>>(receipts);
            return Ok(dto);
        }

        // GET: api/Receipts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptDto>> GetReceipt(int id)
        {
            var receipt = await uow.ReceiptRepository.FindAsync(id);
            var dto = mapper.Map<ReceiptDto>(receipt);

            if (receipt == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }

        // PUT: api/Receipts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ReceiptDto>> PutReceipt(int id, ReceiptDto receiptDto)
        {
            if (id != receiptDto.Id)
            {
                return BadRequest();
            }

            var receipt = mapper.Map<Receipt>(receiptDto);
            uow.ReceiptRepository.Update(receipt);

            ReceiptDto dto;
            try
            {
                await uow.CompleteAsync();
                dto = mapper.Map<ReceiptDto>(receipt);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ReceiptExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(dto);
        }

        // PATCH: api/Receipts/5
        [HttpPatch("{receiptId}")]
        public async Task<ActionResult<ReceiptDto>> PatchReceipt(int receiptId, JsonPatchDocument<ReceiptDto> patchDocument)
        {
            var receiptEntity = await uow.ReceiptRepository.FindAsync(receiptId);
            if (receiptEntity is null)
            {
                return BadRequest();
            }

            var dto = mapper.Map<ReceiptDto>(receiptEntity);

            patchDocument.ApplyTo(dto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(dto))
                return BadRequest(ModelState);

            mapper.Map(dto, receiptEntity);

            try
            {
                await uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ReceiptExists(receiptId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(dto);
        }

        // POST: api/Receipts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReceiptDto>> PostReceipt(ReceiptDto insertReceipt)
        {
            var enitityReceipt = mapper.Map<Receipt>(insertReceipt);
            uow.ReceiptRepository.Add(enitityReceipt);
            await uow.CompleteAsync();

            //halvonödig mappning tillbaka? kan bara skicka tillbaka insertReceipt, men skulle kunna vara om ReceiptRepository ändrar mer än Id i framtiden
            var dto = mapper.Map<ReceiptDto>(enitityReceipt);

            return CreatedAtAction("GetReceipt", new { id = enitityReceipt.Id }, dto);
        }

        // DELETE: api/Receipts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceipt(int id)
        {
            var receipt = await uow.ReceiptRepository.FindAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }

            uow.ReceiptRepository.Remove(receipt);
            await uow.CompleteAsync();

            return NoContent();
        }

        private async Task<bool> ReceiptExists(int id)
        {
            return await uow.ReceiptRepository.AnyAsync(id);
        }
    }
}
