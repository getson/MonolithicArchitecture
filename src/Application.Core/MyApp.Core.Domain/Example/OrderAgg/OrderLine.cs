using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyApp.Domain.Example.ProductAgg;
using MyApp.SharedKernel.Domain;

namespace MyApp.Domain.Example.OrderAgg
{
    /// <summary>
    /// The order line representation
    /// </summary>
    public class OrderLine : BaseEntity, IValidatableObject
    {
        #region Properties

        /// <summary>
        /// Get or set the current unit price of the product in this line
        /// <remarks>
        /// The unit price cannot be less than zero
        /// </remarks>
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Get or set the amount of units in this line
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Get or set the discount associated
        /// <remarks>
        /// Discount is a value between [0-1]
        /// </remarks>
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Get the total amount of money for this line
        /// </summary>
        public decimal TotalLine
        {
            get
            {
                return (UnitPrice * Amount) * (1 - (Discount / 100M));
            }
        }

        /// <summary>
        /// Related aggregate identifier
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Get or set the product identifier
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Get or set associated product 
        /// </summary>
        public virtual Product Product { get; private set; }

        #endregion

        #region Constructor

        #endregion
        #region Public Methods

        /// <summary>
        /// Sets a product in this order line
        /// </summary>
        /// <param name="product">The related product for this order line</param>
        public void SetProduct(Product product)
        {
            if (product == null || product.IsTransient())
            {
                throw new ArgumentNullException("CannotAssociateTransientOrNullProduct");
            }

            //fix identifiers
            ProductId = product.Id;
            Product = product;
        }

        #endregion

        #region IValidatableObject Members

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (Discount < 0 || Discount > 1)
                validationResults.Add(new ValidationResult(".validation_OrderLineDiscountCannotBeLessThanZeroOrGreaterThanOne",
                                                            new[] { "Discount" }));
            if (OrderId == Guid.Empty)
                validationResults.Add(new ValidationResult(".validation_OrderLineOrderIdIsEmpty",
                                                           new[] { "OrderId" }));

            if (Amount <= 0)
                validationResults.Add(new ValidationResult(".validation_OrderLineAmountLessThanOne",
                                                           new[] { "Amount" }));

            if (UnitPrice < 0)
                validationResults.Add(new ValidationResult(".validation_OrderLineUnitPriceLessThanZero",
                                                           new[] { "UnitPrice" }));

            if (ProductId == Guid.Empty)
                validationResults.Add(new ValidationResult(".validation_OrderLineProductIdCannotBeNull",
                                                         new[] { "ProductId" }));

            return validationResults;
        }

        #endregion
    }
}
