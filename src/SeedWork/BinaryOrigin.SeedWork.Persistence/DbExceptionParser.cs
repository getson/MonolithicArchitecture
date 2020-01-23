using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BinaryOrigin.SeedWork.Persistence.Ef
{
    public abstract class DbExceptionParser : IDbExceptionParserProvider
    {
        protected virtual Regex UniqueConstraintRegex => new Regex("'([a-zA-Z0-9]*)_([a-zA-Z0-9]*)_([a-zA-Z0-9]*)_?([a-zA-Z0-9]*)?_?([a-zA-Z0-9]*)?_?([a-zA-Z0-9]*)?'", RegexOptions.Compiled);

        public virtual string ParseUniquenessError(string dbErrorMessage, string uniqueErrorTemplate, string combinationUniqueErrorTemplate)
        {
            var matches = UniqueConstraintRegex.Matches(dbErrorMessage);
            if (matches.Count == 0)
            {
                return null;
            }

            var matchedStrings = matches[0].Groups.Cast<Group>()
                                     .Select(g => g.Value).Where(v => !string.IsNullOrWhiteSpace(v))
                                     .Skip(3) // skip name of the constraint + IV + table
                                     .ToArray();

            if (matchedStrings.Length == 1)
            {
                return string.Format(uniqueErrorTemplate, matchedStrings[0]);
            }
            else
            {
                var combinationFields = new StringBuilder();
                for (var i = 0; i < matchedStrings.Length; i++)
                {
                    var uniqueField = matchedStrings[i];
                    var isIdSuffix = uniqueField.EndsWith("Id", StringComparison.CurrentCultureIgnoreCase);
                    if (isIdSuffix)
                    {
                        combinationFields.Append(uniqueField.Remove(uniqueField.Length - 2));
                    }
                    else
                    {
                        combinationFields.Append(uniqueField);
                    }
                    if (i != matchedStrings.Length - 1)
                    {
                        combinationFields.Append(", ");
                    }
                }
                return string.Format(combinationUniqueErrorTemplate, combinationFields);
            }
        }

        public abstract string Parse(Exception e);
    }
}