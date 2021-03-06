using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using VrBooking.Core.ApplicationServices;
using VrBooking.Core.DomainServices;
using VrBooking.Core.Entity;

namespace TestServices.Services
{
    [TestClass]
    public class TestProductService
    {
        private Mock<IRepository<Product>> _mockRepo;
        private IProductService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<Product>>();
            _service = new ProductService(_mockRepo.Object);
        }

        #region Test for productService.create

        [TestMethod]
        public void TestProductServiceCreate()
        {
            Product prodBeforeCreate = new Product
            {
                Description = "VR Headset",
                Name = "Oculus Quest"
            };

            Product prodAfterCreate = new Product
            {
                Description = "VR Headset",
                Name = "Oculus Quest",
                Id = 1
            };

            _mockRepo.Setup(repo => repo.Create(prodBeforeCreate)).Returns(prodAfterCreate);

            Assert.IsTrue(_service.Create(prodBeforeCreate) != null);
            _mockRepo.Verify(x => x.Create(It.IsAny<Product>()), Times.Once);
        }

        [DataRow(null, "IT IS GOOD")]
        [DataRow("", "IT IS GOOD")]
        [TestMethod]
        public void TestProductServiceCreateInvalidDataExceptionName(string name, string description)
        {
            Product productBeforeCreate = new Product
            {
                Description = description,
                Name = name
            };

            Product productAfterCreate = new Product
            {
                Id = 1,
                Description = description,
                Name = name
            };

            _mockRepo.Setup(repo => repo.Create(productAfterCreate)).Returns(productAfterCreate);

            Assert.ThrowsException<InvalidDataException>(() => _service.Create(productBeforeCreate));
            _mockRepo.Verify(x => x.Create(It.IsAny<Product>()), Times.Never);
        }

        [DataRow("Oculus Rift", "YIIBII", null)]
        [DataRow("Oculus Rift", "YIIBII", -2)]
        [TestMethod]
        public void TestProductServiceCreateInvalidOperationExceptionAssignedInvalidID(string name, string description, long id)
        {
            Product productBeforeCreate = new Product
            {
                Description = description,
                Name = name
            };

            Product productAfterCreate = new Product()
            {
                Description = description,
                Name = name,
                Id = id
            };

            _mockRepo.Setup(repo => repo.Create(productBeforeCreate)).Returns(productAfterCreate);

            Assert.ThrowsException<InvalidOperationException>(() => _service.Create(productBeforeCreate));
            _mockRepo.Verify(x => x.Create(It.IsAny<Product>()), Times.Once);
        }

        [DataRow("Oculus GO", null, 2)]
        [DataRow("Oculus GO", "", 1)]
        [TestMethod]
        public void TestProductServiceCreateInvalidDataExceptionDescription(string name, string description, long id)
        {
            Product productBeforeCreate = new Product()
            {
                Description = description,
                Name = name,
            };

            Product productAfterCreate = new Product()
            {
                Description = description,
                Name = name,
                Id = id
            };

            _mockRepo.Setup(repo => repo.Create(productBeforeCreate)).Returns(productBeforeCreate);
            _mockRepo.Setup(repo => repo.Create(productAfterCreate)).Returns(productAfterCreate);

            Assert.ThrowsException<InvalidDataException>(() => _service.Create(productBeforeCreate));
            Assert.ThrowsException<InvalidDataException>(() => _service.Create(productAfterCreate));
            _mockRepo.Verify(x => x.Create(It.IsAny<Product>()), Times.Never);
        }

        #endregion

        #region Tests for ProductService.read

        [TestMethod]
        public void TestProductServiceRead()
        {
            long id = 1;

            Product product = new Product()
            {
                Description = "JIPPI KAYYAY YA YOOO",
                Name = "HTC VIVE",
                Id = 1
            };

            Product wrongProduct = new Product()
            {
                Description = "Wubba lubba dub dub",
                Name = "HTC VIVE COSMOS",
                Id = 2
            };
            Product productNull = null;

            //Test if ProductService.Read(int) works as entended
            _mockRepo.Setup(repo => repo.Read(id)).Returns(product);

            Assert.IsTrue(_service.Read(id).Id == id);

            _mockRepo.Verify(x => x.Read(id), Times.Once);

            //Test if ProductService.Read(int) Throws exception if returning wrong entity
            _mockRepo.Setup(repo => repo.Read(id)).Returns(wrongProduct);

            Assert.ThrowsException<InvalidOperationException>(() => _service.Read(id));

            //Test if ProductService.Read(int) Throws exception if entity does not exist
            _mockRepo.Setup(repo => repo.Read(id)).Returns(productNull);

            Assert.ThrowsException<InvalidDataException>(() => _service.Read(id));
        }

