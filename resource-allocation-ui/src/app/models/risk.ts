export interface RiskResult {
  riskScore: number;
  riskLevel: string;
  summary: string;
  riskFactors: string[];
  recommendations: string[];
}