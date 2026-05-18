import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Project } from '../../models/project';
import { RiskResult } from '../../models/risk';

@Component({
  selector: 'app-risk',
  templateUrl: './risk.component.html'
})
export class RiskComponent implements OnInit {
  projects: Project[] = [];
  selectedProjectId = 0;
  result: RiskResult | null = null;
  loading = false;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.api.get<Project[]>('Projects')
      .subscribe(data => {
        this.projects = data;
      });
  }

  analyzeRisk(): void {
    if (!this.selectedProjectId) {
      alert('Please select a project.');
      return;
    }

    this.loading = true;
    this.result = null;

    this.api
      .get<RiskResult>(
        `Risk/${this.selectedProjectId}`
      )
      .subscribe({
        next: data => {
          this.result = data;
          this.loading = false;
        },
        error: () => {
          this.loading = false;
          alert('Failed to analyze risk.');
        }
      });
  }

  get selectedProject(): Project | undefined {
    return this.projects.find(p => p.id === this.selectedProjectId);
  }

  getRiskClass(): string {
    if (!this.result) {
      return 'bg-secondary';
    }

    switch (
      this.result.riskLevel.toLowerCase()
    ) {
      case 'low':
        return 'bg-success';
      case 'medium':
        return 'bg-warning text-dark';
      case 'high':
        return 'bg-danger';
      default:
        return 'bg-secondary';
    }
  }
}