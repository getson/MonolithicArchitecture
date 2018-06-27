namespace MyApp.Core.Domain.Example.CustomerAgg
{
    /// <summary>
    /// Picture of customer
    /// </summary>
    public class Picture:BaseEntity
    {
        #region Properties

        /// <summary>
        /// Get the raw of photo
        /// </summary>
        public byte[] RawPhoto{ get; set; }

        #endregion
    }
}
