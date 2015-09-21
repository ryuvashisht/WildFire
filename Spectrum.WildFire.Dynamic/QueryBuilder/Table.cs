using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrum.WildFire.Dynamic
{
     public class Table
    {
         public string TableName { get; set; }

         public string TableAlias { get; set; }

         public string Column1Name { get; set; }

         public string Column2Name { get; set; }

         public JoinType JoinType { get; set; }


       
        internal List<Table> Tables
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
        internal List<Table> InitialTable
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
        

       
        public void JoinOn(Table table)
        {
            Tables.Add(table);
        }

        public void JoinOn(List<Table> tables)
        {
            Tables.AddRange(tables);
        }
    }
}
