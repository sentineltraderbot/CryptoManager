import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

import { ApiService } from "./api.service";
import { ApiType } from "../models/index";
import queryString from "query-string";
import { GetCandlesForChartParams } from "../models/robo-trader/get-candles-for-chart-params.model";
import { CandleStick } from "../models/robo-trader/candle-stick.model";
import { ObjectResult } from "../models/object-result.model";

@Injectable()
export class ChartService {
  public serviceURL = "/chart";

  constructor(private apiService: ApiService) {}

  getCandles(
    criteria: GetCandlesForChartParams
  ): Observable<ObjectResult<CandleStick[]>> {
    var query = queryString.stringify(criteria);
    return this.apiService.get(
      `${this.serviceURL}/getCandles?${query}`,
      null,
      ApiType.RoboTraderApi
    );
  }
}
