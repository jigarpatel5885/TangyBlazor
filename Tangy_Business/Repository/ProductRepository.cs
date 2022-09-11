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
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public ProductRepository(ApplicationDbContext dbContext , IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ProductDTO> Create(ProductDTO objProduct)
        {
            var product = _mapper.Map<ProductDTO, Product>(objProduct);
            var objAddedProduct =  _dbContext.Add(product);
            await _dbContext.SaveChangesAsync();
            
            return _mapper.Map<Product, ProductDTO>(objAddedProduct.Entity);

        }

        public async Task<int> Delete(int id)
        {
            var objProductFromDb = _dbContext.Products.FirstOrDefault(x => x.Id == id);

            if (objProductFromDb !=null)
            {
                _dbContext.Remove(objProductFromDb);

                return await _dbContext.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<ProductDTO> Get(int id)
        {
            var objProductFromDb = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (objProductFromDb != null)
            {
               var objProduct =   _mapper.Map<Product, ProductDTO>(objProductFromDb);

                return objProduct;
            }
            return new ProductDTO();
        }

        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            var products = _dbContext.Products;
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);

        }

        public async Task<ProductDTO> Update(ProductDTO objProduct)
        {
            var objFromDb = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == objProduct.Id);

            if(objFromDb != null)
            {
                objFromDb.Name = objProduct.Name;
                objFromDb.Description = objProduct.Description;
                objFromDb.ImageUrl = objProduct.ImageUrl;
                objFromDb.CategoryId = objProduct.CategoryId;
                objFromDb.Color = objProduct.Color;
                objFromDb.ShopFavourites = objProduct.ShopFavourites;
                objFromDb.CustomerFavourites = objProduct.CustomerFavourites;

                return _mapper.Map<Product,ProductDTO>(objFromDb);

            }
            return objProduct;
        }
    }
}
