import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

import { ApiService } from "./api.service";
import { Order, SetupTrader } from "../models/index";
import { ApiType } from "../models/api-type.enum";

@Injectable()
export class SetupTraderService {
  public serviceURL = "/setuptrader";

  constructor(private apiService: ApiService) {}

  getAllByLoggedUser(): Observable<SetupTrader[]> {
    return this.apiService.get(
      `${this.serviceURL}/GetByApplicationUser`,
      null,
      ApiType.RoboTraderApi
    );
  }

  get(id: string): Observable<SetupTrader> {
    return this.apiService.get(
      `${this.getUrl(id)}/GetByApplicationUser`,
      null,
      ApiType.RoboTraderApi
    );
  }

  add(setupTrader: SetupTrader): Observable<SetupTrader> {
    return this.apiService.post(
      this.serviceURL,
      setupTrader,
      null,
      ApiType.RoboTraderApi
    );
  }

  update(setupTrader: SetupTrader) {
    return this.apiService.put(
      this.serviceURL,
      setupTrader,
      ApiType.RoboTraderApi
    );
  }

  delete(id: string) {
    return this.apiService.delete(this.getUrl(id), ApiType.RoboTraderApi);
  }

  private getUrl(id: string) {
    return this.serviceURL + "/" + id;
  }
}
