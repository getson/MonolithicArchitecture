using BinaryOrigin.SeedWork.Persistence.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;

namespace BinaryOrigin.SeedWork.Persistence.SqlServer
{
    public class SqlServerDbExceptionParserProvider : DbExceptionParser, IDbExceptionParserProvider
    {
        public const int SqlServerViolationOfUniqueIndex = 2601;
        public const int SqlServerViolationOfUniqueConstraint = 2627;
        private readonly DbErrorMessagesConfiguration _errorMessages;

        public SqlServerDbExceptionParserProvider(DbErrorMessagesConfiguration errorMessages)
        {
            _errorMessages = errorMessages ?? throw new ArgumentNullException(nameof(errorMessages));
        }

        public override string Parse(Exception e)
        {
            var dbUpdateEx = e as DbUpdateException;

            if (dbUpdateEx?.InnerException is SqlException sqlEx)
            {
                //This is a DbUpdateException on a SQL database

                if (sqlEx.Number == SqlServerViolationOfUniqueConstraint
                    || sqlEx.Number == SqlServerViolationOfUniqueIndex)
                {
                    //We have an error we can process
                    var valError = ParseUniquenessError(sqlEx.Message, _errorMessages.UniqueErrorTemplate, _errorMessages.CombinationUniqueErrorTemplate);
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