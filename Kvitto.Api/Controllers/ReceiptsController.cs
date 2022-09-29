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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

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
        [HttpHead]
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
        public async Task<ActionResult<ReceiptDto>> PutReceipt(int id, ReceiptForUpdateDto receipt)
        {
            if (receipt is null)
            {
                throw new ArgumentNullException(nameof(receipt));
            }

            var receiptFromRepo = await uow.ReceiptRepository.GetReceipt(id);

            if (receiptFromRepo == null)
            {
                var receiptToAdd = mapper.Map<Receipt>(receipt);
                receiptToAdd.Id = id;

                uow.ReceiptRepository.Add(receiptToAdd);
                await uow.CompleteAsync();

                var receiptToReturn = mapper.Map<ReceiptDto>(receiptToAdd);

                return CreatedAtAction("GetReceipt",
                    new { id = receiptToReturn.Id },
                    receiptToReturn);
            }

            // map the entity to a CourseForUpdateDto
            // apply the updated field values to that dto
            // map the CourseForUpdateDto back to an entity
            mapper.Map(receipt, receiptFromRepo);

            uow.ReceiptRepository.Update(receiptFromRepo);
            try
            {
                await uow.CompleteAsync();
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
            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetReceiptsOptions()
        {
            Response.Headers.Add("Allow", "GET,PUT,PATCH,OPTIONS,POST");
            return Ok();
        }

        // PATCH: api/Receipts/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<ReceiptDto>> PartiallyUpdateReceipt(int id, JsonPatchDocument<ReceiptForUpdateDto> patchDocument)
        {
            var receiptFromRepo = await uow.ReceiptRepository.FindAsync(id);
            if (receiptFromRepo is null)
            {
                var receiptDto = new ReceiptForUpdateDto();
                patchDocument.ApplyTo(receiptDto, ModelState);

                if (!TryValidateModel(receiptDto))
                {
                    return ValidationProblem(ModelState);
                }

                var receiptToAdd = mapper.Map<Receipt>(receiptDto);
                receiptToAdd.Id = id;

                uow.ReceiptRepository.Add(receiptToAdd);
                await uow.CompleteAsync();

                var receiptToReturn = mapper.Map<ReceiptDto>(receiptToAdd);

                return CreatedAtAction("GetReceipt",
                    new { id = receiptToReturn.Id },
                    receiptToReturn);
            }
            var receiptToPatch = mapper.Map<ReceiptForUpdateDto>(receiptFromRepo);
            // add validation
            patchDocument.ApplyTo(receiptToPatch, ModelState);

            if (!TryValidateModel(receiptToPatch))
                return ValidationProblem(ModelState);


            mapper.Map(receiptToPatch, receiptFromRepo);

            uow.ReceiptRepository.Update(receiptFromRepo);
            try
            {
                await uow.CompleteAsync();
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
            return NoContent();
        }

        // POST: api/Receipts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReceiptDto>> CreateReceipt(ReceiptForCreationDto insertReceipt)
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

        public override ActionResult ValidationProblem(
    [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
