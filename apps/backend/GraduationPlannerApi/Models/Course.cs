using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace GraduationPlannerApi.Models;

[DynamoDBTable("Courses")]
public class Course
{
    [DynamoDBHashKey("courseCode")]
    public required string CourseCode { get; set; }
    public required string Name { get; set; }
    public required string Availability { get; set; } // "S1 Only", "S2 Only", etc.
    public List<string>? Prerequisites { get; set; }
    public required bool IsLevel8 { get; set; }
    public required int Credit { get; set; } // 6 or 12
}

[DynamoDBTable("GraduationRequirements")]
public class Requirement
{
    [DynamoDBHashKey("requirementId")] // Required for DynamoDB table
    public required string Id { get; set; } = default!; // e.g., "compulsory", "foundation", "project"
    public required string Title { get; set; } = default!;
    public required int RequiredCredits { get; set; }
    // Each inner list is a group of course codes — completing any full group satisfies one option
    public required List<List<string>> Options { get; set; } = new();
}
