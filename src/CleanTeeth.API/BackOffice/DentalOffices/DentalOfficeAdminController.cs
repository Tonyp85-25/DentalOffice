using CleanTeeth.API.Common.DTO;
using CleanTeeth.API.Extensions;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features;
using CleanTeeth.Application.Utilities;
using CleanTeeth.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleanTeeth.API.BackOffice.DentalOffices;

[ApiController]
[Route("api/admin/dentaloffices")]
public class DentalOfficeAdminController : ControllerBase
{

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<DentalOfficeDetailDto>> GetAsync(Guid id, [FromServices] CleanTeethDbContext context)
    {
        var dentalOffice = await context.DentalOffices.FirstOrDefaultAsync(d => d.Id == id);
        return dentalOffice == null ? throw new NotFoundException("Dental office not found") : dentalOffice.ToDto();
    }

    [HttpPost]
    [Authorize(Policy = "RequiresAdmin")]
    public async Task<IActionResult> PostAsync(CreateDentalOfficeDto createDentalOfficeDto,[FromServices] IMediator mediator)
    {
        var command = createDentalOfficeDto.ToCommand();
        var result =await mediator.Send(command);
        Uri location = new ($"/{result}");
        return Created(location, result);
    }

    [HttpGet()]
    [Authorize(Policy = "RequiresAdmin")]
    public async Task<ActionResult<List<DentalOfficeListDto>>> GetAllAsync([FromServices] CleanTeethDbContext context)
    {
        var result = await context.DentalOffices.Select(office => new DentalOfficeListDto
        {
            Id = office.Id,
            Name = office.Name
        }).ToListAsync();
        return result;
    }


}
