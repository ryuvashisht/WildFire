using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrum.WildFire.Dynamic
{
    public class FromTable
    {
        public string TableString { get; set; }
        public Dictionary<string, string> Tables
        {
            get
            {
                if (_Tables == null)
                {
                    _Tables = new Dictionary<string, string>();
                }
                return _Tables;
            }
        }
        private Dictionary<string, string> _Tables;
        public Dictionary<string, string> InitialTable
        {
            get
            {
                if (_InitialTable == null)
                {
                    _InitialTable = new Dictionary<string, string>();
                }
                return _InitialTable;
            }
        }
        private Dictionary<string, string> _InitialTable;
        public FromTable()
        {

        }

        public FromTable(string tableName, string tableAlias)
        {
            this.InitialTable.Add(tableAlias, tableName);
        }


        public void JoinOn(string tableName, string tableAlias, string column1, string column2, JoinType JoinType)
        {

            if (!String.IsNullOrEmpty(tableName) || !String.IsNullOrEmpty(tableAlias) || !String.IsNullOrEmpty(column1) | !String.IsNullOrEmpty(column2))
            {
                this.Tables.Add(tableAlias, tableName);
                switch (JoinType)
                {
                    case JoinType.Inner:
                        if (Tables.Count == 0)
                        {
                            TableString += String.Format(" [{1}] {0} ", InitialTable.First().Key, InitialTable.First().Value);
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(TableString))
                            {
                                TableString += String.Format(" [{1}] {0} ", InitialTable.First().Key, InitialTable.First().Value);
                            }
                            var initialTableAlias = InitialTable.First().Key;
                            foreach (var table in Tables)
                            {
                                var table2Alias = table.Key;
                                var expression = String.Format(" {0}.[{1}] = {2}.[{3}]", initialTableAlias, column1, table2Alias, column2);
                                TableString += String.Format(" inner join [{0}] {1} on {2} ", table.Value, table.Key, expression);
                            }
                        }
                        break;
                    case JoinType.Outer:
                        if (Tables.Count == 0)
                        {
                            TableString += InitialTable.First().Value;
                        }
                        else
                        {
                            var initialTableAlias = InitialTable.First().Key;
                            foreach (var table in Tables)
                            {
                                var table2Alias = table.Key;
                                var expression = String.Format(" {0}.[{1}] = {2}.[{3}]", initialTableAlias, column1, table2Alias, column2);
                                TableString += String.Format(" outer join [{0}] on {1} ", table.Value, expression);
                            }
                        }
                        break;
                }
            }
        }

    }
}
