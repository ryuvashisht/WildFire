using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrum.WildFire.Dynamic
{
    public class Predicate
    {
        public string Expression { get; set; }

        public OperationType OperationType { get; set; }

        public Predicate()
        {
        }

        public Predicate(string expression)
        {
            this.Expression = expression;
        }
        public void Or(string expression)
        {
            this.Expression = String.Format(" OR {0}", expression);
        }

        public void And(string expression)
        {
            this.Expression = String.Format(" AND {0}", expression);
        }

        
    }
}
