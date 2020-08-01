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
            matrix.SetOrder(await OrderInclude() + 1);
            matrix.SetDateCreated(DateTime.Now);
            matrix.SetDateModified(DateTime.Now);
            _context.Add(matrix);
            await _context.SaveChangesAsync();

        }

        public async Task<int> OrderInclude()
        {
            var obj = await _context.Matrix.OrderBy(x => x.Order).LastOrDefaultAsync();
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

        public async Task MoveSubtractAsync(int objId)
        {
            if (!await _context.Matrix.AnyAsync(x => x.Id == objId))
            {
                throw new NotFoundException("I");
            }
            try
            {
                Matrix obj = await FindByIdAsync(objId);
                Matrix obj2 = await _context.Matrix.FirstOrDefaultAsync(x => x.Order == (obj.Order - 1));
                obj.SetOrder(obj.Order - 1);
                _context.Update(obj);
                await _context.SaveChangesAsync();
                obj2.SetOrder(obj.Order + 1);
                _context.Update(obj2);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        public async Task MoveAddAsync(int objId)
        {
            if (!await _context.Matrix.AnyAsync(x => x.Id == objId))
            {
                throw new NotFoundException("I");
            }
            try
            {
                Matrix obj = await FindByIdAsync(objId);
                Matrix obj2 = await _context.Matrix.FirstOrDefaultAsync(x => x.Order == (obj.Order + 1));
                obj.SetOrder(obj.Order + 1);
                _context.Update(obj);
                await _context.SaveChangesAsync();
                obj2.SetOrder(obj.Order - 1);
                _context.Update(obj2);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
