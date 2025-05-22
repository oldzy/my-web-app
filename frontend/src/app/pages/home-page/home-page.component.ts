import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/api/product.service';
import { Product } from '../../services/api/models/product.model';
import { AuthStateService } from '../../services/auth-state.service';

@Component({
  selector: 'app-home-page',
  imports: [CommonModule],
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {
  products: Product[] = [];
  isLoading = false;
  error: string | null = null;

  constructor(
    private productService: ProductService,
    private authStateService: AuthStateService
  ) { }

  ngOnInit(): void {
    this.fetchProducts();
  }

  fetchProducts(): void {
    this.isLoading = true;
    this.error = null;
    this.productService.getAllProducts().subscribe({
      next: (data) => {
        this.products = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Failed to load products. Please try again later.';
        if (err.status === 401) {
          this.error = 'Your session has expired. Please log in again.';
          this.authStateService.logout();
        }
        this.isLoading = false;
      }
    });
  }
}
