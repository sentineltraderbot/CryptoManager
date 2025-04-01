import { Injectable } from "@angular/core";
import { Observable, BehaviorSubject, ReplaySubject } from "rxjs";

import { ApiService } from "./api.service";
import { Asset, ApiType } from "../models/index";

@Injectable()
export class AssetService {
  public serviceURL = "/asset";

  constructor(private apiService: ApiService) {}

  getAll(): Observable<Asset[]> {
    return this.apiService.get(
      this.serviceURL,
      null,
      ApiType.CryptoManagerServerApi
    );
  }

  get(id: string): Observable<Asset> {
    return this.apiService.get(
      this.getUrl(id),
      null,
      ApiType.CryptoManagerServerApi
    );
  }

  add(asset: Asset): Observable<Asset> {
    return this.apiService.post(
      this.serviceURL,
      asset,
      null,
      ApiType.CryptoManagerServerApi
    );
  }

  update(asset: Asset) {
    return this.apiService.put(
      this.serviceURL,
      asset,
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
