using System;
using MyApp.SharedKernel.Domain;

namespace MyApp.Domain.Example.BankAccountAgg
{
    /// <summary>
    /// The bank transferLog representation
    /// </summary>
    public class BankAccountActivity : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Get or set the bank account identifier
        /// </summary>
        public Guid BankAccountId { get; set; }

        /// <summary>
        /// The bank transferLog date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The bank transferLog amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Get or set the activity description
        /// </summary>
        public string ActivityDescription { get; set; }

        #endregion
    }
}
