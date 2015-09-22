using Spectrum.WildFire.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrum.WildFire.Dynamic
{
    public class QueryBuilder
    {
        public string TableString { get; set; }

        public Table Table { get; set; }
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

        private List<string> FromTables
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

        /// <summary>
        /// Adds predicate using a string without the operation type.
        /// </summary>
        /// <param name="expression"></param>
        public void AddPredicate(string expression)
        {
            var predicate = new Predicate(expression);
            Predicates.Add(predicate);
        }

        /// <summary>
        /// Adds the predicate object to list with Or and And methods.
        /// </summary>
        /// <param name="expression">Of Predicate object type</param>
        public void AddPredicate(Predicate predicate, OperationType operationType)
        {
            predicate.OperationType = operationType;
            Predicates.Add(predicate);
        }

        /// <summary>
        /// Adds list of predicates which are list of strings.
        /// </summary>
        /// <param name="predicates"></param>
        /// <param name="operationType"></param>
        public void AddPredicate(List<string> predicates, OperationType innerOperation,OperationType operationType)
        {

            var expression = "(";
            var operation = "";
            switch (innerOperation)
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
            if (predicates.Count > 1)
            {
                expression += predicates[0];
                for (var i = 1; i < predicates.Count; i++)
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
            FromTables.Add(fromTable);
        }
        public void AddFromTable(Table fromTable)
        {
            FromTables.Add(String.Format("{0} {1}",fromTable.TableName,fromTable.TableAlias));
        }

        public void AddTable(Table table)
        {
            this.Table = table; 
        }

        public void AddSelectColumn(string selectColumn)
        {
            SelectItems.Add(selectColumn);
        }



        public Query Query
        {
            
            get{
                if (_Query == null)
                {
                    _Query = new Query();
                }
                return _Query;
            }
        }
        private Query _Query;
        public void GenerateQuery()
        {
            GenerateJoins();
           
            try
            {
                var operation = "";
                if (Predicates.Count == 0)
                {
                  
                }
                else if (Predicates.Count == 1)
                {
                    this.Query.Where += Predicates[0].Expression;
                   
                }
                else
                {
                    this.Query.Where += Predicates[0].Expression + " " + Predicates[0].OperationType.ToString() + " ";
                    var count = 0;
                    for (var i = 1; i < Predicates.Count; i++)
                    {
                        count++;
                        var lastObject = Predicates.LastOrDefault();
                        if (count < Predicates.Count)
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
                                case OperationType.Empty:
                                    operation = "";
                                    break;

                            }
                            if (Predicates[i].Equals(lastObject))
                            {
                                this.Query.Where += Predicates[i].Expression + " " ;

                            }
                            else
                            {
                                this.Query.Where += Predicates[i].Expression + " " + operation;

                            }
                        }
                      
                    }
                }
                if (FromTables.Count == 0)
                {
                    throw new Exception("You are missing tables which you want to select from.");
                }
                else
                {
                    foreach (var item in FromTables)
                    {
                        this.Query.From += item ;
                    }
                }
                if (SelectItems.Count == 0)
                {
                     this.Query.Select = " * ";
                }
                else
                {
                    var count = 0;
                    foreach (var item in SelectItems)
                    {
                        count++;
                        var ItemsCount = SelectItems.Count;
                        this.Query.Select += item;
                        if (count < ItemsCount)
                        {
                            this.Query.Select += ", ";
                        }
                    }
                }
                if (!String.IsNullOrEmpty(this.Query.QueryString))
                {

                }

                if (Predicates.Count == 0)
                {
                this.Query.QueryString = String.Format("SELECT {0} FROM {1} {2} {3};", this.Query.Select, this.Query.From, this.Query.Order, this.Query.Fetch);
                }else{
                this.Query.QueryString = String.Format("SELECT {0} FROM {1} WHERE {2} {3} {4};", this.Query.Select, this.Query.From, this.Query.Where,this.Query.Order, this.Query.Fetch);
                }
            }
            catch (Exception ex)
            {
                throw new NullReferenceException(ex.ToString());
            }

        }
        public void GenerateJoins()
        {
            TableString = "";
            if (!String.IsNullOrEmpty(this.Table.TableName) || !String.IsNullOrEmpty(this.Table.TableAlias) || !String.IsNullOrEmpty(this.Table.Column1Name) | !String.IsNullOrEmpty(this.Table.Column2Name))
            {
                switch (this.Table.JoinType)
                {
                    case JoinType.Inner:
                        if (this.Table.Tables.Count == 0)
                        {
                            TableString += String.Format(" [{1}] {0} ", this.Table.TableAlias, this.Table.TableName);
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(TableString))
                            {
                                TableString += String.Format(" [{1}] {0} ", this.Table.TableAlias, this.Table.TableName);
                            }
                            var initialTableAlias = this.Table.TableAlias;
                            foreach (var t in this.Table.Tables)
                            {
                                var table2Alias = t.TableAlias;
                                var expression = String.Format(" {0}.[{1}] = {2}.[{3}]", initialTableAlias, t.Column1Name, table2Alias, t.Column2Name);
                                TableString += String.Format(" inner join [{0}] {1} on {2} ", t.TableName, t.TableAlias, expression);
                            }
                        }
                        break;
                    case JoinType.Outer:
                        if (this.Table.Tables.Count == 0)
                        {
                            TableString += this.Table.TableName;
                        }
                        else
                        {
                            var initialTableAlias = this.Table.TableAlias;
                            foreach (var t in this.Table.Tables)
                            {
                                var table2Alias = t.TableAlias;
                                var expression = String.Format(" {0}.[{1}] = {2}.[{3}]", initialTableAlias, t.Column1Name, table2Alias, t.Column2Name);
                                TableString += String.Format(" outer join [{0}] on {1} ", t.TableName, expression);
                            }
                        }
                        break;
                }
               }
            this.AddFromTable(TableString);
        }

        public void GenerateCountQuery()
        {
                if (Predicates.Count == 0)
                {
                    this.Query.CountQueryString = String.Format("SELECT Count(*) FROM {0} {1};",this.Query.From, this.Query.Order);
                }
                else
                {
                    this.Query.CountQueryString = String.Format("SELECT Count(*) FROM {0} WHERE {1} {2};", this.Query.From, this.Query.Where, this.Query.Order);
                }
            }
        
    }
}
