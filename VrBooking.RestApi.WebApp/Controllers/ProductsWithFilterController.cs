﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VrBooking.Core.ApplicationServices;
using VrBooking.Core.Entity;

namespace VrBooking.RestApi.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsWithFilterController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;


        public ProductsWithFilterController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpPost]
        public ActionResult<List<FilterPageList<Product>>> Post([FromBody] FilterPageList<Product> pagefilter)
        {
            try
            {

                return Ok(_productService.ReadAllWithPageFilter(pagefilter));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

    }
}