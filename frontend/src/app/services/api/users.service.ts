import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { User } from './models/user.model';
import { AuthStateService } from '../auth-state.service';
import { Cart } from './models/cart.model';
import { CartItemRequest } from './models/cart-item-request.model';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private apiUrl = `${environment.baseUrl}/api/users`;

  constructor(private http: HttpClient, private authStateService: AuthStateService) { }
  
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl, { headers: this.authStateService.getAuthHeaders() });
  }
  
  getMyCart(): Observable<Cart> {
    return this.http.get<Cart>(`${this.apiUrl}/me/cart`, { headers: this.authStateService.getAuthHeaders() });
  }
  
  addOrUpdateCartItems(items: CartItemRequest[]): Observable<Cart> {
    return this.http.post<Cart>(`${this.apiUrl}/me/cart/items`, items, { headers: this.authStateService.getAuthHeaders() });
  }

  clearMyCart(): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/me/cart/items`, { headers: this.authStateService.getAuthHeaders() });
  }
}
