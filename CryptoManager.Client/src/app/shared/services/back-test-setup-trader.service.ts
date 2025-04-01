import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

import { ApiService } from "./api.service";
import { ApiType } from "../models/index";
import { BackTestSetupTraderSearchCriteria } from "../models/robo-trader/back-test-setup-trader-search-criteria.model";
import { BackTestSetupTrader } from "../models/robo-trader/back-test-setup-trader.model";
import queryString from "query-string";
import { SimpleObjectResult } from "../models/simple-object-result.model";

@Injectable()
export class BackTestSetupTraderService {
  public serviceURL = "/BackTest";

  constructor(private apiService: ApiService) {}

  getByCriteria(
    criteria: BackTestSetupTraderSearchCriteria
  ): Observable<BackTestSetupTrader[]> {
    var query = queryString.stringify(criteria);
    return this.apiService.get(
      `${this.serviceURL}/GetByCriteria?${query}`,
      null,
      ApiType.RoboTraderApi
    );
  }

  continueFaulty(
    backTestSetupTraderId: string
  ): Observable<SimpleObjectResult> {
    return this.apiService.post(
      `${this.serviceURL}/continueFaulty?backTestSetupTraderId=${backTestSetupTraderId}`,
      null,
      null,
      ApiType.RoboTraderApi
    );
  }
}
