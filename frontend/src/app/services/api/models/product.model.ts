export interface Product {
  id: string;
  name: string;
  description?: string | null;
  price: number;
  stock: number;
  image?: string | null;
}
