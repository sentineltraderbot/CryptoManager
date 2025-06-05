import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApiService } from "./api.service";
import { ApiType, ObjectResult, TickerBalance } from "../models";

@Injectable()
export class SentinelTraderBotTokenService {
  public serviceURL = "/SentinelTraderBotToken";

  constructor(private apiService: ApiService) {}

  getBalancesBySolanaWallet(
    solanaWalletAddress: string
  ): Observable<ObjectResult<TickerBalance[]>> {
    var queryString = new URLSearchParams({ solanaWalletAddress }).toString();
    return this.apiService.get(
      `${this.serviceURL}/balances?${queryString}`,
      null,
      ApiType.CryptoManagerServerApi
    );
  }
}
