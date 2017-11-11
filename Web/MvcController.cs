using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace Web
{
    /// <summary>
    /// Example of a controller used in a RESTful API.
    /// This class simulates an implimentation of Entity Framwork. 
    /// It also shows different techniques used to pass data from
    /// the url to the method.
    /// </summary>
    public class MvcController : ApiController
    {
        AzureSqlEntities db = new AzureSqlEntities();

        bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Return all User records.
        /// </summary>
        /// <returns></returns>
        // GET: api/User
        public IQueryable<MvcModel> GetUsers()
        {
            return db.Users;
        }

        /// <summary>
        /// Return User records whose last name matches a string.
        /// </summary>
        /// <returns></returns>
        // GET: api/UserByLastName/{string:lastName}/{int:count?}
        public IQueryable<MvcModel> GetUsersByLastName(string lastName, int count)
        {
            return db.Users
                .Where(x => x.LastName.Contains(lastName))
                .Take(count);
        }

        /// <summary>
        /// Return the User information associated with the ID provided.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/User/5
        [ResponseType(typeof(MvcModel))]
        public IHttpActionResult GetUser(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Update the User record associated with the ID provided.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, MvcModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        /// <summary>
        /// Create a new User record.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        // POST: api/User
        [ResponseType(typeof(MvcModel))]
        public IHttpActionResult PostUser(MvcModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        /// <summary>
        /// Delete the User record associated with the ID provided.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/User/5
        [ResponseType(typeof(MvcModel))]
        public IHttpActionResult DeleteUser(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();
            return Ok(user);
        }
    }
}