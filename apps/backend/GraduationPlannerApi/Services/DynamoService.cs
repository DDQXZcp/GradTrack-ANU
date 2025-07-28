using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using GraduationPlannerApi.Models;
using System.Text.Json;
using System.IO;

namespace GraduationPlannerApi.Services;

public class DynamoService
{
    private readonly IDynamoDBContext _context;

    public DynamoService(IAmazonDynamoDB dynamoDb)
    {
        var config = new DynamoDBContextConfig
        {
            Conversion = DynamoDBEntryConversion.V2
        };

        _context = new DynamoDBContext(dynamoDb, config);
    }

    public async Task<List<Course>> GetCoursesAsync()
    {
        var allCourses = await _context.ScanAsync<Course>(new List<ScanCondition>()).GetRemainingAsync();
        return allCourses;
    }

    public async Task<List<Course>> GetAllCoursesAsync()
    {
        var conditions = new List<ScanCondition>(); // No filters, fetch all
        var allCourses = await _context.ScanAsync<Course>(conditions).GetRemainingAsync();
        return allCourses;
    }

    public async Task<List<Requirement>> GetAllRequirementsAsync()
    {
        var conditions = new List<ScanCondition>(); // No filters
        var requirements = await _context.ScanAsync<Requirement>(conditions).GetRemainingAsync();
        return requirements;
    }

    public async Task<List<object>> GetFormattedRequirementsAsync()
    {
        var requirements = await GetAllRequirementsAsync();  // your existing method
        return requirements.Select(r => new {
            Requirement = r.Id,
            Title = r.Title,
            RequiredCourses = string.Join(" OR ", r.Options.Select(option =>
                $"{r.RequiredCredits} units from [{string.Join(", ", option)}]")),
            Credits = r.RequiredCredits
        }).ToList<object>();
    }

    public async Task<List<RequirementStatusDto>> CheckRequirementsAsync(List<string> selectedCourses)
    {
        var requirements = await GetAllRequirementsAsync();
        var allCourses = await GetAllCoursesAsync(); // 🔁 fetch all course data
        var courseCreditMap = allCourses.ToDictionary(c => c.CourseCode, c => c.Credit);

        var result = new List<RequirementStatusDto>();

        foreach (var req in requirements)
        {
            bool met = false;

            foreach (var group in req.Options)
            {
                int matchedCredits = group
                    .Where(selectedCourses.Contains)
                    .Sum(code => courseCreditMap.TryGetValue(code, out var credit) ? credit : 0); // 🧠 get actual credit

                if (matchedCredits >= req.RequiredCredits)
                {
                    met = true;
                    break;
                }
            }

            result.Add(new RequirementStatusDto
            {
                Requirement = req.Id,
                Met = met
            });
        }

        return result;
    }

    public async Task BulkUploadCoursesAsync(string filePath)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var courses = JsonSerializer.Deserialize<List<Course>>(json);

        if (courses == null) return;

        foreach (var course in courses)
        {
            await _context.SaveAsync(course);
        }
    }

    public async Task BulkUploadRequirementsAsync(string jsonFilePath)
    {
        var json = await File.ReadAllTextAsync(jsonFilePath);
        var requirements = JsonSerializer.Deserialize<List<Requirement>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (requirements == null) return;

        foreach (var requirement in requirements)
        {
            await _context.SaveAsync(requirement); // Use the existing context
        }
    }
}