        #endregion

        #region Tests for ProductService.readall

        [TestMethod]
        public void TestUserServiceReadAll()
        {
            IEnumerable<Product> products = new List<Product>()
            {
                new Product()
                {
                    Description = "Good Shit",
                    Name = "valve Index",
                    Id = 2
                }
            };
            _mockRepo.Setup(repo => repo.ReadAll()).Returns(products);

            _service.ReadAll();
            _mockRepo.Verify(x => x.ReadAll(), Times.Once);
        }

        #endregion

        #region Tests for ProductService.Update

        [TestMethod]
        public void TestProductServiceUpdate()
        {
            Product product = new Product()
            {
                Id = 1,
                Name = "Oculus GO",
                Description = "Come on facebook"
            };

            Product updatedProduct = new Product()
            {
                Id = 1,
                Name = "Oculus GO 2",
                Description = "Come on facebook for real"
            };

            //Test if ProductService.Update(Product) works as intended
            _mockRepo.Setup(x => x.Update(product)).Returns(updatedProduct);
            _mockRepo.Setup(x => x.Read(product.Id)).Returns(new Queue<Product>(new[] { product, updatedProduct }).Dequeue);

            Assert.IsTrue(_service.Update(product).Equals(updatedProduct));

            _mockRepo.Verify(x => x.Update(product), Times.Once);
            _mockRepo.Verify(x => x.Read(product.Id), Times.Exactly(2));
        }

        [TestMethod]
        public void TestProductServiceUpdateInvalidDataExceptionID()
        {
            Product product = new Product()
            {
                Id = 1,
                Name = "Google cardboard",
                Description = "ITS ShiT"
            };

            Product productNull = null;

            //Test if ProductService.Update(Product) throws exception if the product to be updated does not exist
            _mockRepo.Setup(x => x.Update(product)).Returns(product);
            _mockRepo.Setup(x => x.Read(product.Id)).Returns(productNull);

            Assert.ThrowsException<InvalidDataException>(() => _service.Update(product));
            _mockRepo.Verify(x => x.Update(It.IsAny<Product>()), Times.Never);
        }

        [TestMethod]
        public void TestProductServiceUpdateInvalidOperationExceptionReturnsNull()
        {
            Product product = new Product()
            {
                Id = 1,
                Name = "VR Headset",
                Description = "ITS SHIT"
            };

            Product updatedProduct = new Product()
            {
                Id = 1,
                Name = "VR Headset",
                Description = "Still shit"
            };

            Product productNull = null;

            //Test if ProductService.Update(Product) throws exception if not works as entended
            _mockRepo.Setup(x => x.Update(product)).Returns(productNull);
            _mockRepo.Setup(x => x.Read(product.Id)).Returns(new Queue<Product>(new[] { product, updatedProduct }).Dequeue);

            Assert.ThrowsException<InvalidOperationException>(() => _service.Update(product));
            _mockRepo.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
        }

        [TestMethod]
        public void TestProductServiceUpdateInvalidOperationExceptionDoesNotUpdate()
        {
            Product product = new Product()
            {
                Id = 1,
                Name = "Beat Saber",
                Description = "Facebook buys beat saber"
            };

            Product updatedProduct = new Product()
            {
                Id = 1,
                Name = "Fuck facebook",
                Description = "They gon ruin it"
            };


            //Test if UserService.Update(User) throws exception if not works as entended
            _mockRepo.Setup(x => x.Update(product)).Returns(product);
            _mockRepo.Setup(x => x.Read(product.Id)).Returns(new Queue<Product>(new[] { product, product }).Dequeue);

            Assert.ThrowsException<InvalidOperationException>(() => _service.Update(product));
            _mockRepo.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
        }
        #endregion

        #region Tests for ProductService.Delete

        [TestMethod]
        public void TestProductServiceDelete()
        {
            Product product = new Product()
            {
                Description = "ijwrbijwbrg",
                Name = "fh",
                Id = 2
            };

            // test if ProductService.Delete(id) works as entended
            _mockRepo.Setup(x => x.Delete(product)).Returns(product);
            _mockRepo.Setup(x => x.Read(product.Id)).Returns(new Queue<Product>(new[] { product, null }).Dequeue);

            Assert.IsTrue(_service.Delete(product.Id).Equals(product));

            _mockRepo.Verify(x => x.Delete(product), Times.Once);
            _mockRepo.Verify(x => x.Read(product.Id), Times.Exactly(2));
        }

        #endregion
    }
}
