using EvaSystem.Data;
using EvaSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Services
{
    public class PositionManager
    {
        DataContext _dataContext;

        public PositionManager(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<PositionModel> AddNewPositionAsync(string posName)
        {
            var existPos = await GetPositionByNameAsync(posName);
            
            if(existPos !=null)
            {
                return existPos;
            }

            await _dataContext.AddAsync(new PositionModel { PositionName = posName });
            await _dataContext.SaveChangesAsync();

            return await GetPositionByNameAsync(posName);
        }

        public async Task<bool> DeletePositionAsync(int posId)
        {
            var existPos = await GetPositionByIDAsync(posId);

            if(existPos == null)
            {
                return false;
            }

            var usersWithPos = _dataContext.Users.Where(x => x.PositionId == existPos.PositionId);

            if(usersWithPos.Count()>0)
            {
                return false;
            }

            var crtitsToPosition = _dataContext.CriterionsToPosition.Where(x => x.PositionId == existPos.PositionId);

            _dataContext.CriterionsToPosition.RemoveRange(crtitsToPosition);
            _dataContext.Positions.Remove(existPos);
            await _dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<PositionModel> GetPositionByNameAsync(string posName)
        {
            var existPos = await _dataContext.Positions.FirstOrDefaultAsync(x => x.PositionName == posName);

            if(existPos == null)
            {
                return null;
            }

            return existPos;

        }

        public async Task<PositionModel> GetPositionByIDAsync(int id)
        {
            var existPos = await _dataContext.Positions.FirstOrDefaultAsync(x => x.PositionId == id);

            if(existPos == null)
            {
                return null;
            }

            return existPos;
        }

        public async Task<List<PositionModel>> GetAllPosition()
        {
            return await _dataContext.Positions.ToListAsync();
        }
    }
}
