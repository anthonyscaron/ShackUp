using ShackUp.Data.Factory;
using ShackUp.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShackUp.UI.Controllers
{
    public class ListingsAPIController : ApiController
    {
        [Route("api/listings/search")]
        [AcceptVerbs("GET")]
        public IHttpActionResult Search(decimal? minRate, decimal? maxRate, string city, string stateId)
        {
            var repo = ListingRepositoryFactory.GetRepository();

            try
            {
                var parameters = new ListingSearchParameters()
                {
                    MinRate = minRate,
                    MaxRate = maxRate,
                    City = city,
                    StateId = stateId
                };

                var result = repo.Search(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/contact/check/{userId}/{listingId}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult CheckContact(string userId, int listingId)
        {
            var repo = AccountRepositoryFactory.GetRepository();

            try
            {
                var result = repo.IsContact(userId, listingId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/favorite/check/{userId}/{listingId}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult CheckFavorite(string userId, int listingId)
        {
            var repo = AccountRepositoryFactory.GetRepository();

            try
            {
                var result = repo.IsFavorite(userId, listingId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/contact/add/{userId}/{listingId}")]
        [AcceptVerbs("POST")]
        public IHttpActionResult AddContact(string userId, int listingId)
        {
            var repo = AccountRepositoryFactory.GetRepository();

            try
            {
                repo.AddContact(userId, listingId);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/contact/remove/{userId}/{listingId}")]
        [AcceptVerbs("DELETE")]
        public IHttpActionResult RemoveContact(string userId, int listingId)
        {
            var repo = AccountRepositoryFactory.GetRepository();

            try
            {
                repo.RemoveContact(userId, listingId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/favorite/add/{userId}/{listingId}")]
        [AcceptVerbs("POST")]
        public IHttpActionResult AddFavortite(string userId, int listingId)
        {
            var repo = AccountRepositoryFactory.GetRepository();

            try
            {
                repo.AddFavorite(userId, listingId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/favorite/remove/{userId}/{listingId}")]
        [AcceptVerbs("DELETE")]
        public IHttpActionResult RemoveFavorite(string userId, int listingId)
        {
            var repo = AccountRepositoryFactory.GetRepository();

            try
            {
                repo.RemoveFavorite(userId, listingId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
