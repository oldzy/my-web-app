import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { User } from './models/user.model';
import { AuthStateService } from '../auth-state.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private apiUrl = `${environment.baseUrl}/api/users`;

  constructor(private http: HttpClient, private authStateService: AuthStateService) { }
  
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl, { headers: this.authStateService.getAuthHeaders() });
  }
}
