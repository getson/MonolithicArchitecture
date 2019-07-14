using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyApp.Core.SharedKernel.Domain;
using MyApp.Domain.Example.CountryAgg;

namespace MyApp.Domain.Example.CustomerAgg
{
    /// <summary>
    /// Aggregate root for Customer Aggregate.
    /// </summary>
    public class Customer : AggregateRoot
    {

        #region Members

        bool _isEnabled;
        #endregion

        #region Properties


        /// <summary>
        /// Get or set the Given name of this customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Get or set the surname of this customer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Get or set the full name of this customer
        /// </summary>
        public string FullName => string.Format("{0}, {1}", LastName, FirstName);

        /// <summary>
        /// Get or set the telephone 
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Get or set the company name
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Get or set the address of this customer
        /// </summary>
        public virtual Address Address { get; set; }

        /// <summary>
        /// Get or set the current credit limit for this customer
        /// </summary>
        public decimal CreditLimit { get; private set; }

        /// <summary>
        /// Get or set if this customer is enabled
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            private set
            {
                _isEnabled = value;
            }
        }


        /// <summary>
        /// Get or set associated country identifier
        /// </summary>
        public int CountryId { get; private set; }

        /// <summary>
        /// Get the current country for this customer
        /// </summary>
        public virtual Country Country { get; private set; }

        /// <summary>
        /// Get or set associated photo for this customer
        /// </summary>
        public virtual Picture Picture { get; private set; }

        #endregion

        #region Constructor

        #endregion
        #region Public Methods

        /// <summary>
        /// Disable customer
        /// </summary>
        public void Disable()
        {
            if (IsEnabled)
                _isEnabled = false;
        }

        /// <summary>
        /// Enable customer
        /// </summary>
        public void Enable()
        {
            if (!IsEnabled)
                _isEnabled = true;
        }

        /// <summary>
        /// Associate existing country to this customer
        /// </summary>
        /// <param name="country"></param>
        public void SetTheCountryForThisCustomer(Country country)
        {
            if (country == null ||country.IsTransient())
            {
                throw new ArgumentException("null country");
            }

            //fix relation
            CountryId = country.Id;

            Country = country;
        }

        /// <summary>
        /// Set the country reference for this customer
        /// </summary>
        /// <param name="countryId"></param>
        public void SetTheCountryReference(int countryId)
        {
            if (countryId != 0)
            {
                //fix relation
                CountryId = countryId;

                Country = null;
            }
        }

        /// <summary>
        /// Change the customer credit limit
        /// </summary>
        /// <param name="newCredit">the new credit limit</param>
        public void ChangeTheCurrentCredit(decimal newCredit)
        {
            if (IsEnabled)
                CreditLimit = newCredit;
        }

        /// <summary>
        /// change the picture for this customer
        /// </summary>
        /// <param name="picture">the new picture for this customer</param>
        public void ChangePicture(Picture picture)
        {
            if (picture != null &&
                !picture.IsTransient())
            {
                Picture = picture;
            }
        }

        #endregion

        #region IValidatableObject Members

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            //-->Check first name property
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                validationResults.Add(new ValidationResult("validation_CustomerFirstNameCannotBeNull",
                                                           new[] { "FirstName" }));
            }

            //-->Check last name property
            if (string.IsNullOrWhiteSpace(LastName))
            {
                validationResults.Add(new ValidationResult("validation_CustomerLastNameCannotBeBull",
                                                           new[] { "LastName" }));
            }

            //-->Check Country identifier
            if (CountryId == 0)
                validationResults.Add(new ValidationResult("validation_CustomerCountryIdCannotBeEmpty",
                                                          new[] { "CountryId" }));


            return validationResults;
        }
        #endregion
    }
}
