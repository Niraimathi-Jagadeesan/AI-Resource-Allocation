import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { Component } from '@angular/core';
import { AuthService } from './auth.service';

@Component({ template: '' })
class DummyComponent {}

describe('AuthService', () => {
  let service: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        RouterTestingModule.withRoutes([
          { path: 'login', component: DummyComponent },
          { path: 'dashboard', component: DummyComponent }
        ])
      ],
      declarations: [DummyComponent]
    });

    service = TestBed.inject(AuthService);
    localStorage.clear();
  });

  afterEach(() => {
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return null when token does not exist', () => {
    expect(service.getToken()).toBe('');
  });

  it('should return token from localStorage', () => {
    localStorage.setItem('token', 'test-token');
    expect(service.getToken()).toBe('test-token');
  });

  it('should detect logged in user', () => {
    localStorage.setItem('token', 'test-token');
    expect(service.isLoggedIn()).toBeTrue();
  });

  it('should detect logged out user', () => {
    expect(service.isLoggedIn()).toBeFalse();
  });

  it('should return role from localStorage', () => {
    localStorage.setItem('role', 'Admin');
    expect(service.getRole()).toBe('Admin');
  });

  it('should verify matching role', () => {
    localStorage.setItem('role', 'Admin');
    expect(service.hasRole('Admin')).toBeTrue();
  });

  it('should return false for non-matching role', () => {
    localStorage.setItem('role', 'Employee');
    expect(service.hasRole('Admin')).toBeFalse();
  });

  it('should clear localStorage on logout', () => {
    localStorage.setItem('token', 'test-token');
    localStorage.setItem('role', 'Admin');

    service.logout();

    expect(localStorage.getItem('token')).toBeNull();
    expect(localStorage.getItem('role')).toBeNull();
  });
});