namespace QueryMaker
{
    public interface IExpression2Query
    {
        string ToSql();
        SqlKata.Query ToSqlQuery();
    }
}