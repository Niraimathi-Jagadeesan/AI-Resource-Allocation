export interface Employee {
  id: number;
  name: string;
  skills: string;
  experience: number;
  isAvailable: boolean;
  performanceScore: number;
  currentAllocationPercent: number;
  primaryRole?: string;
  successRate: number;
}