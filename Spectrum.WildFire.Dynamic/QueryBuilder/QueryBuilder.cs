using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrum.WildFire.Dynamic
{
    public class QueryBuilder
    {
        private List<Predicate> Predicates
        {
            get
            {
                if (_Predicates == null)
                {
                    _Predicates = new List<Predicate>();
                }
                return _Predicates;
            }
        }
        private List<Predicate> _Predicates;

        private List<string> SelectItems
        {
            get
            {
                if (_SelectItems == null)
                {
                    _SelectItems = new List<string>();
                }
                return _SelectItems;
            }
        }
        private List<string> _SelectItems;

        private List<string> FromItems
        {
            get
            {
                if (_FromItems == null)
                {
                    _FromItems = new List<string>();
                }
                return _FromItems;
            }
        }
        private List<string> _FromItems;

        public void AddPredicate(string expression)
        {
            var predicate = new Predicate(expression);
            Predicates.Add(predicate);
        }

        /// <summary>
        /// Adds the predicate object to list with Or and And methods.
        /// </summary>
        /// <param name="expression">Of Predicate object type</param>
        public void AddPredicate(Predicate predicate,OperationType operationType)
        {
            predicate.OperationType = operationType;
            Predicates.Add(predicate);
        }

        public void AddPredicate(List<string> predicates,OperationType operationType)
        {

            var expression = "(";
            var operation = "";
            switch(operationType){
                case OperationType.Or:
                    operation = " OR ";
                    break;
                case OperationType.And:
                    operation = " AND ";
                    break;
                case OperationType.Not:
                    operation = " NOT ";
                    break;

            }
            if (predicates.Count > 1)
            {
                expression += predicates[0];
                for(var i=1;i<predicates.Count; i++)
                {
                    expression += operation + predicates[i];
                }
            }
            else
            {
                expression += predicates[0];
            }
           
            expression += ")";
            var predicate = new Predicate(expression);
            predicate.OperationType = operationType;
            Predicates.Add(predicate);
        }
        public void AddFromTable(string fromTable)
        {
            FromItems.Add(fromTable);
        }

        public void AddSelectColumn(string selectColumn)
        {
            SelectItems.Add(selectColumn);
        }

      

        public string Query { get; set; }

        /// <summary>
        /// order ASC or DESC
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="order"></param>
       public void OrderBy(string orderBy,bool order){
           if (order)
           {
               Query += String.Format(" ORDERBY BY {0} ASC", orderBy);
           }
           else
           {
               Query += String.Format(" ORDERBY BY {0} DESC", orderBy);
           }
           
        }

        public void FetchNext(string numberOfRows){
            Query += String.Format(" FETCH NEXT {0} ROWS ONLY",numberOfRows);
        }

        public void GenerateQuery()
        {
            var select = "";
            var from = "";
            var where = "";
            try
            {
                var operation = "";
                if (Predicates.Count == 1)
                {
                    where += Predicates[0].Expression;
                }
                else
                {
                    where += operation + Predicates[0].Expression;
                    for(var i=1; i<Predicates.Count; i++)
                    {
                        switch (Predicates[i].OperationType)
                        {
                            case OperationType.Or:
                                operation = " OR ";
                                break;
                            case OperationType.And:
                                operation = " AND ";
                                break;
                            case OperationType.Not:
                                operation = " NOT ";
                                break;

                        }
                        where += operation + Predicates[i].Expression;
                    }
                }
                if (FromItems.Count == 0)
                {
                    throw new Exception("You are missing tables which you want to select from.");
                }
                else
                {
                    foreach (var item in FromItems)
                    {
                        from += item;
                    }
                }
                if (SelectItems.Count == 0)
                {
                    select = " * ";
                }
                else
                {
                    foreach(var item in SelectItems){
                        select += item;
                    }
                   
                }

                Query = String.Format("SELECT {0} FROM {1} WHERE {2} ;", select, from, where);
                
            }
            catch(Exception ex){

            }
           
        }
    }
}
