using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GraduationPlannerApi.Models;
using GraduationPlannerApi.Services;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly DynamoService _dynamoService;

    public CourseController(DynamoService service)
    {
        _dynamoService = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _dynamoService.GetAllCoursesAsync();
        return Ok(courses);
    }

    [HttpGet("requirements")]
    public async Task<IActionResult> GetFormattedRequirements()
    {
        var formatted = await _dynamoService.GetFormattedRequirementsAsync();
        return Ok(formatted);
    }

    [HttpPost("requirements/check")]
    public async Task<IActionResult> CheckRequirements([FromBody] UserCoursesDto input)
    {
        var result = await _dynamoService.CheckRequirementsAsync(input.CourseCodes);
        return Ok(result);
    }
}