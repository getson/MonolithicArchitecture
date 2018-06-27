using MyApp.Core.Domain.Example.BankAccountAgg;

namespace MyApp.Core.Domain.Services.Banking
{
    /// <summary>
    /// Bank transfer domain service base contract
    /// </summary>
    public interface IBankTransferService
    {
        /// <summary>
        /// Perform a new transferLog into two bank accounts
        /// </summary>
        /// <param name="amount">The bank transferLog amount</param>
        /// <param name="originAccount">The originAccount bank account</param>
        /// <param name="destinationAccount">The destinationAccount bank account</param>
        void PerformTransfer(decimal amount, BankAccount originAccount, BankAccount destinationAccount);
    }
}
