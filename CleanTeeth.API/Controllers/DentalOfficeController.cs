using CleanTeeth.API.DTO.DentalOffices;
using CleanTeeth.API.Extensions;
using CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Application.Utilities;
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
    public async Task<ActionResult<DentalOfficeDetailDTO>> Get(Guid id)
    {
        var query = new GetDentalOfficeDetailQuery { Id = id };
        var result = await _mediator.Send(query);
        return result;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateDentalOfficeDTO createDentalOfficeDto)
    {
        var command = createDentalOfficeDto.ToCommand();
        var result =await _mediator.Send(command);
        var location = $"/{result}";
        return Created(location, result);
    }

    [HttpGet]
    public async Task<ActionResult<List<DentalOfficeListDTO>>> GetAll()
    {
        var query = new GetDentalOfficeListQuery();
        List<DentalOfficeListDTO> result = await _mediator.Send(query);
        return result;
    }
    
}