using ExpenseTracker.Api.DTOs.Reports;
using ExpenseTracker.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/reports")]
public sealed class ReportsController(ReportService service) : ControllerBase
{
    [HttpGet("summary")]
    public async Task<ActionResult<ReportSummaryResponse>> GetSummary(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        CancellationToken cancellationToken)
        => Ok(await service.GetSummaryAsync(from, to, cancellationToken));

    [HttpGet("monthly")]
    public async Task<ActionResult<IReadOnlyList<MonthlyReportResponse>>> GetMonthly(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        CancellationToken cancellationToken)
    {
        var report = await service.GetSummaryAsync(from, to, cancellationToken);
        return Ok(report.Monthly);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<CategoryReportResponse>>> GetCategories(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        CancellationToken cancellationToken)
    {
        var report = await service.GetSummaryAsync(from, to, cancellationToken);
        return Ok(report.Categories);
    }
}
