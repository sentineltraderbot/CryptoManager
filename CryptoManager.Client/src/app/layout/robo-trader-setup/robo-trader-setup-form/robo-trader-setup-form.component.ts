import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { routerTransition } from "../../../router.animations";
import { TranslateService } from "@ngx-translate/core";
import {
  AlertHandlerService,
  AlertType,
  Asset,
  AssetService,
  AssetToTrade,
  BollingerBandsIndicatorParameter,
  Exchange,
  ExchangeService,
  ExponentialMovingAverageIndicatorParameter,
  IndicatorType,
  MLSdcaLogisticRegressionParameter,
  PriceIndicatorParameter,
  RelativeStrengthIndexIndicatorParameter,
  SetupTrader,
  SetupTraderService,
  SimpleMovingAverageIndicatorParameter,
  TradingStrategyType,
} from "../../../shared";

@Component({
  selector: "app-robo-trader-setup-form",
  templateUrl: "./robo-trader-setup-form.component.html",
  styleUrl: "./robo-trader-setup-form.component.scss",
  animations: [routerTransition()],
})
export class RoboTraderSetupFormComponent implements OnInit {
  indicatorType = IndicatorType;
  title: string;
  setupTrader: SetupTrader = {} as SetupTrader;
  formState: string;
  exchanges: Exchange[] = [];
  tradingStrategyTypes: TradingStrategyType[] =
    Object.values(TradingStrategyType);
  assets: Asset[] = [];
  selectedAssetToTradeId: string;
  indicatorTypes: IndicatorType[] = Object.values(IndicatorType);
  selectedIndicatorType: IndicatorType;

  constructor(
    private translate: TranslateService,
    private setupTraderService: SetupTraderService,
    private router: Router,
    private route: ActivatedRoute,
    private alertHandlerService: AlertHandlerService,
    private exchangeService: ExchangeService,
    private assetService: AssetService
  ) {}

  ngOnInit() {
    this.assetService.getAll().subscribe((items) => (this.assets = items));
    this.exchangeService
      .getAll()
      .subscribe((items) => (this.exchanges = items));

    this.route.params.subscribe((params) => {
      const id = params["id"];

      this.formState = id ? "Edit" : "Add";
      this.title = this.translate.instant(this.formState);

      if (!id) return;

      this.setupTraderService.get(id).subscribe({
        next: (setupTrader) => (this.setupTrader = setupTrader),
        error: (error) => {
          if (error.status == 404) {
            this.router.navigate(["NotFound"]);
          } else {
            this.alertHandlerService.createAlert(
              AlertType.Danger,
              this.translate.instant("CouldNotProcess")
            );
          }
        },
      });
    });
  }

  save() {
    let result;
    let operationMessage = "";
    if (this.setupTrader.id) {
      result = this.setupTraderService.update(this.setupTrader);
      operationMessage = "Setup Trader Updated";
    } else {
      result = this.setupTraderService.add(this.setupTrader);
      operationMessage = "Setup Trader Added";
    }

    result.subscribe(
      () => {
        this.alertHandlerService.createAlert(
          AlertType.Success,
          operationMessage
        );
        this.router.navigate(["/sentinel-trader/robo-trader-setup"]);
      },
      () =>
        this.alertHandlerService.createAlert(
          AlertType.Danger,
          this.translate.instant("CouldNotProcess")
        )
    );
  }

