public class UserCoursesDto
{
    public required List<string> CourseCodes { get; set; }
}

public class RequirementStatusDto
{
    public required string Requirement { get; set; }
    public required bool Met { get; set; }
}