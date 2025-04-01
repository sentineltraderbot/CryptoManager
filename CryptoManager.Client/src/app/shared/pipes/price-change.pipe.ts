import { Injectable, Pipe, PipeTransform } from "@angular/core";
import { TickerPrice } from "../models";

@Pipe({
  name: "priceChange",
})
@Injectable({
  providedIn: "root",
})
export class PriceChangePipe implements PipeTransform {
  transform(
    ticker: TickerPrice,
    previousPrices: { [symbol: string]: number }
  ): string {
    if (!previousPrices || !previousPrices[ticker.symbol]) {
      return "";
    }

    const oldPrice = previousPrices[ticker.symbol];
    previousPrices[ticker.symbol] = ticker.price;
    if (ticker.price > oldPrice) {
      return "price-up";
    } else if (ticker.price < oldPrice) {
      return "price-down";
    } else {
      return "price-neutral";
    }
  }
}
