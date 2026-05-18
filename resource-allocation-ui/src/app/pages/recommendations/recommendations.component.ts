import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Project } from '../../models/project';
import { Recommendation } from '../../models/recommendation';

@Component({
  selector: 'app-recommendations',
  templateUrl: './recommendations.component.html'
})
export class RecommendationsComponent implements OnInit {
  projects: Project[] = [];
  selectedProjectId = 0;
  recommendations: Recommendation[] = [];
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

  getRecommendations(): void {
    if (!this.selectedProjectId) {
      alert('Please select a project.');
      return;
    }

    this.loading = true;

    this.api
      .get<Recommendation[]>(
        `Recommendation/${this.selectedProjectId}`
      )
      .subscribe({
        next: data => {
          this.recommendations = data;
          this.loading = false;
        },
        error: () => {
          this.loading = false;
          alert('Failed to get recommendations.');
        }
      });
  }
}