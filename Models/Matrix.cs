using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixForm.Models
{
    public class Matrix
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        /*        public int Position { get; set; }*/
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int Order { get; set; }
        public void SetOrder(int order)
        {
            this.Order = order;
        }
        public void SetDateCreated(DateTime dateTime)
        {
            this.DateCreated = dateTime;
        }
        public void SetDateModified(DateTime dateTime)
        {
            this.DateModified = dateTime;
        }


    }
}
