﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VrBooking.Core.ApplicationServices;
using VrBooking.Core.Entity;

namespace VrBooking.RestApi.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;


        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // GET: api/Product
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            try
            {
                return Ok(_productService.ReadAll());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            try
            {
                return Ok(_productService.Read(id));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // POST: api/Product
        [HttpPost]
        public ActionResult Post([FromBody] Product value)
        {
            try
            {
                if (value.Category != null)
                {
                    value.Category =_categoryService.Read(value.Category.Id);
                }
                Product product = _productService.Create(value);
                return Created("" + product.Id, product);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // PUT: api/Product/5
        [HttpPut]
        public ActionResult Put([FromBody] Product value)
        {
            try
            {
                if (value.Category != null)
                {
                    value.Category =_categoryService.Read(value.Category.Id);
                }
                _productService.Update(value);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _productService.Delete(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}