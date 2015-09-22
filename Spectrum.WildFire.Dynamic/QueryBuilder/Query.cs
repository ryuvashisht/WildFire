using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrum.WildFire.Dynamic
{
    public class Query
    {
        public string QueryString { get; set; }

        public string Select { get; set; }

        public string From { get; set; }

        public string Where { get; set; }

        public string Fetch { get; set; }

        public string Order
        {
            get;
            set;
        }
        /// <summary>
        /// order ASC or DESC
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="order"></param>
        public void OrderBy(string orderBy, OrderByType orderbyType)
        {
            if (orderbyType.HasFlag(OrderByType.ASC))
            {
                this.Order += String.Format(" ORDER BY {0} ASC", orderBy);
            }
            else
            {
                this.Order += String.Format(" ORDER BY {0} DESC", orderBy);
            }

        }

        public void FetchNext(string numberOfRows)
        {
            try
            {
                if (String.IsNullOrEmpty(this.Order))
                {
                    throw new Exception("The fetch will not work without orderby.");
                }
                else
                {
                    this.Fetch += String.Format(" OFFSET 0 ROWS FETCH NEXT {0} ROWS ONLY", numberOfRows);

                }
            }
            catch (Exception ex)
            {
                throw new NullReferenceException(ex.ToString());
            }
           
        }

        public void FetchNext(string offset,string numberOfRows)
        {
            try
            {
                if (String.IsNullOrEmpty(this.Order))
                {
                    throw new Exception("The fetch will not work without orderby.");
                }
                else
                {
                    this.Fetch += String.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY",offset, numberOfRows);

                }
            }
            catch (Exception ex)
            {
                throw new NullReferenceException(ex.ToString());
            }

        }
    }
}
