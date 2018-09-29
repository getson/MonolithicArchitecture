using System.Collections.Generic;
using MyApp.Domain.Example.ProductAgg;
using MyApp.Services.DTOs;
using Xunit;

namespace MyApp.Services.Tests.Mapping
{
    public class ProductAdapterTests : TestsInitialize
    {
        [Fact]
        public void ProductToProductDtoAdapter()
        {
            //Arrange
            var product = new Software("the title", "The description","AB001");
            product.ChangeUnitPrice(10);
            product.IncrementStock(10);
       

            //Act
            var productDto = TypeAdapter.Adapt<Product, ProductDto>(product);

            //Assert
            Assert.Equal(product.Id, productDto.Id);
            Assert.Equal(product.Title, productDto.Title);
            Assert.Equal(product.Description, productDto.Description);
            Assert.Equal(product.AmountInStock, productDto.AmountInStock);
            Assert.Equal(product.UnitPrice, productDto.UnitPrice);
        }

        [Fact]
        public void EnumerableProductToListProductDtoAdapter()
        {
            //Arrange
            var software = new Software("the title", "The description","AB001");
            software.ChangeUnitPrice(10);
            software.IncrementStock(10);
           

            var products = new List<Software> { software };

            //Act
            var productsDto = TypeAdapter.Adapt<IEnumerable<Product>, List<ProductDto>>(products);

            //Assert
            Assert.Equal(products[0].Id, productsDto[0].Id);
            Assert.Equal(products[0].Title, productsDto[0].Title);
            Assert.Equal(products[0].Description, productsDto[0].Description);
            Assert.Equal(products[0].AmountInStock, productsDto[0].AmountInStock);
            Assert.Equal(products[0].UnitPrice, productsDto[0].UnitPrice);
        }

        [Fact]
        public void SoftwareToSoftwareDtoAdapter()
        {
            //Arrange
            var software = new Software("the title", "The description","AB001");
            software.ChangeUnitPrice(10);
            software.IncrementStock(10);
        
            //Act
            var softwareDto = TypeAdapter.Adapt<Software, SoftwareDto>(software);

            //Assert
            Assert.Equal(software.Id, softwareDto.Id);
            Assert.Equal(software.Title, softwareDto.Title);
            Assert.Equal(software.Description, softwareDto.Description);
            Assert.Equal(software.AmountInStock, softwareDto.AmountInStock);
            Assert.Equal(software.UnitPrice, softwareDto.UnitPrice);
            Assert.Equal(software.LicenseCode, softwareDto.LicenseCode);
        }

        [Fact]
        public void EnumerableSoftwareToListSoftwareDtoAdapter()
        {
            //Arrange
            var software = new Software("the title", "The description", "AB001");

            software.ChangeUnitPrice(10);
            software.IncrementStock(10);
           

            var softwares = new List<Software> { software };
            

            //Act
            var softwaresDto = TypeAdapter.Adapt<IEnumerable<Software>, List<SoftwareDto>>(softwares);

            //Assert
            Assert.Equal(softwares[0].Id, softwaresDto[0].Id);
            Assert.Equal(softwares[0].Title, softwaresDto[0].Title);
            Assert.Equal(softwares[0].Description, softwaresDto[0].Description);
            Assert.Equal(softwares[0].AmountInStock, softwaresDto[0].AmountInStock);
            Assert.Equal(softwares[0].UnitPrice, softwaresDto[0].UnitPrice);
            Assert.Equal(softwares[0].LicenseCode, softwaresDto[0].LicenseCode);
        }

        [Fact]
        public void BookToBookDtoAdapter()
        {
            //Arrange
            var book = new Book("the title", "The description", "Krasis Press", "ABD12");

            book.ChangeUnitPrice(10);
            book.IncrementStock(10);

   

            //Act
            var bookDto = TypeAdapter.Adapt<Book, BookDto>(book);

            //Assert
            Assert.Equal(book.Id, bookDto.Id);
            Assert.Equal(book.Title, bookDto.Title);
            Assert.Equal(book.Description, bookDto.Description);
            Assert.Equal(book.AmountInStock, bookDto.AmountInStock);
            Assert.Equal(book.UnitPrice, bookDto.UnitPrice);
            Assert.Equal(book.Isbn, bookDto.Isbn);
            Assert.Equal(book.Publisher, bookDto.Publisher);
        }

        [Fact]
        public void EnumerableBookToListBookDtoAdapter()
        {
            //Arrange
            var book = new Book("the title", "The description","Krasis Press","ABD12");

            book.ChangeUnitPrice(10);
            book.IncrementStock(10);
          

            var books = new List<Book> { book };

            //Act
            var booksDto = TypeAdapter.Adapt<IEnumerable<Book>, List<BookDto>>(books);

            //Assert
            Assert.Equal(books[0].Id, booksDto[0].Id);
            Assert.Equal(books[0].Title, booksDto[0].Title);
            Assert.Equal(books[0].Description, booksDto[0].Description);
            Assert.Equal(books[0].AmountInStock, booksDto[0].AmountInStock);
            Assert.Equal(books[0].UnitPrice, booksDto[0].UnitPrice);
            Assert.Equal(books[0].Isbn, booksDto[0].Isbn);
            Assert.Equal(books[0].Publisher, booksDto[0].Publisher);
        }
    }
}
