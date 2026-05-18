import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { Project } from '../../models/project';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html'
})
export class ProjectsComponent implements OnInit {
  projects: Project[] = [];
  loading = false;

  showForm = false;
  isEditMode = false;

  model: Project = this.getEmptyProject();

  constructor(
    private api: ApiService,
    public auth: AuthService
  ) {}

  ngOnInit(): void {
    this.loadProjects();
  }

  getEmptyProject(): Project {
    return {
      id: 0,
      projectName: '',
      requiredSkills: '',
      minimumExperience: 0,
      priority: 1,
      complexity: 'Medium',
      startDate: undefined,
      endDate: undefined
    };
  }

  loadProjects(): void {
    this.loading = true;

    this.api.get<Project[]>('Projects')
      .subscribe({
        next: data => {
          this.projects = data;
          this.loading = false;
        },
        error: () => {
          this.loading = false;
        }
      });
  }

  addNew(): void {
    this.model = this.getEmptyProject();
    this.showForm = true;
    this.isEditMode = false;
  }

  edit(project: Project): void {
    this.model = { ...project };
    this.showForm = true;
    this.isEditMode = true;
  }

  save(): void {
    if (this.isEditMode) {
      this.api.put(
        `Projects/${this.model.id}`,
        this.model
      ).subscribe(() => this.afterSave());
    } else {
      this.api.post<Project>(
        'Projects',
        this.model
      ).subscribe(() => this.afterSave());
    }
  }

  delete(id: number): void {
    if (!confirm('Delete this project?')) {
      return;
    }

    this.api.delete(
      `Projects/${id}`
    ).subscribe(() => this.loadProjects());
  }

  afterSave(): void {
    this.showForm = false;
    this.loadProjects();
  }

  cancel(): void {
    this.showForm = false;
  }
}