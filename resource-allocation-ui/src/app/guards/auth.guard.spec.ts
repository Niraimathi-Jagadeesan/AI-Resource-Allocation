import { TestBed } from '@angular/core/testing';
import { Router, ActivatedRouteSnapshot } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { AuthService } from '../services/auth.service';

describe('AuthGuard', () => {
  let guard: AuthGuard;
  let authService: jasmine.SpyObj<AuthService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(() => {
    authService = jasmine.createSpyObj('AuthService', [
      'isLoggedIn',
      'hasRole'
    ]);

    router = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        AuthGuard,
        { provide: AuthService, useValue: authService },
        { provide: Router, useValue: router }
      ]
    });

    guard = TestBed.inject(AuthGuard);
  });

  function createRoute(
    roles?: string[]
  ): ActivatedRouteSnapshot {
    return {
      data: roles ? { roles } : {}
    } as ActivatedRouteSnapshot;
  }

  it('should allow access when user is logged in', () => {
    authService.isLoggedIn.and.returnValue(true);

    const route = createRoute();
    const result = guard.canActivate(route);

    expect(result).toBeTrue();
    expect(router.navigate).not.toHaveBeenCalled();
  });

  it('should redirect to login when user is not logged in', () => {
    authService.isLoggedIn.and.returnValue(false);

    const route = createRoute();
    const result = guard.canActivate(route);

    expect(result).toBeFalse();
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should allow access when user has required role', () => {
    authService.isLoggedIn.and.returnValue(true);
    authService.hasRole.and.returnValue(true);

    const route = createRoute(['Admin']);
    const result = guard.canActivate(route);

    expect(result).toBeTrue();
  });

  it('should redirect to dashboard when user lacks required role', () => {
    authService.isLoggedIn.and.returnValue(true);
    authService.hasRole.and.returnValue(false);

    const route = createRoute(['Admin']);
    const result = guard.canActivate(route);

    expect(result).toBeFalse();
    expect(router.navigate).toHaveBeenCalledWith(['/dashboard']);
  });
});