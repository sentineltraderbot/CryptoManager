import { Component, OnInit, OnDestroy, ChangeDetectorRef } from "@angular/core";
import { routerTransition } from "../../router.animations";
import { TranslateService } from "@ngx-translate/core";
import {
  AlertHandlerService,
  AlertType,
  ExchangeType,
  TickerPrice,
  TickerService,
} from "../../shared";
import { interval, Subscription } from "rxjs";
import { switchMap, take } from "rxjs/operators";

@Component({
  selector: "app-dashboard",
  templateUrl: "./dashboard.component.html",
  styleUrls: ["./dashboard.component.scss"],
  animations: [routerTransition()],
})
export class DashboardComponent implements OnInit, OnDestroy {
  tickers: TickerPrice[] = [];
  previousPrices: { [symbol: string]: number } = {};
  refreshInterval = 3000; // Updates every 3 seconds
  autoUpdate = false;
  subscription!: Subscription;

  constructor(
    private translate: TranslateService,
    private tickerService: TickerService,
    private alertHandlerService: AlertHandlerService,
    private cdRef: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.tickerService
      .getAllPrices(ExchangeType.Binance)
      .pipe(take(1))
      .subscribe({
        next: (data) => {
          this.tickers = data.filter((ticker) => ticker.price > 0);
          this.updatePreviousPrices(this.tickers);
          this.cdRef.detectChanges();
        },
        error: () =>
          this.alertHandlerService.createAlert(
            AlertType.Danger,
            this.translate.instant("CouldNotProcess")
          ),
      });
  }

  startAutoUpdate() {
    if (this.autoUpdate) {
      this.subscription = interval(this.refreshInterval)
        .pipe(
          switchMap(() => this.tickerService.getAllPrices(ExchangeType.Binance))
        )
        .subscribe({
          next: (data) => {
            this.tickers = data.filter((ticker) => ticker.price > 0);
            this.updatePreviousPrices(this.tickers);
            this.cdRef.detectChanges();
          },
          error: () =>
            this.alertHandlerService.createAlert(
              AlertType.Danger,
              this.translate.instant("CouldNotProcess")
            ),
        });
    }
  }

  stopAutoUpdate() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  toggleAutoUpdate() {
    this.stopAutoUpdate();
    if (this.autoUpdate) {
      this.startAutoUpdate();
    }
  }

  updatePreviousPrices(newTickers: TickerPrice[]) {
    newTickers.forEach((ticker) => {
      if (this.previousPrices[ticker.symbol] === undefined) {
        this.previousPrices[ticker.symbol] = ticker.price;
      }
    });
  }

  ngOnDestroy() {
    this.stopAutoUpdate();
  }
}
