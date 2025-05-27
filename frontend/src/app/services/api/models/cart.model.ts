import { CartItem } from './cart-item.model';

export interface Cart {
  id: string;
  totalPrice: number;
  cartItems: CartItem[];
}
