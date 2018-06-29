﻿using System.Collections.Generic;
using MyApp.Core.Domain.Common;

namespace MyApp.Core.Domain.Example.CustomerAgg
{
    /// <summary>
    /// Address  information for existing customer
    /// For this Domain-Model, the Address is a Value-Object
    /// </summary>
    public class Address : ValueObject<Address>
    {
        /// For this Domain-Model, the Address is a Value-Object
        /// 'sets' are private as Value-Objects must be immutable, 
        /// so the only way to set properties is using the constructor 

        #region Properties

        /// <summary>
        /// Get or set the city of this address 
        /// </summary>
        public string City { get; private set; }

        /// <summary>
        /// Get or set the zip code
        /// </summary>
        public string ZipCode { get; private set; }

        /// <summary>
        /// Get or set address line 1
        /// </summary>
        public string AddressLine1 { get; private set; }

        /// <summary>
        /// Get or set address line 2
        /// </summary>
        public string AddressLine2 { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of address specifying its values
        /// </summary>
        /// <param name="city"></param>
        /// <param name="zipCode"></param>
        /// <param name="addressLine1"></param>
        /// <param name="addressLine2"></param>
        public Address(string city, string zipCode, string addressLine1, string addressLine2)
        {
            City = city;
            ZipCode = zipCode;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
        }

        public Address() { }  //required for EF

        #endregion

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new System.NotImplementedException();
        }
    }
}
