﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMAPP.Models;

namespace SMAPP.Controllers
{
    public class SponsorsController : Controller
    {
        private ApplicationDbContext _context;

        public SponsorsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        // GET: Sponsors
        public ViewResult Index()
        {
            if (User.IsInRole(RoleName.CanManageAPP))
                return View("Index");

            return View("ReadOnlyList");
        }



        [Authorize(Roles = RoleName.CanManageAPP)]
        public ActionResult New()
        {
            var model = new Sponsor();

            return View("SponsorsForm", model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageAPP)]
        public ActionResult Save(Sponsor sponsor)
        {
            if (!ModelState.IsValid)
            {
                var model = new Sponsor();
                return View("SponsorForm",model);
            }

            if (sponsor.Id == 0)
                _context.Sponsors.Add(sponsor);
            else
            {
                var sponsorInDb = _context.Sponsors.Single(c => c.Id == sponsor.Id);

                sponsorInDb.Name = sponsor.Name;
                sponsorInDb.Url = sponsor.Url;
                sponsorInDb.Username = sponsor.Username;
                sponsorInDb.Password = sponsor.Password;
                sponsorInDb.UsernameTag = sponsor.UsernameTag;
                sponsorInDb.PasswordTag = sponsor.PasswordTag;
                sponsorInDb.SubmitTag = sponsor.SubmitTag;
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Sponsors");
        }



        [Authorize(Roles = RoleName.CanManageAPP)]
        public ActionResult Details(int id)
        {
            var sponsor = _context.Sponsors.SingleOrDefault(c => c.Id == id);

            if (sponsor == null)
                return HttpNotFound();

            return View(sponsor);
        }



        [Authorize(Roles = RoleName.CanManageAPP)]
        public ActionResult Edit(int id)
        {
            var sponsor = _context.Sponsors.SingleOrDefault(c => c.Id == id);

            if (sponsor == null)
                return HttpNotFound();

            var model = new Sponsor();

            return View("SponsorsForm", model);

        }
    }
}