export interface Recommendation {
  employeeId: number;
  employeeName: string;
  finalScore: number;
  currentAllocationPercent: number;
  reason: string;
  skillsMatched?: string[];
}