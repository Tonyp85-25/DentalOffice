using CleanTeeth.API.Extensions;
using CleanTeeth.Application.Features;
using CleanTeeth.Application.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanTeeth.API.PatientApp.DentalOffices;

[ApiController]
[Route("api/dentaloffices")]
public class DentalOfficeController : ControllerBase
{
    private readonly IMediator _mediator;

    public DentalOfficeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<DentalOfficeListDto>>> GetAllByAsync([FromQuery] SearchDentalOfficeDto dto)
    {
        if (dto.IsEmpty())
        {
            return BadRequest();
        }

        var query = dto.ToQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
