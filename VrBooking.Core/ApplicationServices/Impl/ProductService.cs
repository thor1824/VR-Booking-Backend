﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VrBooking.Core.DomainServices;
using VrBooking.Core.Entity;

namespace VrBooking.Core.ApplicationServices
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repo;
        public ProductService(IRepository<Product> repo)
        {
            _repo = repo;
        }

        #region C.R.U.D.

        public Product Create(Product product)
        {
            Product createdProduct;
            try
            {
                if (string.IsNullOrEmpty(product.Name))
                {
                    throw new InvalidDataException("product must contain a name");
                }

                if (string.IsNullOrEmpty(product.Description))
                {
                    throw new InvalidDataException("the product must have a description");
                }

                createdProduct = _repo.Create(product);

                if (!IsIdValid(createdProduct))
                {
                    throw new InvalidOperationException("ID not valid");
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return createdProduct;
        }

        public Product Read(long id)
        {
            Product product;
            try
            {
                product = _repo.Read(id);
                if (product == null)
                {
                    throw new InvalidDataException("Product does not exist");
                }
                if (product.Id != id)
                {
                    throw new InvalidOperationException("Error: ProductService Read(id) retrieves wrong product");
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return product;
        }
        public List<Product> ReadAll()
        {
            try
            {
                return _repo.ReadAll().ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public FilterPageList<Product> ReadAllWithPageFilter(int pageIndex, int itemsPrPage, int filterId)
        {
            FilterPageList<Product> pageList = new FilterPageList<Product>();
            try
            {

                IEnumerable<Product> list = _repo.ReadAll();
                if (filterId != 0)
                {
                    IEnumerable<Product> temp = list.Where(p => p.Category.Id == filterId).ToList();
                    list = temp;
                }
                pageList.ItemsTotal = list.Count();
                pageList.ItemsPrPage = itemsPrPage;
                pageList.PageIndex = pageIndex;
                pageList.PageTotal = pageList.ItemsTotal / itemsPrPage;

                pageList.List = list.Skip(pageIndex * itemsPrPage).Take(itemsPrPage).ToList();

            }
            catch (Exception e)
            {
                throw e;
            }
            return pageList;
        }

        public Product Update(Product product)
        {
            Product updatedProduct;
            try
            {
                if (!ProductExist(product.Id))
                {
                    throw new InvalidDataException("Product does not exist");
                }

                updatedProduct = _repo.Update(product);

                if (updatedProduct == null)
                {
                    throw new InvalidOperationException("Updated Product was null");
                }

                if (product.Equals(Read(product.Id)))
                {
                    throw new InvalidOperationException("Product was not Updated");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return updatedProduct;
        }

        public Product Delete(long id)
        {
            Product deletedProduct;
            try
            {
                deletedProduct = _repo.Delete(Read(id));
                if (deletedProduct == null)
                {
                    throw new InvalidOperationException("Deleted Product was null");
                }

                if (ProductExist(id))
                {
                    throw new InvalidOperationException("Product was not deleted");
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return deletedProduct;
        }

        #endregion

        #region validation
        public bool IsIdValid(Product product)
        {
            if (product.Id <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ProductExist(long id)
        {
            if (_repo.Read(id) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        #endregion
    }
}
