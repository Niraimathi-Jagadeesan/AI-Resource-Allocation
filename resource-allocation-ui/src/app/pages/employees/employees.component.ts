import { Component, OnInit } from '@angular/core';
import { Employee } from '../../models/employee';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html'
})
export class EmployeesComponent implements OnInit {
  employees: Employee[] = [];
  loading = false;

  showForm = false;
  isEditMode = false;

  model: Employee = this.getEmptyEmployee();

  constructor(
    private api: ApiService,
    public auth: AuthService
  ) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  getEmptyEmployee(): Employee {
    return {
      id: 0,
      name: '',
      skills: '',
      experience: 0,
      isAvailable: true,
      performanceScore: 0,
      currentAllocationPercent: 0,
      primaryRole: '',
      successRate: 0
    };
  }

  loadEmployees(): void {
    this.loading = true;

    this.api.get<Employee[]>('Employees')
      .subscribe({
        next: data => {
          this.employees = data;
          this.loading = false;
        },
        error: () => {
          this.loading = false;
        }
      });
  }

  addNew(): void {
    this.model = this.getEmptyEmployee();
    this.showForm = true;
    this.isEditMode = false;
  }

  edit(employee: Employee): void {
    this.model = { ...employee };
    this.showForm = true;
    this.isEditMode = true;
  }

  save(): void {
    if (this.isEditMode) {
      this.api.put(
        `Employees/${this.model.id}`,
        this.model
      ).subscribe(() => {
        this.afterSave();
      });
    } else {
      this.api.post<Employee>(
        'Employees',
        this.model
      ).subscribe(() => {
        this.afterSave();
      });
    }
  }

  delete(id: number): void {
    if (!confirm('Delete this employee?')) {
      return;
    }

    this.api.delete(
      `Employees/${id}`
    ).subscribe({
      next: () => this.loadEmployees(),
      error: () => alert('Failed to delete employee.')
    });
  }

  afterSave(): void {
    this.showForm = false;
    this.loadEmployees();
  }

  cancel(): void {
    this.showForm = false;
  }
}