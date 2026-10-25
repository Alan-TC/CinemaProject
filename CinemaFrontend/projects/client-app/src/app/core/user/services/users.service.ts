import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../../environment/environment';
import { Observable } from 'rxjs';
import { IUserProfile } from '../models/user-profile.interface';

@Service()
export class UsersService {
  private readonly _httpClient = inject(HttpClient);
  private readonly REQUEST_URL = `${environment.apiUrl}/users`;

  getProfile():
    Observable<IUserProfile>
  {
    return this._httpClient.get<IUserProfile>(`${this.REQUEST_URL}/profile`);
  }
}
