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
            var table = new FromTable("Fact_Sales", "f");

            // Perform a join 
            table.JoinOn("DimensionMembers", "d0", "Currency_Id", "DimensionMemberId", JoinType.Inner);

            //add table

            queryBuilder.AddFromTable(table.TableString);

            //Create Where clauses using predicates query builder provides you methods to add predicate
            //there are two ways you can add predicate  either by string or predicate object
            var cancellationTermExpression = new Predicate("f.[CancellationTerms] like @search");

            //AddPredicate also takes just string if you have only one where condition.
            //add predicate  and specify the operation type supports and or and not
            queryBuilder.AddPredicate(cancellationTermExpression, OperationType.And);


            var bookingDates = new List<string>();
            bookingDates.Add("f.[BookingDate] >= '2015-09-01'");//
            bookingDates.Add("f.[BookingDate] <= '2015-12-31'");

            var cancellationDates = new List<string>();
            cancellationDates.Add("f.[CollectionDate] >= '2015-10-01'");
            cancellationDates.Add("f.[CollectionDate] >= '2015-12-31'");

            //add groups of predicates
            queryBuilder.AddPredicate(bookingDates, OperationType.And);
            queryBuilder.AddPredicate(cancellationDates, OperationType.Or);

            queryBuilder.GenerateQuery();


        }
    }
}
