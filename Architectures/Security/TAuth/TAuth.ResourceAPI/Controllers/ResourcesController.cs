using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAuth.ResourceAPI.Entities;
using TAuth.ResourceAPI.Models.Resource;


namespace TAuth.ResourceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly ResourceContext _context;

        public ResourcesController(ResourceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<ResourceListItemModel>> Get()
        {
            var list = await _context.Resources.Select(o => new ResourceListItemModel
            {
                Id = o.Id,
                Name = o.Name
            }).ToArrayAsync();

            return list;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _context.Resources.Select(o => new ResourceDetailModel
            {
                Id = o.Id,
                Name = o.Name
            }).SingleOrDefaultAsync(o => o.Id == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<int> Post([FromBody] CreateResourceModel model)
        {
            var entity = new ResourceEntity
            {
                Name = model.Name
            };

            await _context.Resources.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Resources.Select(o => new ResourceEntity
            {
                Id = o.Id
            }).SingleOrDefaultAsync(o => o.Id == id);

            if (item == null)
                return NotFound();

            _context.Resources.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
