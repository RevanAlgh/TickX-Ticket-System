import { Ticket } from './Ticket';

export class Product {
  productId: number;
  name: string;

  constructor(productId: number, name: string, tickets: Ticket[]) {
    this.productId = productId;
    this.name = name;
  }
}
