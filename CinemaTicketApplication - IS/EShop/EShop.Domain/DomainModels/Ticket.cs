﻿using EShop.Domain.Relations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EShop.Domain.DomainModels
{
    public class Ticket : BaseEntity
    {
        [Required]
        public string TicketName { get; set; }
        [Required]
        public string TicketImage { get; set; }
        [Required]
        public string TicketDescription { get; set; }
        [Required]
        public double TicketPrice { get; set; }
        [Required]
        public double TicketRating { get; set; }


        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCarts { get; set; }
        public virtual ICollection<TicketInOrder> TicketInOrders { get; set; }
    }
}
