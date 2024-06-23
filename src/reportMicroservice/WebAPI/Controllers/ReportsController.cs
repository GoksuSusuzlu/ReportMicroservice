using Application.Features.Reports.Commands.Create;
using Application.Features.Reports.Commands.Delete;
using Application.Features.Reports.Commands.Update;
using Application.Features.Reports.Queries.GetById;
using Application.Features.Reports.Queries.GetList;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using Application.Services.RabbitMQClientService;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : BaseController
{
    private readonly IRabbitMQClientService _rabbitMQClientService;

    public ReportsController(IRabbitMQClientService rabbitMQClientService)
    {
        _rabbitMQClientService = rabbitMQClientService;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateReportCommand createReportCommand)
    {
        CreatedReportResponse response = await Mediator.Send(createReportCommand);

        return Created(uri: "", response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateReportCommand updateReportCommand)
    {
        UpdatedReportResponse response = await Mediator.Send(updateReportCommand);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        DeletedReportResponse response = await Mediator.Send(new DeleteReportCommand { Id = id });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdReportResponse response = await Mediator.Send(new GetByIdReportQuery { Id = id });
        CounterDetailsDto counterDetails = await _rabbitMQClientService.GetCounterDetailsAsync(response.CounterId);
        response.CounterDetails = counterDetails;
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListReportQuery getListReportQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListReportListItemDto> response = await Mediator.Send(getListReportQuery);
        return Ok(response);
    }
}