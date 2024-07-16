using System.Linq;
using System.Net;
using System.Web.Http;
using WebApi.Models;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace WebApi.Controllers
{
    public class ProductController : ApiController
    {
        private ProductContext db = new ProductContext();

        // Get all products
        public IQueryable<Product> GetProducts()
        {
            return db.Products;
        }

        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ID)
            {
                return BadRequest();
            }

            var existingProduct = db.Products.AsNoTracking().FirstOrDefault(p => p.ID == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges(); 
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        public IHttpActionResult PostProduct(Product product)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product); 
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.ID }, product);
        }

      
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product); 
            db.SaveChanges(); 
            return Ok(product); 
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ID == id) > 0;
        }
    }
}

