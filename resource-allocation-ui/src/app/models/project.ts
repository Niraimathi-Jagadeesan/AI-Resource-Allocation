export interface Project {
  id: number;
  projectName: string;
  requiredSkills: string;
  minimumExperience: number;
  priority: number;
  complexity?: string;
  startDate?: string;
  endDate?: string;
}