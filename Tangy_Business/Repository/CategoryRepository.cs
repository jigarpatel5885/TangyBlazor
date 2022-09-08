using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper; 
        public CategoryRepository(ApplicationDbContext dbContext,IMapper mapper)
        {
            dbContext = _dbContext;
            mapper = _mapper;
        }
        public async Task<CategoryDTO> Create(CategoryDTO objDTO)
        {
            var category =  _mapper.Map<CategoryDTO, Category>(objDTO);
            var addedObj =   _dbContext.Add(category);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Category, CategoryDTO>(addedObj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var objCategory = _dbContext.Categories.Where(x=>x.Id == id).FirstOrDefault();
            
            if(objCategory != null)
            {
                _dbContext.Categories.Remove(objCategory);
                return await _dbContext.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<CategoryDTO> Get(int id)
        {
            var objCategory =  await _dbContext.Categories.FirstOrDefaultAsync(u => u.Id == id);

            if (objCategory !=null)
            {
                return _mapper.Map<Category, CategoryDTO>(objCategory);
            }
            return new CategoryDTO();
        }

        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var objCategories =  _dbContext.Categories;

            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(objCategories);

        }

        public async Task<CategoryDTO> Update(CategoryDTO objDTO)
        {
            var objCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == objDTO.Id);

            if (objCategory !=null)
            {
                objCategory.Name = objDTO.Name;           

                _dbContext.Categories.Update(objCategory);

                return _mapper.Map<Category, CategoryDTO>(objCategory);
            }
            return objDTO;

        }
    }
}
