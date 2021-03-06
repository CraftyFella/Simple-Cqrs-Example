﻿using System;
using System.Web.Mvc;
using AgileWorkshop.Bus;

using Inventory.Commands;

namespace Inventory.Gui.Controllers
{
	using AgileWorkshop.Cqrs.Reporting;

	[HandleError]
    public class HomeController : Controller
    {
        private ICommandBus _bus;
		private ReadModelFacade _readmodel;

        public HomeController(ICommandBus bus, IReportingRepository reportingRepository)
        {
            _bus = bus;
        	_readmodel = new ReadModelFacade(reportingRepository);
        }

        public ActionResult Index()
        {
            ViewData.Model = _readmodel.GetInventoryItems();

            return View();
        }

        public ActionResult Details(Guid id)
        {
            ViewData.Model = _readmodel.GetInventoryItemDetails(id);
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string name)
        {
			TempData["message"] = string.Format("New Inventory Item {0} added.", name);
            _bus.Send(new CreateInventoryItem(Guid.NewGuid(), name));
            return RedirectToAction("Index");
        }

        public ActionResult ChangeName(Guid id)
        {
            ViewData.Model = _readmodel.GetInventoryItemDetails(id);
            return View();
        }

        [HttpPost]
        public ActionResult ChangeName(Guid id, string name, int version)
        {
			TempData["message"] = string.Format("Inventory Item {0} renamed.", name);

            var command = new RenameInventoryItem(id, name, version);
            _bus.Send(command);
            return RedirectToAction("Index");
        }

        public ActionResult Deactivate(Guid id, int version)
        {
			TempData["message"] = string.Format("Inventory Item {0} deactivated.", _readmodel.GetInventoryItemDetails(id).Name);
            _bus.Send(new DeactivateInventoryItem(id, version));
            return RedirectToAction("Index");
        }

        public ActionResult CheckIn(Guid id)
        {
            ViewData.Model = _readmodel.GetInventoryItemDetails(id);
            return View();
        }

        [HttpPost]
        public ActionResult CheckIn(Guid id, int number, int version)
        {
			TempData["message"] = string.Format("Added {1} items to Inventory Item {0}.", _readmodel.GetInventoryItemDetails(id).Name, number);
            _bus.Send(new CheckInItemsToInventory(id, number, version));
            return RedirectToAction("Index");
        }

        public ActionResult Remove(Guid id)
        {
            ViewData.Model = _readmodel.GetInventoryItemDetails(id);
            return View();
        }

        [HttpPost]
        public ActionResult Remove(Guid id, int number, int version)
        {
			TempData["message"] = string.Format("Removed {1} items from Inventory Item {0}.", _readmodel.GetInventoryItemDetails(id).Name, number);
            _bus.Send(new RemoveItemsFromInventory(id, number, version));
            return RedirectToAction("Index");
        }
    }
}
