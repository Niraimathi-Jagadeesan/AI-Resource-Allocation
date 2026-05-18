import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent {
  username = '';
  role = '';

  constructor(public auth: AuthService) {
    this.username = this.auth.getUsername();
    this.role = this.auth.getRole();
  }

  logout(): void {
    this.auth.logout();
  }
}