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
using FMS;

namespace FMS.API
{
    public class MenusAPIController : ApiController
    {
        private FMSExpEntities db = new FMSExpEntities();

        // GET: api/tblMenus
        public IQueryable<tblMenu> GetMenus()
        {
            return db.tblMenus;
        }

        // GET: api/tblMenus/5
        [ResponseType(typeof(tblMenu))]
        public async Task<IHttpActionResult> GettblMenu(int id)
        {
            tblMenu tblMenu = await db.tblMenus.FindAsync(id);
            if (tblMenu == null)
            {
                return NotFound();
            }

            return Ok(tblMenu);
        }

        // PUT: api/tblMenus/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttblMenu(int id, tblMenu tblMenu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tblMenu.ID)
            {
                return BadRequest();
            }

            db.Entry(tblMenu).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblMenuExists(id))
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

        // POST: api/tblMenus
        [ResponseType(typeof(tblMenu))]
        public async Task<IHttpActionResult> PosttblMenu(tblMenu tblMenu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tblMenus.Add(tblMenu);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tblMenu.ID }, tblMenu);
        }

        // DELETE: api/tblMenus/5
        [ResponseType(typeof(tblMenu))]
        public async Task<IHttpActionResult> DeletetblMenu(int id)
        {
            tblMenu tblMenu = await db.tblMenus.FindAsync(id);
            if (tblMenu == null)
            {
                return NotFound();
            }

            db.tblMenus.Remove(tblMenu);
            await db.SaveChangesAsync();

            return Ok(tblMenu);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tblMenuExists(int id)
        {
            return db.tblMenus.Count(e => e.ID == id) > 0;
        }
    }
}