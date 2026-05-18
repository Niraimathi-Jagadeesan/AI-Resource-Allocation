namespace ResourceAllocation.API.Helpers
{
    public static class SkillHelper
    {
        /// <summary>
        /// Normalizes a skill string:
        /// - trims spaces
        /// - converts to lowercase
        /// - maps synonyms to standard names
        /// </summary>
        public static string NormalizeSkill(string skill)
        {
            if (string.IsNullOrWhiteSpace(skill))
                return string.Empty;

            skill = skill.Trim().ToLower();

            return skill switch
            {
                "asp.net core" => ".net",
                "dotnet" => ".net",
                "angular 18" => "angular",
                "angularjs" => "angular",
                _ => skill
            };
        }

        /// <summary>
        /// Converts comma-separated skills into normalized list.
        /// Example: ".NET, Angular" => [".net", "angular"]
        /// </summary>
        public static List<string> SplitAndNormalize(string skills)
        {
            if (string.IsNullOrWhiteSpace(skills))
                return new List<string>();

            return skills
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(NormalizeSkill)
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Returns percentage of required skills matched by employee.
        /// </summary>
        public static double CalculateSkillMatchPercentage(
            string employeeSkills,
            string requiredSkills)
        {
            var employeeSkillSet = SplitAndNormalize(employeeSkills);
            var requiredSkillSet = SplitAndNormalize(requiredSkills);

            if (!requiredSkillSet.Any())
                return 0;

            var matchedCount = requiredSkillSet
                .Count(skill => employeeSkillSet.Contains(skill));

            return (double)matchedCount / requiredSkillSet.Count * 100;
        }
    }
}