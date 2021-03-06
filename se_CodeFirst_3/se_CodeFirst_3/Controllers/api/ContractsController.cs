﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using se_CodeFirst_3.Models;

namespace se_CodeFirst_3.Controllers.api
{

#if DEBUG

#else
    [Authorize(Roles = "Administrator,Secretary")]
#endif    
    public class ContractsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Contracts
        public IQueryable<Contract> GetContracts()
        {
            return db.Contracts;
        }

        // GET: api/Contracts/5
        [ResponseType(typeof(Contract))]
        public async Task<IHttpActionResult> GetContract(int id)
        {
            Contract contract = await db.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            return Ok(contract);
        }

        // PUT: api/Contracts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutContract(int id, Contract contract)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contract.Id)
            {
                return BadRequest();
            }

            db.Entry(contract).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContractExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Contracts
        [ResponseType(typeof(Contract))]
        public async Task<IHttpActionResult> PostContract(Contract contract)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Contracts.Add(contract);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = contract.Id }, contract);
        }

        // DELETE: api/Contracts/5
        [ResponseType(typeof(Contract))]
        public async Task<IHttpActionResult> DeleteContract(int id)
        {
            Contract contract = await db.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            db.Contracts.Remove(contract);
            await db.SaveChangesAsync();

            return Ok(contract);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContractExists(int id)
        {
            return db.Contracts.Count(e => e.Id == id) > 0;
        }
    }
}