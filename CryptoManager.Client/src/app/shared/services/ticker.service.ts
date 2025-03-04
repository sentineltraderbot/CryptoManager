import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

import { ApiService } from "./api.service";
import { ApiType, ExchangeType, ObjectResult, Order } from "../models/index";
import { TickerPrice } from "../models/ticker-price.model";

@Injectable()
export class TickerService {
  public serviceURL = "/ticker";

  constructor(private apiService: ApiService) {}

  getAllPrices(exchangeType: ExchangeType): Observable<TickerPrice[]> {
    var queryString = new URLSearchParams({ exchangeType } as {}).toString();
    return this.apiService.get(
      `${this.serviceURL}?${queryString}`,
      null,
      ApiType.CryptoManagerServerApi
    );
  }

  getCurrentPrice(
    baseAssetSymbol: string,
    quoteAssetSymbol: string,
    exchangeType: ExchangeType
  ): Observable<ObjectResult<TickerPrice>> {
    var queryString = new URLSearchParams({
      baseAssetSymbol,
      quoteAssetSymbol,
      exchangeType,
    } as {}).toString();
    return this.apiService.get(
      `${this.serviceURL}/GetCurrentPrice?${queryString}`,
      null,
      ApiType.CryptoManagerServerApi
    );
  }
}
