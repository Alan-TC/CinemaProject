import { inject, Service } from '@angular/core';
import { environment } from '../../../environment/environment';
import { BehaviorSubject, catchError, filter, map, Observable, of, take, tap, throwError } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ITokens } from '../models/tokens.interface';
import { jwtDecode } from 'jwt-decode';

@Service()
export class AuthService {
  private readonly ACCESS_TOKEN_KEY = 'access_token';
  private readonly REFRESH_TOKEN_KEY = 'refresh_token';
  private readonly REQUEST_URL = `${environment.apiUrl}/authentication`;

  private readonly _httpClient = inject(HttpClient);

  private _isRefreshing = false;
  private _refreshTokenSubject = new BehaviorSubject<ITokens | null | 'error' >(null);


  login(email: string, password: string): Observable<ITokens> {
    return this._httpClient.post<ITokens>(
      this.REQUEST_URL + '/login', { email, password })
      .pipe(
        map((response) => {
          localStorage.setItem(this.ACCESS_TOKEN_KEY, response.accessToken);
          localStorage.setItem(this.REFRESH_TOKEN_KEY, response.refreshToken);
          return response;
        })
      );
    }

  clear_tokens(): void {
    localStorage.removeItem(this.ACCESS_TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
  }

  refreshToken(): Observable<ITokens | 'error'> {
    if (!this._isRefreshing) {
      this._isRefreshing = true;
      this._refreshTokenSubject.next(null);

      return this._refreshToken().pipe(
        tap((response) => {
          this._isRefreshing = false;
          this._refreshTokenSubject.next(response);
          return response;
        }),
        catchError((error) => {
          this._isRefreshing = false;
          this.clear_tokens();
          this._refreshTokenSubject.next('error');
          return throwError(() => error);
        })
      );
    }
    else {
      return this._refreshTokenSubject.pipe(
        filter(token => token !== null),
        take(1),
        map(token => token)
      );
    }
  }

  private _refreshToken(): Observable<ITokens> {
    const refreshToken = localStorage.getItem(this.REFRESH_TOKEN_KEY);

    if (!refreshToken) {
      return throwError(() => ({
        status: 401,
        message: 'Refresh token not found'
      }));
    }

    return this._httpClient.post<ITokens>(
      this.REQUEST_URL + '/refresh', { refreshToken }
    ).pipe(
      tap((response) => {
        localStorage.setItem(this.ACCESS_TOKEN_KEY, response.accessToken);
        localStorage.setItem(this.REFRESH_TOKEN_KEY, response.refreshToken);
      })
    );
  }

  get tokenSubject(): BehaviorSubject<ITokens | null | 'error'> {
    return this._refreshTokenSubject;
  }

  validateTokens(): Observable<boolean> {
    const accessToken = localStorage.getItem(this.ACCESS_TOKEN_KEY);
    if (!accessToken) {
      return of(false);
    }
    if (!this.isTokenExpired(accessToken)) {
      return of(true);
    }

    const refreshToken = localStorage.getItem(this.REFRESH_TOKEN_KEY);
    if (!refreshToken || this.isTokenExpired(refreshToken)) {
      return of(false);
    }

    return this.refreshToken().pipe(
      map((token) => {
        if (token === 'error') {
          return false;
        }
        return true;
      }),
      catchError(() => of(false))
    )
  }

  getRefreshUrlRelativePath(): string {
    return 'authentication' + '/refresh';
  }

  getAccessToken(): string | null {
    return localStorage.getItem(this.ACCESS_TOKEN_KEY);
  }

  isTokenExpired(token: string): boolean {
    try {
      const { exp } = jwtDecode<{ exp?: number }>(token);

      return !exp || exp < Date.now() / 1000;
    } catch {
      return true;
    }
  }
}
