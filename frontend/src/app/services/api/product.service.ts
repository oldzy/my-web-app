import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Product } from './models/product.model';
import { AuthStateService } from '../auth-state.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = `${environment.baseUrl}/api/products`;

  constructor(private http: HttpClient, private authStateService: AuthStateService) { }

  getAllProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl, { headers: this.authStateService.getAuthHeaders() });
  }

  addProduct(product: Product): Observable<any> {
    return this.http.post<any>(this.apiUrl, product, { headers: this.authStateService.getAuthHeaders() });
  }
}
