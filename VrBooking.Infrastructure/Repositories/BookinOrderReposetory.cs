﻿using System;
using System.Collections.Generic;
using System.Text;
using VrBooking.Core;
using VrBooking.Core.ApplicationServices;

namespace VrBooking.Infrastructure.Repositories
{
    class BookinOrderReposetory: IRepository<BookingOrder>
    {
        private IBookingOrderService _service;

        public BookinOrderReposetory(IBookingOrderService bookingOrderService)
        {
            _service = bookingOrderService;
        }

        public BookingOrder Create(BookingOrder entity)
        {
            throw new NotImplementedException();
        }

        public BookingOrder Read(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BookingOrder> ReadAll()
        {
            throw new NotImplementedException();
        }

        public BookingOrder Update(BookingOrder entity)
        {
            throw new NotImplementedException();
        }

        public BookingOrder Delete(BookingOrder entity)
        {
            throw new NotImplementedException();
        }
    }
}