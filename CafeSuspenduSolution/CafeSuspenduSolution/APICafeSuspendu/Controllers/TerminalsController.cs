using System;
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
using APICafeSuspendu.Models;

namespace APICafeSuspendu.Controllers
{
    public class TerminalsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Terminals
        public IQueryable<Terminal> GetTerminals()
        {
            return db.Terminals;
        }

        // GET: api/Terminals/5
        [ResponseType(typeof(Terminal))]
        public async Task<IHttpActionResult> GetTerminal(int id)
        {
            Terminal terminal = await db.Terminals.FindAsync(id);
            if (terminal == null)
            {
                return NotFound();
            }

            return Ok(terminal);
        }

        // PUT: api/Terminals/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTerminal(int id, Terminal terminal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != terminal.TerminalId)
            {
                return BadRequest();
            }

            db.Entry(terminal).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TerminalExists(id))
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

        // POST: api/Terminals
        [ResponseType(typeof(Terminal))]
        public async Task<IHttpActionResult> PostTerminal(Terminal terminal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Terminals.Add(terminal);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = terminal.TerminalId }, terminal);
        }

        // DELETE: api/Terminals/5
        [ResponseType(typeof(Terminal))]
        public async Task<IHttpActionResult> DeleteTerminal(int id)
        {
            Terminal terminal = await db.Terminals.FindAsync(id);
            if (terminal == null)
            {
                return NotFound();
            }

            db.Terminals.Remove(terminal);
            await db.SaveChangesAsync();

            return Ok(terminal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TerminalExists(int id)
        {
            return db.Terminals.Count(e => e.TerminalId == id) > 0;
        }
    }
}