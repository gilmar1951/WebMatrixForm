using MatrixForm.Data;
using MatrixForm.Models;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses.ResultOperators;
using SalesWebMvc.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixForm.Services
{
    public class MatrixService
    {
        private readonly MatrixFormContext _context;

        public MatrixService(MatrixFormContext context)
        {
            _context = context;
        }

        public async Task<List<Matrix>> FindAllAsync()
        {
            return await _context.Matrix.OrderBy(x => x.Order).ToListAsync();
        }

        public async Task InsertAsync(Matrix matrix)
        {
            matrix.SetOrder(OrderInclude() + 1);
            matrix.SetDateCreated(DateTime.Now);
            matrix.SetDateModified(DateTime.Now);
            _context.Add(matrix);
            await _context.SaveChangesAsync();

        }

        private int OrderInclude()
        {
            var obj = _context.Matrix.OrderBy(x => x.Order).LastOrDefault();
            return obj.Order;

        }

        public async Task<Matrix> FindByIdAsync(int id)
        {
            return await _context.Matrix.FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Matrix.FindAsync(id);
                _context.Matrix.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task UpdateAsync(Matrix obj)
        {
            if (!await _context.Matrix.AnyAsync(x => x.Id == obj.Id))
            {
                throw new NotFoundException("I");
            }
            try
            {
                obj.SetDateModified(DateTime.Now);
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }

            
        }

        public async Task MoveUpAsync(Matrix obj)
        {
            if (!await _context.Matrix.AnyAsync(x => x.Id == obj.Id))
            {
                throw new NotFoundException("I");
            }
            try
            {
                List<Matrix> result = await _context.Matrix.OrderBy(x => x.Order).ToListAsync();
                int maxOrder = OrderInclude();

                Matrix matrix = await _context.Matrix.FirstOrDefaultAsync(x => x.Order == obj.Order);

                obj.SetOrder(obj.Order -1);
                matrix.SetOrder(matrix.Order +1);

                _context.Update(obj);
                await _context.SaveChangesAsync();
                _context.Update(matrix);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        
    }
}
