using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyApp.SharedKernel.Domain;

namespace MyApp.Domain.Example.CountryAgg
{
    /// <summary>
    /// The country entity
    /// </summary>
    public class Country : AggregateRoot
    {
        #region Properties

        /// <summary>
        /// Get or set the Country Name
        /// </summary>
        public string CountryName { get; private set; }

        /// <summary>
        /// Get or set the Country ISO Code
        /// </summary>
        public string CountryIsoCode { get; private set; }

        #endregion

        #region Constructor

        //required by EF
        public Country() { }

        public Country(string countryName, string countryIsoCode)
        {
            if (string.IsNullOrWhiteSpace(countryName))
                throw new ArgumentNullException("countryName");

            if (string.IsNullOrWhiteSpace(countryIsoCode))
                throw new ArgumentNullException("countryIsoCode");

            CountryName = countryName;
            CountryIsoCode = countryIsoCode;
        }

        #endregion
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
