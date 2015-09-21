using Spectrum.WildFire.Dynamic;
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
        private List<Table> Tables
        {
            get
            {
                if (_Tables == null)
                {
                    _Tables = new List<Table>();
                }
                return _Tables;
            }
        }
        private List<Table>  _Tables;
        private List<Table> InitialTable
        {
            get
            {
                if (_InitialTable == null)
                {
                    _InitialTable = new List<Table>();
                }
                return _InitialTable;
            }
        }
        private List<Table> _InitialTable;
        public FromTable()
        {

        }

        public FromTable(string tableName, string tableAlias)
        {
            var table = new Table()
            {
                 TableName = tableName,
                  TableAlias = tableAlias
            };
            this.InitialTable.Add(table);
        }
        public FromTable(Table table)
        {
           
            this.InitialTable.Add(table);
        }

        public void JoinOn(Table table)
        {

            if (!String.IsNullOrEmpty(table.TableName) || !String.IsNullOrEmpty(table.TableAlias) || !String.IsNullOrEmpty(table.Column1Name) | !String.IsNullOrEmpty(table.Column2Name))
            {
                this.Tables.Add(table);
                switch (table.JoinType)
                {
                    case JoinType.Inner:
                        if (Tables.Count == 0)
                        {
                            TableString += String.Format(" [{1}] {0} ", InitialTable.First().TableAlias, InitialTable.First().TableName);
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(TableString))
                            {
                                TableString += String.Format(" [{1}] {0} ", InitialTable.First().TableAlias, InitialTable.First().TableName);
                            }
                            var initialTableAlias = InitialTable.First().TableAlias;
                            foreach (var t in Tables)
                            {
                                var table2Alias = t.TableAlias;
                                var expression = String.Format(" {0}.[{1}] = {2}.[{3}]", initialTableAlias, t.Column1Name, table2Alias, t.Column2Name);
                                TableString += String.Format(" inner join [{0}] {1} on {2} ", t.TableName, t.TableAlias, expression);
                            }
                        }
                        break;
                    case JoinType.Outer:
                        if (Tables.Count == 0)
                        {
                            TableString += InitialTable.First().TableName;
                        }
                        else
                        {
                            var initialTableAlias = InitialTable.First().TableAlias;
                            foreach (var t in Tables)
                            {
                                var table2Alias = t.TableAlias;
                                var expression = String.Format(" {0}.[{1}] = {2}.[{3}]", initialTableAlias, t.Column1Name, table2Alias, t.Column2Name);
                                TableString += String.Format(" outer join [{0}] on {1} ", t.TableName, expression);
                            }
                        }
                        break;
                }
            }
        }


    }
}
