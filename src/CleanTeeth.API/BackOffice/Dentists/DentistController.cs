using CleanTeeth.Application.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanTeeth.API.BackOffice.Dentists;

[ApiController]
[Authorize(Policy = "RequiresAdmin")]
[Route("api/admin/dentists")]
public class DentistController : ControllerBase
{
    private readonly IMediator _mediator;

    public DentistController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateDentistDto dto)
    {
        var command = dto.ToCommand();
        var result = await _mediator.Send(command);
        Uri location = new($"/{result}");
        return Created(location, result);
    }
}
