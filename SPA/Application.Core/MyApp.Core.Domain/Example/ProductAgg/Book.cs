using System;

namespace MyApp.Core.Domain.Example.ProductAgg
{
    /// <summary>
    /// The book product
    /// </summary>
    public class Book: Product
    {
        #region Properties

        /// <summary>
        /// Get or set the publisher of this book
        /// </summary>
        public string Publisher { get; private set; }

        /// <summary>
        /// Get or set related ISBN
        /// </summary>
        public string Isbn { get; private set; }

        #endregion

        #region Constructor

        //required by ef
        private Book() { }

        public Book(string title, string description,string publisher,string isbn)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException("title");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("description");

            if (string.IsNullOrWhiteSpace(publisher))
                throw new ArgumentNullException("publisher");

            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentNullException("isbn");

            Title = title;
            Description = description;
            Publisher = publisher;
            Isbn = isbn;
        }

        #endregion
    }
}
