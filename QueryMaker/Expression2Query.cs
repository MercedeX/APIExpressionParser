using System;

namespace QueryMaker
{
    public class Expression2Query :IExpression2Query
    {
        readonly string _input;

        public Expression2Query(string input)
        {
            _input = input?.Trim();
            if(string.IsNullOrEmpty(_input))
            {
                throw new ArgumentException("Input expression as string cannot be empty");
            }
        }

        public string ToSql() => null;

        public SqlKata.Query ToSqlQuery()
        {
            return null;
        }
    }
}