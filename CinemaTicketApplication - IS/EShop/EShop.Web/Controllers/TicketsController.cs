﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using EShop.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EShop.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _productService;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(ILogger<TicketsController> logger, ITicketService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        // GET: Products
        public IActionResult Index()
        {
            _logger.LogInformation("User Request -> Get All tickets!");
            return View(this._productService.GetAllTickets());
        }

        // GET: Products/Details/5
        public IActionResult Details(Guid? id)
        {
            _logger.LogInformation("User Request -> Get Details For Ticket");
            if (id == null)
            {
                return NotFound();
            }

            var product = this._productService.GetDetailsForTicket(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            _logger.LogInformation("User Request -> Get create form for Ticket!");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,TicketName,TicketImage,TicketDescription,TicketPrice,TicketRating")] Ticket ticket)
        {
            _logger.LogInformation("User Request -> Inser Product in DataBase!");
            if (ModelState.IsValid)
            {
                ticket.Id = Guid.NewGuid();
                this._productService.CreateNewTicket(ticket);
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(Guid? id)
        {
            _logger.LogInformation("User Request -> Get edit form for Ticket!");
            if (id == null)
            {
                return NotFound();
            }

            var product = this._productService.GetDetailsForTicket(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,TicketName,TicketImage,TicketDescription,TicketPrice,TicketRating")] Ticket product)
        {
            _logger.LogInformation("User Request -> Update Ticket in DataBase!");

            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._productService.UpdeteExistingTicket(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(Guid? id)
        {
            _logger.LogInformation("User Request -> Get delete form for Ticket!");

            if (id == null)
            {
                return NotFound();
            }

            var product = this._productService.GetDetailsForTicket(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _logger.LogInformation("User Request -> Delete Ticket in DataBase!");

            this._productService.DeleteTicket(id);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult AddTicketToCard(Guid id)
        {
            var result = this._productService.GetShoppingCartInfo(id);

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTicketToCard(AddToShoppingCardDto model)
        {

            _logger.LogInformation("User Request -> Add Ticket in ShoppingCart and save changes in database!");


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._productService.AddToShoppingCart(model, userId);

            if(result)
            {
                return RedirectToAction("Index", "Tickets");
            }
            return View(model);
        }
        private bool TicketExists(Guid id)
        {
            return this._productService.GetDetailsForTicket(id) != null;
        }
    }
}
