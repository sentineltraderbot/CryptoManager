import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

import { ApiService } from "./api.service";
import {
  ApiType,
  Order,
  OrderDetail,
  GetOrdersCriteria,
} from "../models/index";

@Injectable()
export class OrderService {
  public serviceURL = "/order";

  constructor(private apiService: ApiService) {}

  add(order: Order): Observable<Order> {
    return this.apiService.post(
      this.serviceURL,
      order,
      null,
      ApiType.CryptoManagerServerApi
    );
  }

  getAllByLoggedUser(
    getOrdersCriteria: GetOrdersCriteria
  ): Observable<OrderDetail[]> {
    var queryString = new URLSearchParams(getOrdersCriteria as {}).toString();
    return this.apiService.get(
      `${this.serviceURL}/GetOrderDetailsByApplicationUser?${queryString}`,
      null,
      ApiType.CryptoManagerServerApi
    );
  }

  delete(id: string) {
    return this.apiService.delete(
      this.getUrl(id),
      ApiType.CryptoManagerServerApi
    );
  }

  private getUrl(id) {
    return this.serviceURL + "/" + id;
  }
}
