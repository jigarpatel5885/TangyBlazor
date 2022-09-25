using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tangy_Business.Repository.IRepository;
using Tangy_Common;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;
using Tangy_Models.ViewModels;

namespace Tangy_Business.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public OrderRepository(ApplicationDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<OrderDTO> Create(OrderDTO objDTO)
        {
            try
            {
                var obj = _mapper.Map<OrderDTO, Order>(objDTO);

                _db.OrderHeader.Add(obj.OrderHeader);
                await _db.SaveChangesAsync();

                foreach (var details in obj.OrderDetails)
                {
                    details.OrderHeaderId = obj.OrderHeader.Id;
                }
                 _db.OrderDetail.AddRange(obj.OrderDetails);
                await _db.SaveChangesAsync();

                return new OrderDTO()
                {
                    OrderHeader = _mapper.Map<OrderHeader, OrderHeaderDTO>(obj.OrderHeader),
                    OrderDetails = _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDTO>>(obj.OrderDetails).ToList()
                };

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> Delete(int id)
        {
            var objOrderHeader = await _db.OrderHeader.FirstOrDefaultAsync(x => x.Id == id);
            if (objOrderHeader !=null)
            {
                IEnumerable<OrderDetail> objOrderDetail =  _db.OrderDetail.Where(x => x.OrderHeaderId == id);
                _db.OrderDetail.RemoveRange(objOrderDetail);
                _db.OrderHeader.Remove(objOrderHeader);

                await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<OrderDTO> Get(int id)
        {
            Order order = new()
            {
                OrderHeader = _db.OrderHeader.FirstOrDefault(x => x.Id == id),
                OrderDetails = _db.OrderDetail.Where(x => x.OrderHeaderId == id)
            };

            if(order!=null)
            {
                return _mapper.Map<Order, OrderDTO>(order);
            }

            return new OrderDTO();
            

        }

        public async  Task<IEnumerable<OrderDTO>> GetAll(string? userId = null, string? status = null)
        {
            List<Order> OrderFromDb = new List<Order>();
            IEnumerable<OrderHeader> orderHeaderList = _db.OrderHeader;
            IEnumerable<OrderDetail> orderDetailList = _db.OrderDetail;

            foreach (  OrderHeader header in orderHeaderList)
            {
                Order order = new()
                {
                    OrderHeader = header,
                    OrderDetails = orderDetailList.Where(x => x.OrderHeaderId == header.Id)
                };
                OrderFromDb.Add(order);
            }
            //do some filtering #Todo

            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(OrderFromDb);
        }

        public async Task<OrderHeaderDTO> MarkPaymentSuccessful(int id)
        {
            var data = await _db.OrderHeader.FirstOrDefaultAsync(x => x.Id == id);

            if(data == null)
            {
                return new OrderHeaderDTO();
            }

            if(data.Status == SD.Status_Pending)
            {
                data.Status = SD.Status_Confirmed;
                await _db.SaveChangesAsync();

                return _mapper.Map<OrderHeader, OrderHeaderDTO>(data);
            }

            return new OrderHeaderDTO();
        }
         
        public  async Task<OrderHeaderDTO> UpdateHeader(OrderHeaderDTO objDTO)
        {
            if(objDTO !=null)
            {
                var OrderHeader = _mapper.Map<OrderHeaderDTO, OrderHeader>(objDTO);
                _db.Update(OrderHeader);
                await _db.SaveChangesAsync();
                return _mapper.Map<OrderHeader, OrderHeaderDTO>(OrderHeader);
            }
            return new OrderHeaderDTO();
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
        {
            var data = await _db.OrderHeader.FirstOrDefaultAsync(x => x.Id == orderId);

            if (data == null)
            {
                return false;
            }
            data.Status = status;
            if (data.Status == SD.Status_Shipped)
            {
                data.ShippingDate= DateTime.Now;

            }

            await _db.SaveChangesAsync();

            return true;
        }
    }
}
