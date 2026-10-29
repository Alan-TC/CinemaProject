import { inject, Service } from "@angular/core";
import { environment } from "../../../environment/environment";
import { HttpClient } from "@angular/common/http";
import { IOrderRequest } from "../models/order-request.interface";
import { Observable } from "rxjs";
import { IOrder } from "../models/order.interface";

@Service()
export class OrdersService {
  private readonly _httpClient = inject(HttpClient);
  private readonly REQUEST_URL = `${environment.apiUrl}/orders`;

  makeOrder(orderRequest: IOrderRequest): Observable<IOrder> {
    return this._httpClient.post<IOrder>(this.REQUEST_URL, orderRequest);
  }

  getOrderById(orderId: string): Observable<IOrder> {
    return this._httpClient.get<IOrder>(`${this.REQUEST_URL}/${orderId}`);
  }

  payOrder(orderId: string): Observable<IOrder> {
    return this._httpClient.put<IOrder>(`${this.REQUEST_URL}/${orderId}/pay`, {});
  }
}
