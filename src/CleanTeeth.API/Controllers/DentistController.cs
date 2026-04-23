using CleanTeeth.API.DTO.Dentists;
using CleanTeeth.API.Extensions;
using CleanTeeth.Application.Features.Dentists.Command.CreateDentist;
using CleanTeeth.Application.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CleanTeeth.API.Controllers;

[ApiController]
[Route("api/dentists")]
public class DentistController:ControllerBase
{
    private readonly IMediator _mediator;

    public DentistController(IMediator mediator)
    {
        _mediator = mediator;
    }

    async Task<IActionResult> Create(CreateDentistDTO dto)
    {
        var command = dto.ToCommand() ;
        await _mediator.Send(command);
        return Ok();
    }
}