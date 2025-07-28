using Microsoft.AspNetCore.Mvc;
using GraduationPlannerApi.Services;

namespace GraduationPlannerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly DynamoService _dynamoService;

    public UploadController(DynamoService dynamoService)
    {
        _dynamoService = dynamoService;
    }

    [HttpPost("courses")]
    public async Task<IActionResult> UploadCourses()
    {
        try
        {
            await _dynamoService.BulkUploadCoursesAsync("semester_courses.json");
            return Ok("Courses uploaded successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to upload courses: {ex.Message}");
        }
    }

    [HttpPost("requirements")]
    public async Task<IActionResult> UploadRequirements()
    {
        try
        {
            await _dynamoService.BulkUploadRequirementsAsync("requirements.json");
            return Ok("Requirements uploaded successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to upload requirements: {ex.Message}");
        }
    }
}