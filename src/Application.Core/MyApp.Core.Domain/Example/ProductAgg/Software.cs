﻿using System;

namespace MyApp.Domain.Example.ProductAgg
{
    /// <summary>
    /// The software product
    /// </summary>
    public class Software:Product
    {
        #region Properties

        /// <summary>
        /// Get or set the license code
        /// </summary>
        public string LicenseCode { get; private set; }

        #endregion

        #region Constructor

        //required by ef
        private Software() { }

        public Software(string title, string description,string licenseCode)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException("title");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("description");

            if (string.IsNullOrWhiteSpace(licenseCode))
                throw new ArgumentNullException("licenseCode");

            Title = title;
            Description = description;
            LicenseCode = licenseCode;
        }

        #endregion
    }
}