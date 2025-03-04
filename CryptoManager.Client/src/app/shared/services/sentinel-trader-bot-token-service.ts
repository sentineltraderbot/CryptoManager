import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApiService } from "./api.service";
import {
  ApiType,
  ObjectResult,
  SimpleObjectResult,
  TickerBalance,
} from "../models";

@Injectable()
export class SentinelTraderBotTokenService {
  public serviceURL = "/SentinelTraderBotToken";

  constructor(private apiService: ApiService) {}

  airdrop(solanaWalletAddress: string): Observable<SimpleObjectResult> {
    var queryString = new URLSearchParams({ solanaWalletAddress }).toString();
    return this.apiService.post(
      `${this.serviceURL}/airdrop?${queryString}`,
      null,
      null,
      ApiType.CryptoManagerServerApi
    );
  }

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
