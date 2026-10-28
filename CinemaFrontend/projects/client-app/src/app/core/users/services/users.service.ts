import { inject, Service } from '@angular/core';
import { environment } from '../../../environment/environment';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { IUserProfile } from '../models/user-profile.interface';

@Service()
export class UsersService {
  private readonly REQUEST_URL = `${environment.apiUrl}/users`;
  private readonly _httpClient = inject(HttpClient);

  register(name: string, email: string, password: string): Observable<null>{
    return this._httpClient.post<null>(
      this.REQUEST_URL, { name, email, password });
  }

  getProfile():
    Observable<IUserProfile>
  {
    return this._httpClient.get<IUserProfile>(`${this.REQUEST_URL}/profile`);
  }

  updateProfile(profile: IUserProfile):
    Observable<IUserProfile>
  {
    return this._httpClient.put<IUserProfile>(`${this.REQUEST_URL}/profile`, profile);
  }
}
