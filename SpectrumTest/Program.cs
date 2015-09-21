using Spectrum.WildFire.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var queryBuilder = new QueryBuilder();

           
            var t0 = new Table()
            {
                TableName = "Table0",
                TableAlias = "t0"
            };
            //// Perform a join 

            var t1 = new Table()
            {
                TableName = "Table2",
                Column1Name = "Column1",
                Column2Name = "Column2",
                JoinType = JoinType.Inner,
                TableAlias = "t2"
            };
            var t2 = new Table()
            {
                TableName = "Table1",
                Column1Name = "Column1",
                Column2Name = "Column2",
                JoinType = JoinType.Inner,
                TableAlias = "t1"
            };
            var t3 = new Table()
            {
                TableName = "Table3",
                Column1Name = "Column1",
                Column2Name = "Column2",
                JoinType = JoinType.Inner,
                TableAlias = "t3"
            };
            var t4 = new Table()
            {
                TableName = "Table4",
                Column1Name = "Column1",
                Column2Name = "Column2",
                JoinType = JoinType.Inner,
                TableAlias = "t4"
            };

            ////Create table from first table

            //var tables = new List<Table>();

            //tables.Add(t1);
            //tables.Add(t3);
            //tables.Add(t2);
            //tables.Add(t4);

            //////add table
            //t0.JoinOn(tables);

           

            queryBuilder.AddTable(t0);
            ////Create Where clauses using predicates query builder provides you methods to add predicate
            ////there are two ways you can add predicate  either by string or predicate object
            var textMatchExpression = new Predicate("t1.[Column3] like 'Joe'");

            ////AddPredicate also takes just string if you have only one where condition.
            ////add predicate  and specify the operation type supports and or and not
            queryBuilder.AddPredicate(textMatchExpression, OperationType.And);

            var datesExpressions1 = new List<string>();
            datesExpressions1.Add("f.[Column5] >= '2015-09-01'");
            datesExpressions1.Add("f.[Column6] <= '2015-12-31'");

            var dateExpressions2 = new List<string>();
            dateExpressions2.Add("f.[Column7] >= '2015-10-01'");
            dateExpressions2.Add("f.[Column8] >= '2015-12-31'");
            
            //add groups of predicates
            queryBuilder.AddPredicate(datesExpressions1, OperationType.And, OperationType.Or);
            queryBuilder.AddPredicate(dateExpressions2, OperationType.And, OperationType.Empty);

            //Order by 
            queryBuilder.Query.OrderBy("Column1", true);

            //Fetch overloaded with offset and number of rows.
            queryBuilder.Query.FetchNext("2","20");

            queryBuilder.GenerateQuery();

        }
    }
}