  addIndicatorToStart() {
    switch (this.selectedIndicatorType) {
      case IndicatorType.SimpleMovingAverage:
        if (!this.setupTrader.smaIndicatorsToStart) {
          this.setupTrader.smaIndicatorsToStart = [];
        }
        this.setupTrader.smaIndicatorsToStart?.push(
          {} as SimpleMovingAverageIndicatorParameter
        );
        break;
      case IndicatorType.ExponentialMovingAverage:
        if (!this.setupTrader.emaIndicatorsToStart) {
          this.setupTrader.emaIndicatorsToStart = [];
        }
        this.setupTrader.emaIndicatorsToStart?.push(
          {} as ExponentialMovingAverageIndicatorParameter
        );
        break;
      case IndicatorType.RelativeStrengthIndex:
        if (!this.setupTrader.rsiIndicatorsToStart) {
          this.setupTrader.rsiIndicatorsToStart = [];
        }
        this.setupTrader.rsiIndicatorsToStart?.push(
          {} as RelativeStrengthIndexIndicatorParameter
        );
        break;
      case IndicatorType.Price:
        if (!this.setupTrader.priceIndicatorsToStart) {
          this.setupTrader.priceIndicatorsToStart = [];
        }
        this.setupTrader.priceIndicatorsToStart?.push(
          {} as PriceIndicatorParameter
        );
        break;
      case IndicatorType.BollingerBands:
        if (!this.setupTrader.bollingerBandsIndicatorsToStart) {
          this.setupTrader.bollingerBandsIndicatorsToStart = [];
        }
        this.setupTrader.bollingerBandsIndicatorsToStart?.push(
          {} as BollingerBandsIndicatorParameter
        );
        break;
      case IndicatorType.MLSdcaLogisticRegression:
        if (!this.setupTrader.mlSdcaLogisticRegressionsToStart) {
          this.setupTrader.mlSdcaLogisticRegressionsToStart = [];
        }
        this.setupTrader.mlSdcaLogisticRegressionsToStart?.push(
          {} as MLSdcaLogisticRegressionParameter
        );
        break;
      default:
        break;
    }
  }

  removeIndicatorToStart(indicatorType: IndicatorType, index: number) {
    switch (indicatorType) {
      case IndicatorType.SimpleMovingAverage:
        this.setupTrader.smaIndicatorsToStart?.splice(index, 1);
        break;
      case IndicatorType.ExponentialMovingAverage:
        this.setupTrader.emaIndicatorsToStart?.splice(index, 1);
        break;
      case IndicatorType.RelativeStrengthIndex:
        this.setupTrader.rsiIndicatorsToStart?.splice(index, 1);
        break;
      case IndicatorType.Price:
        this.setupTrader.priceIndicatorsToStart?.splice(index, 1);
        break;
      case IndicatorType.BollingerBands:
        this.setupTrader.bollingerBandsIndicatorsToStart?.splice(index, 1);
        break;
      case IndicatorType.MLSdcaLogisticRegression:
        this.setupTrader.mlSdcaLogisticRegressionsToStart?.splice(index, 1);
        break;
      default:
        break;
    }
  }

  addAssetToTrade() {
    if (!this.setupTrader.assetsToTrade) {
      this.setupTrader.assetsToTrade = [];
    }
    let maxPriority = 0;
    if (this.setupTrader.assetsToTrade.length > 0) {
      maxPriority = this.setupTrader.assetsToTrade.reduce(
        (oa, a) => Math.max(oa, a.priorityOrder),
        0
      );
    }
    const assetToTrade = this.assets.find(
      (a) => a.id === this.selectedAssetToTradeId
    );
    if (!assetToTrade) {
      return;
    }
    this.setupTrader.assetsToTrade?.push({
      asset: {
        externalId: assetToTrade.id,
        symbol: assetToTrade.symbol,
      },
      priorityOrder: maxPriority + 1,
    });
  }

  removeAssetToTrade(assetId: string) {
    const index = this.setupTrader.assetsToTrade.findIndex(
      (a) => a.asset.externalId === assetId
    );
    if (index > -1) {
      this.setupTrader.assetsToTrade.splice(index, 1);
      for (let i = 0; i < this.setupTrader.assetsToTrade.length; i++) {
        this.setupTrader.assetsToTrade[i].priorityOrder = i + 1;
      }
    }
  }

  trackByAssetId(index: number, assetToTrade: AssetToTrade): string {
    return assetToTrade.asset.externalId;
  }

  onQuoteAssetChange(asset: Asset) {
    if (!asset) {
      this.setupTrader.quoteAsset = null;
      return;
    }
    this.setupTrader.quoteAsset = {
      externalId: asset.id,
      symbol: asset.symbol,
    };
  }
}
