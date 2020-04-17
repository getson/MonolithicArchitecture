using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;

namespace BinaryOrigin.SeedWork.Persistence.Ef.MySql
{
    /// <summary>
    /// Parse errors that comes from databases
    /// Actually it parses only unique violation
    /// </summary>
    public class MySqlDbExeptionParserProvider : DbExceptionParser, IDbExceptionParserProvider
    {
        public const int MySqlViolationOfUniqueIndex = 23505;
        private readonly DbErrorMessagesConfiguration _errorMessages;

        public MySqlDbExeptionParserProvider(DbErrorMessagesConfiguration errorMessages)
        {
            _errorMessages = errorMessages ?? throw new ArgumentNullException(nameof(errorMessages));
        }

        public override string Parse(Exception e)
        {
            var dbUpdateEx = e as DbUpdateException;

            if (dbUpdateEx?.InnerException is NpgsqlException mySqlEx)
            {
                //This is a DbUpdateException on a SQL database

                if (mySqlEx.ErrorCode == MySqlViolationOfUniqueIndex)
                {
                    //We have an error we can process
                    var valError = ParseUniquenessError(mySqlEx.Message, _errorMessages.UniqueErrorTemplate, _errorMessages.CombinationUniqueErrorTemplate);
                    if (valError != null)
                    {
                        return valError;
                    }
                }
            }
            //TODO: add other types of exception we can handle
            //otherwise exception wasn't handled, so return null
            return null;
        }
    }
}