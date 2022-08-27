using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShackUp.Data.Factory;
using ShackUp.Models.Tables;
using ShackUp.UI.Models;
using ShackUp.UI.Utilities;
using System;
using System.IO;
using System.Web.Mvc;

namespace ShackUp.UI.Controllers
{
    public class ListingsController : Controller
    {  
        // GET: Listings
        public ActionResult Details(int id)
        {
            if(Request.IsAuthenticated)
            {
                ViewBag.UserId = AuthorizeUtilities.GetUserId(this);
            }
            var repo = ListingRepositoryFactory.GetRepository();
            var model = repo.GetDetails(id);
            return View(model);
        }

        public ActionResult Index()
        {
            var repo = StatesRepositoryFactory.GetRepository();

            return View(repo.GetAll());
        }

        [Authorize]
        public ActionResult Add()
        {
            var model = new ListingAddViewModel();
            var statesRepo = StatesRepositoryFactory.GetRepository();
            var bathroomRepo = BathroomTypesRepositoryFactory.GetRepository();

            model.States = new SelectList(statesRepo.GetAll(), "StateId", "StateId");
            model.BathroomTypes = new SelectList(bathroomRepo.GetAll(), "BathroomTypeId", "BathroomTypeName");
            model.Listing = new Listing();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Add(ListingAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var repo = ListingRepositoryFactory.GetRepository();

                try
                {
                    model.Listing.UserId = AuthorizeUtilities.GetUserId(this);

                    if(model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                    {
                        var savePath = Server.MapPath("~/Images");

                        string fileName = Path.GetFileNameWithoutExtension(model.ImageUpload.FileName);
                        string extension = Path.GetExtension(model.ImageUpload.FileName);

                        var filePath = Path.Combine(savePath, fileName + extension);

                        int counter = 1;

                        while (System.IO.File.Exists(filePath))
                        {
                            filePath = Path.Combine(savePath, fileName + counter.ToString() + extension);
                            counter++;
                        }

                        model.ImageUpload.SaveAs(filePath);
                        model.Listing.ImageFileName = Path.GetFileName(filePath);
                    }
                    repo.Insert(model.Listing);

                    return RedirectToAction("Edit", new { id = model.Listing.ListingId });
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            } 
            else
            {
                var statesRepo = StatesRepositoryFactory.GetRepository();
                var bathroomRepo = BathroomTypesRepositoryFactory.GetRepository();

                model.States = new SelectList(statesRepo.GetAll(), "StateId", "StateId");
                model.BathroomTypes = new SelectList(bathroomRepo.GetAll(), "BathroomTypeId", "BathroomTypeName");

                return View(model);
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var model = new ListingEditViewModel();
            var statesRepo = StatesRepositoryFactory.GetRepository();
            var bathroomRepo = BathroomTypesRepositoryFactory.GetRepository();
            var listingsRepo = ListingRepositoryFactory.GetRepository();

            model.States = new SelectList(statesRepo.GetAll(), "StateId", "StateId");
            model.BathroomTypes = new SelectList(bathroomRepo.GetAll(), "BathroomTypeId", "BathroomTypeName");
            model.Listing = listingsRepo.GetById(id);

            if(model.Listing.UserId != AuthorizeUtilities.GetUserId(this))
            {
                throw new Exception("Attempt to edit a listing you do not own! Naughty!");
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(ListingEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var repo = ListingRepositoryFactory.GetRepository();

                try
                {
                    model.Listing.UserId = AuthorizeUtilities.GetUserId(this);
                    var oldListing = repo.GetById(model.Listing.ListingId);

                    if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                    {
                        var savePath = Server.MapPath("~/Images");

                        string fileName = Path.GetFileNameWithoutExtension(model.ImageUpload.FileName);
                        string extension = Path.GetExtension(model.ImageUpload.FileName);

                        var filePath = Path.Combine(savePath, fileName + extension);

                        int counter = 1;

                        while (System.IO.File.Exists(filePath))
                        {
                            filePath = Path.Combine(savePath, fileName + counter.ToString() + extension);
                            counter++;
                        }

                        model.ImageUpload.SaveAs(filePath);
                        model.Listing.ImageFileName = Path.GetFileName(filePath);

                        var oldPath = Path.Combine(savePath, oldListing.ImageFileName);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                    else
                    {     
                        model.Listing.ImageFileName = oldListing.ImageFileName;
                    }

                    repo.Update(model.Listing);

                    return RedirectToAction("Edit", new { id = model.Listing.ListingId });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                var statesRepo = StatesRepositoryFactory.GetRepository();
                var bathroomRepo = BathroomTypesRepositoryFactory.GetRepository();

                model.States = new SelectList(statesRepo.GetAll(), "StateId", "StateId");
                model.BathroomTypes = new SelectList(bathroomRepo.GetAll(), "BathroomTypeId", "BathroomTypeName");

                return View(model);
            }
        }
    }
}