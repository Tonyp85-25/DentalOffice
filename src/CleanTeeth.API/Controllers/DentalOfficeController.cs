using CleanTeeth.API.DTO.DentalOffices;
using CleanTeeth.API.Extensions;
using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Application.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanTeeth.API.Controllers;

[ApiController]
[Route("api/dentaloffices")]
public class DentalOfficeController : ControllerBase
{
    private readonly IMediator _mediator;

    public DentalOfficeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<DentalOfficeDetailDto>> GetAsync(Guid id)
    {
        var query = new GetDentalOfficeDetailQuery { Id = id };
        var result = await _mediator.Send(query);
        return result;
    }

    [HttpPost]
    [Authorize(Policy = "RequiresAdmin")]
    public async Task<IActionResult> PostAsync(CreateDentalOfficeDto createDentalOfficeDto)
    {
        var command = createDentalOfficeDto.ToCommand();
        var result =await _mediator.Send(command);
        var location = $"/{result}";
        return Created(location, result);
    }

    [HttpGet()]
    [Authorize(Policy = "RequiresAdmin")]
    public async Task<ActionResult<List<DentalOfficeListDto>>> GetAllAsync()
    {
        var query = new GetDentalOfficeListQuery();
        List<DentalOfficeListDto> result = await _mediator.Send(query);
        return result;
    }

    [HttpGet("search")]
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
