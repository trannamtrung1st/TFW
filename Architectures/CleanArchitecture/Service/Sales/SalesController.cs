using Application.Sales.Commands.CreateSale;
using Application.Sales.Queries.GetSaleDetail;
using Application.Sales.Queries.GetSalesList;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly IGetSalesListQuery _listQuery;
        private readonly IGetSaleDetailQuery _detailQuery;
        private readonly IMediator _mediator;

        public SalesController(IGetSalesListQuery listQuery,
            IGetSaleDetailQuery detailQuery,
            IMediator mediator)
        {
            _listQuery = listQuery;
            _detailQuery = detailQuery;
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IEnumerable<SalesListItemModel>> GetSalesListAsync()
        {
            return await _listQuery.ExecuteAsync();
        }

        [HttpGet("{id}")]
        public async Task<SaleDetailModel> GetSaleDetailAsync(int id)
        {
            return await _detailQuery.ExecuteAsync(id);
        }

        [HttpPost("")]
        public async Task<CreatedResult> CreateAsync(CreateSaleCommand sale)
        {
            var result = await _mediator.Send(sale);

            return Created($"/api/sales/{result.Id}", new
            {
                result.Id,
                result.Date
            });
        }
    }
}
