import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/api/product.service';
import { Product } from '../../services/api/models/product.model';
import { AuthStateService } from '../../services/auth-state.service';
import { UsersService } from '../../services/api/users.service';
import { CartItemRequest } from '../../services/api/models/cart-item-request.model';

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
    private authStateService: AuthStateService,
    private usersService: UsersService
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

  addToCart(product: Product): void {
    const quantityStr = prompt(`Specify the number of "${product.name}" to add to the cart:`, "1");
    if (quantityStr === null) {
      return;
    }

    const quantity = parseInt(quantityStr, 10);

    if (isNaN(quantity) || quantity <= 0) {
      alert("Invalid quantity. Please enter a positive number.");
      return;
    }

    if (quantity > product.stock) {
      alert(`Cannot add ${quantity} items. Only ${product.stock} available.`);
      return;
    }

    const cartItem: CartItemRequest = {
      productId: product.id,
      quantity: quantity,
      price: product.price
    };

    this.isLoading = true;
    this.usersService.addOrUpdateCartItems([cartItem]).subscribe({
      next: () => {
        this.fetchProducts();
      },
      error: (err) => {
        alert('Failed to add item to cart. Please try again.');
        if (err.status === 401) {
          this.error = 'Your session has expired. Please log in again.';
          this.authStateService.logout();
        }
        this.isLoading = false;
      }
    });
  }
}
