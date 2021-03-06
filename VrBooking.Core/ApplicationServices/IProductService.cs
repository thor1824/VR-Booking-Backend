﻿using System.Collections.Generic;
using VrBooking.Core.Entity;

namespace VrBooking.Core.ApplicationServices
{
    public interface IProductService
    {
        Product Create(Product product);
        Product Delete(long id);
        Product Read(long id);
        List<Product> ReadAll();
        Product Update(Product product);
        FilterPageList<Product> ReadAllWithPageFilter(int pageIndex, int itemsPrPage, int filterId);
    }
}