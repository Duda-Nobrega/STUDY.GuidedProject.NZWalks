using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repository;

namespace NZWalksAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    //Usamos o dbContext para pegar as informações do banco de dados
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }
        // Get All Regions
        //Get: https://localhost:portnumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll() /* IActionResult é um retorno genérico que pode ser qualquer classe que implementa a interface IActionResult,                                * enquanto o Task é usado para representar uma operação assíncrona que retorna um resultado quando concluída*/
        {
            //Get data From DataBase - Domain Model
            var regions = await regionRepository.GetAllAsync();

            //Map Domain Models to DTOs
            var regionsDto = new List<RegionsDto>();
            foreach (var region in regions)
            {
                regionsDto.Add(new RegionsDto()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl,
                });
            }
            //Return DTOs

            return Ok(regionsDto);
        }


        [HttpGet]
        [Route("{id:guid}")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var Region = await regionRepository.GetByIdAsync(id);
            //var Region = dbContext.Regions.FirstOrDefault(x => x.Id = id);

            if (Region == null)
            {
                return NotFound();
            }

            var regionsDto = new RegionsDto
            {
                Id = Region.Id,
                Name = Region.Name,
                Code = Region.Code,
                RegionImageUrl = Region.RegionImageUrl,
            };

            return Ok(regionsDto);
        }
        //POST to create new Region
        [HttpPost]

        public async Task<IActionResult> CreateRegionAsync([FromBody] AddNewRegionRequestDto addNewRegionRequestDto)
        {
            //Convert DTO to domain
            var regionDomainModel = new Region
            {
                Code = addNewRegionRequestDto.Code,
                Name = addNewRegionRequestDto.Name,
                RegionImageUrl = addNewRegionRequestDto.RegionImageUrl,
            };

            //Use Domain Model to create a new Region 
            regionDomainModel = await regionRepository.CreateRegionAsync(regionDomainModel);
            await dbContext.SaveChangesAsync(); //Salva no banco de dados

            //Map Domain model back to DTO
            var regionDto = new RegionsDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

        }
        //Update Region
        //PUT: Https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestoDto updateRegionRequestoDto)
        {

            //Map dto to domain model
            var regionDomainModel = new Region
            {
                Code = updateRegionRequestoDto.Code,
                Name = updateRegionRequestoDto.Name,
                RegionImageUrl = updateRegionRequestoDto.RegionImageUrl,
            };
            //check if region exists
            regionDomainModel = await regionRepository.UpdateRegionAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            // map dto to Domain Model

            regionDomainModel.Code = updateRegionRequestoDto.Code;
            regionDomainModel.Name = updateRegionRequestoDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestoDto.RegionImageUrl;

            await dbContext.SaveChangesAsync();

            //convert Domain Model to dto
            var regionDto = new RegionsDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return Ok(regionDto);

        }
        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //Check if the region exists
            var regionDomainModel = await regionRepository.DeleteRegionAsync(id);

            if (regionDomainModel == null)
            {

                return NotFound();
            }

            //Delete the regio

            dbContext.Regions.Remove(regionDomainModel);
            await dbContext.SaveChangesAsync();

            //return the deleted region back
            //map Domain model to dto
            var RegionDto = new RegionsDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,

            };

            return Ok(RegionDto);
        }





    }

}
