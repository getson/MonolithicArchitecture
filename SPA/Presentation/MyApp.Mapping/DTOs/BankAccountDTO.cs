namespace MyApp.Mapping.DTOs
{
    /// <summary>
    /// This is the data transfer object for
    /// Bank Account entitiy.The name
    /// of properties for this type
    /// is based on conventions of many mappers
    /// to simplificate the mapping process.
    /// </summary>
    public class BankAccountDto
    {
        /// <summary>
        /// The bank account identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Bank account number
        /// </summary>
        public string BankAccountNumber { get; set; }

        /// <summary>
        /// The bank balance
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// The locked state of this bank account
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Get or set the customer id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// The owner first name
        /// </summary>
        public string CustomerFirstName { get; set; }

        /// <summary>
        /// the owner last name
        /// </summary>
        public string CustomerLastName { get; set; }

      
    }
}
