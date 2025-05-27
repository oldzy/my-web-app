import { Component, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { UsersService } from '../../services/api/users.service';
import { AuthStateService } from '../../services/auth-state.service';
import { Cart } from '../../services/api/models/cart.model';
import { CartItem } from '../../services/api/models/cart-item.model';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-cart-page',
  imports: [CommonModule, RouterLink],
  templateUrl: './cart-page.component.html',
  styleUrls: ['./cart-page.component.css']
})
export class CartPageComponent implements OnInit {
  cart: Cart | null = null;
  isLoading = false;
  error: string | null = null;

  constructor(
    private usersService: UsersService,
    private authStateService: AuthStateService
  ) { }

  ngOnInit(): void {
    this.fetchCart();
  }

  fetchCart(): void {
    this.isLoading = true;
    this.error = null;
    this.usersService.getMyCart().subscribe({
      next: (data) => {
        this.cart = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Failed to load cart. Please try again later.';
        if (err.status === 401) {
          this.error = 'Your session has expired. Please log in again.';
          this.authStateService.logout();
        }
        this.isLoading = false;
      }
    });
  }

  calculateItemTotal(item: CartItem): number {
    return item.unitPrice * item.quantity;
  }

  clearCart(): void {
    if (!this.cart || !this.cart.id) {
      alert('No cart to clear or cart ID is missing.');
      return;
    }

    const confirmClear = confirm('Are you sure you want to clear your cart?');
    if (!confirmClear) {
      return;
    }

    this.isLoading = true;
    this.usersService.clearMyCart().subscribe({
      next: () => {
        this.fetchCart();
      },
      error: (err) => {
        alert('Failed to clear cart. Please try again.');
        console.error('Clear cart error:', err);
        if (err.status === 401) {
          this.error = 'Your session has expired. Please log in again.';
          this.authStateService.logout();
        }
        this.isLoading = false;
      }
    });
  }
}
