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

            //Add table from there you want to select.
            var table = new FromTable("Table1", "t1");

            // Perform a join 
            table.JoinOn("Table2", "t2", "Column1", "Column2", JoinType.Inner);

            //add table
            queryBuilder.AddFromTable(table.TableString);

            //Create Where clauses using predicates query builder provides you methods to add predicate
            //there are two ways you can add predicate  either by string or predicate object
            var textMatchExpression = new Predicate("t1.[Column3] like 'Joe'");

            //AddPredicate also takes just string if you have only one where condition.
            //add predicate  and specify the operation type supports and or and not
            queryBuilder.AddPredicate(textMatchExpression, OperationType.And);

            var datesExpressions1 = new List<string>();
            datesExpressions1.Add("f.[Column5] >= '2015-09-01'");//
            datesExpressions1.Add("f.[Column6] <= '2015-12-31'");

            var dateExpressions2 = new List<string>();
            dateExpressions2.Add("f.[Column7] >= '2015-10-01'");
            dateExpressions2.Add("f.[Column8] >= '2015-12-31'");

            //add groups of predicates
            queryBuilder.AddPredicate(datesExpressions1, OperationType.And,OperationType.Or);
            queryBuilder.AddPredicate(dateExpressions2, OperationType.And, OperationType.Empty);

            queryBuilder.GenerateQuery();

        }
    }
}
