using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MyApp.Core.SharedKernel.Domain;
using MyApp.Domain.Example.CustomerAgg;

namespace MyApp.Domain.Example.OrderAgg
{
    /// <summary>
    /// Order aggregate root-entity
    /// </summary>
    public class Order : AggregateRoot
    {
        #region Members

        private ICollection<OrderLine> _orderLines;

        #endregion

        #region Properties

        /// <summary>
        /// Get or set the Order Date
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Get or set order delivery date
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// True if this order is delivered
        /// </summary>
        public bool IsDelivered { get; set; }

        /// <summary>
        /// Associated customer identifier to this Order
        /// </summary>
        public int CustomerId { get; private set; }

        /// <summary>
        /// Get the  sequence number order of  this order
        /// </summary>
        public int SequenceNumberOrder { get; private set; }

        /// <summary>
        /// Get a friendly order number
        /// </summary>
        public string OrderNumber
        {
            get
            {
                return string.Format("{0}/{1}-{2}", OrderDate.Year, OrderDate.Month, SequenceNumberOrder);
            }
        }

        /// <summary>
        /// Get the related customer
        /// </summary>
        public virtual Customer Customer { get; private set; }

        /// <summary>
        /// Get or set the shipping information
        /// </summary>
        public virtual ShippingInfo ShippingInformation { get; set; }

        /// <summary>
        /// Get or set related order lines
        /// </summary>
        public virtual ICollection<OrderLine> OrderLines
        {
            get
            {
                if (_orderLines == null)
                    _orderLines = new Collection<OrderLine>();

                return _orderLines;
            }
            set
            {
                _orderLines = value;
            }
        }

        #endregion

        #region Constructor

        #endregion

        #region Public Methods

        /// <summary>
        /// Create and add a new order line
        /// </summary>
        /// <param name="productId">the product identifier</param>
        /// <param name="amount">the number of items</param>
        /// <param name="unitPrice">the unit price of each item</param>
        /// <param name="discount">applied discount</param>
        /// <returns>added new order line</returns>
        public OrderLine AddNewOrderLine(int productId, int amount, decimal unitPrice, decimal discount)
        {
            //check precondition
            if (amount <= 0 || productId == 0)
            {
                throw new ArgumentException("exception_InvalidDataForOrderLine");
            }

            //check discount values
            if (discount < 0)
                discount = 0;


            if (discount > 100)
                discount = 100;

            //create new order line
            var newOrderLine = new OrderLine
            {
                OrderId = Id,
                ProductId = productId,
                Amount = amount,
                Discount = discount,
                UnitPrice = unitPrice
            };
            //set identity
            //newOrderLine.GenerateNewIdentity();

            //add order line
            OrderLines.Add(newOrderLine);

            //return added orderline
            return newOrderLine;
        }
        /// <summary>
        /// Link a customer to this order line
        /// </summary>
        /// <param name="customer">The customer to relate</param>
        public void SetTheCustomerForThisOrder(Customer customer)
        {
            if (customer == null
                ||
                customer.IsTransient())
            {
                throw new ArgumentException("exception_CannotAssociateTransientOrNullCustomer");
            }

            Customer = customer;
            CustomerId = customer.Id;
        }

        /// <summary>
        /// Set the customer reference for this order
        /// </summary>
        /// <param name="customerId">the customer identifier</param>
        public void SetTheCustomerReferenceForThisOrder(int customerId)
        {
            if (customerId != 0)
            {
                Customer = null;
                CustomerId = customerId;
            }
        }

        /// <summary>
        /// Set this order as delivered. This method
        /// changes the delivered date and sets a new state for this order
        /// </summary>
        public void SetOrderAsDelivered()
        {
            DeliveryDate = DateTime.UtcNow;
            IsDelivered = true;
        }
        /// <summary>
        /// Get the total of the order
        /// </summary>
        /// <returns>The total of the order</returns>
        public decimal GetOrderTotal()
        {
            var total = 0M;

            if (OrderLines != null //use OrderLines for lazy loading
                &&
                OrderLines.Any())
            {
                total = OrderLines.Aggregate(total, (t, l) => t += l.TotalLine);
            }

            return total;
        }

        /// <summary>
        /// Check if the total order is less than the Max Credit
        /// </summary>
        /// <returns>True if total order is less thatn the max customer credit, else false</returns>
        public bool IsCreditValidForOrder()
        {
            //Check if amout of order is valid for the customer credit

            var customerCredit = Customer.CreditLimit;

            if (GetOrderTotal() > customerCredit)
                return false;

            //TODO: This is a parametrizable value, you can 
            //set this value in configuration or other system

            var maxTotalOrder = 1000000M;

            //Check if total order exceeds  limits 
            if (GetOrderTotal() > maxTotalOrder)
                return false;

            return true;
        }

        #endregion

        #region IValidatableObject Members
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (CustomerId == 0)
                validationResults.Add(new ValidationResult("validation_OrderCustomerIdCannotBenull", new[] { "CustomerId" }));

            return validationResults;
        }

        #endregion
    }
}
