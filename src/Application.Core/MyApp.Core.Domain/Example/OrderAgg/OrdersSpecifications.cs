using System;
using System.Text.RegularExpressions;
using MyApp.Domain.Example.CustomerAgg;
using MyApp.SharedKernel.Specification;

namespace MyApp.Domain.Example.OrderAgg
{
    /// <summary>
    /// The Orders specifications
    /// </summary>
    public static class OrdersSpecifications
    {
        /// <summary>
        /// Orders by Customer specification
        /// </summary>
        /// <param name="customer">The customer</param>
        /// <returns>Related specification for this criterion</returns>
        public static ISpecification<Order> OrdersByCustomer(Customer customer)
        {
            if (customer == null || customer.IsTransient())
            {
                throw new ArgumentNullException("customer");
            }

            return new DirectSpecification<Order>(o => o.CustomerId == customer.Id);
        }
        /// <summary>
        /// Order by order number
        /// </summary>
        /// <param name="orderNumber">Find orders using the order number</param>
        /// <returns>Related specification for this criterion</returns>
        public static ISpecification<Order> OrdersByNumber(string orderNumber)
        {


            const string orderNumberPattern = @"\d{4}/\d{1,2}-\d+";

            if (!Regex.IsMatch(orderNumber, orderNumberPattern))
                throw new InvalidOperationException("invalid order number pattern");

            var sequenceNumber = int.Parse(Regex.Split(orderNumber, "-")[1]);
            return new DirectSpecification<Order>(o => o.SequenceNumberOrder == sequenceNumber);
        }

        /// <summary>
        /// The orders in a date range
        /// </summary>
        /// <param name="startDate">The start date </param>
        /// <param name="endDate">The end date</param>
        /// <returns>Related specification for this criteria</returns>
        public static ISpecification<Order> OrderFromDateRange(DateTime? startDate, DateTime? endDate)
        {
            Specification<Order> spec = new TrueSpecification<Order>();

            if (startDate.HasValue)
                spec &= new DirectSpecification<Order>(o => o.OrderDate > ((DateTime) startDate));

            if (endDate.HasValue)
                spec &= new DirectSpecification<Order>(o => o.OrderDate < ((DateTime) endDate));

            return spec;
        }

    }
}
