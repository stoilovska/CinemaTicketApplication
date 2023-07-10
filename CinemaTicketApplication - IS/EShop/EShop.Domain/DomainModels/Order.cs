﻿using EShop.Domain.Identity;
using EShop.Domain.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public EShopApplicationUser User { get; set; }

        public virtual ICollection<TicketInOrder> TicketInOrders { get; set; }
    }
}
