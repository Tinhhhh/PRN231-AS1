using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repository;

namespace SilverJewelry_VVT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SilverJewelryController : ODataController
    {
        private readonly ISilverJewelryRepo jewelryRepository;
        private readonly IBranchAccountRepo accountRepository;
        private readonly ICategoryRepo categoryRepository;
        public SilverJewelryController(ISilverJewelryRepo jewelryRepository, IBranchAccountRepo accountRepository, ICategoryRepo categoryRepository)
        {
            this.jewelryRepository = jewelryRepository;
            this.accountRepository = accountRepository;
            this.categoryRepository = categoryRepository;
        }

        [Authorize(Roles = "1,2")]
        [EnableQuery]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(categoryRepository.GetCategories());
        }

        [Authorize(Roles = "1")]
        [EnableQuery]
        [HttpGet("id")]
        public IActionResult GetById([FromODataUri] string id)
        {
            var entity = jewelryRepository.GetSilverJewelry(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [Authorize(Roles = "1,2")]
        [EnableQuery]
        [HttpGet("all")]
        public IActionResult GetByIdGetSilverJewelries()
        {
            return Ok(jewelryRepository.GetSilverJewelries());
        }

        [Authorize(Roles = "1")]
        [HttpPost("create")]
        public IActionResult CreateSilverJewelry([FromBody] SilverJewelry silverJewelry)
        {
            jewelryRepository.AddJewelry(silverJewelry);
            //string locationUri = Url.Link("GetById", new { id = silverJewelry.SilverJewelryId });
            return Ok("Save Successfully");
        }

        [Authorize(Roles = "1")]
        [HttpPost("update")]
        public IActionResult UpdateSilverJewelry([FromBody] SilverJewelry silverJewelry)
        {
            jewelryRepository.UpdateJewelry(silverJewelry);
            //string locationUri = Url.Link("GetById", new { id = silverJewelry.SilverJewelryId });
            return Ok("Update Successfully");
        }

        [Authorize(Roles = "1")]
        [HttpPost("delete")]
        public IActionResult UpdateSilverJewelry([FromQuery] string id)
        {
            jewelryRepository.DeleteJewelry(id);
            //string locationUri = Url.Link("GetById", new { id = silverJewelry.SilverJewelryId });
            return Ok("Delete Successfully");
        }

        [Authorize(Roles = "1,2")]
        [EnableQuery]
        [HttpGet("search")]
        public IActionResult SearchSilverJewelry()
        {
            var jewelries = jewelryRepository.GetSilverJewelries();
            return Ok(jewelries);
        }


    }
}
