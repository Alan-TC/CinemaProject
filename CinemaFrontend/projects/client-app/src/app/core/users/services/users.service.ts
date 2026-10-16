import { inject, Service } from '@angular/core';
import { environment } from '../../../environment/environment';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Service()
export class UsersService {
  private readonly REQUEST_URL = `${environment.apiUrl}/users`;
  private readonly _httpClient = inject(HttpClient);

  register(name: string, email: string, password: string): Observable<null>{
    return this._httpClient.post<null>(
      this.REQUEST_URL, { name, email, password });
  }
}
