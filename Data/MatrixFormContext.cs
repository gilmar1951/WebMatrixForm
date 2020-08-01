using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MatrixForm.Models;

namespace MatrixForm.Data
{
    public class MatrixFormContext : DbContext
    {
        public MatrixFormContext (DbContextOptions<MatrixFormContext> options)
            : base(options)
        {
        }

        public DbSet<Matrix> Matrix { get; set; }
    }
}
