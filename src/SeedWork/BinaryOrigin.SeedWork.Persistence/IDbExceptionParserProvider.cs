using System;

namespace BinaryOrigin.SeedWork.Persistence.Ef
{
    public interface IDbExceptionParserProvider
    {
        /// <summary>
        ///  Parse exception raised by DB provider
        /// </summary>
        /// <param name="e">exception object</param>
        /// <returns>Error message extracted from the exception</returns>
        string Parse(Exception e);
    }
}