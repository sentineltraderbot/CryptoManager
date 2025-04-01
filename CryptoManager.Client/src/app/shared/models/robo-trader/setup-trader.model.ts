import { ExchangeType } from "../exchange-type.enum";
import { AssetToTrade } from "./asset-to-trade.model";
import { BollingerBandsIndicatorParameter } from "./bollinger-bands-indicator-parameter.model";
import { ExponentialMovingAverageIndicatorParameter } from "./exponential-moving-average-indicator-parameter.model";
import { MLSdcaLogisticRegressionParameter } from "./m-l-sdca-logistic-regression-parameter.model";
import { PriceIndicatorParameter } from "./price-indicator-parameter.model";
import { RelativeStrengthIndexIndicatorParameter } from "./relative-strength-index-indicator-parameter.model";
import { RoboTraderAsset } from "./robo-trader-asset.model";
import { SimpleMovingAverageIndicatorParameter } from "./simple-moving-average-indicator-parameter.model";
import { TradingStatusType } from "./trading-status-type.enum";
import { TradingStrategyType } from "./trading-strategy-type.enum";

export class SetupTrader {
  id: string;
  exchange: ExchangeType;
  assetsToTrade: AssetToTrade[];

  quoteAssetId: string | null;
  quoteAsset: RoboTraderAsset;
  amountToTrade: number;
  minimumAmountToTrade: number | null;

  emaIndicatorsToStart: ExponentialMovingAverageIndicatorParameter[];
  rsiIndicatorsToStart: RelativeStrengthIndexIndicatorParameter[];
  smaIndicatorsToStart: SimpleMovingAverageIndicatorParameter[];
  priceIndicatorsToStart: PriceIndicatorParameter[];
  mlSdcaLogisticRegressionsToStart: MLSdcaLogisticRegressionParameter[];
  bollingerBandsIndicatorsToStart: BollingerBandsIndicatorParameter[];

  tradingStrategy: TradingStrategyType;
  tradingStatus: TradingStatusType;

  isRepeatable: boolean;
  isTestMode: boolean;
  isBackTestMode: boolean;
  startExpression: string;
  stopExpression: string;
  startHoldingExpression: string;
  stopHoldingExpression: string;
  finishReason: string | null;
  strategyName: string;

  emaIndicatorsToStop: ExponentialMovingAverageIndicatorParameter[];
  rsiIndicatorsToStop: RelativeStrengthIndexIndicatorParameter[];
  smaIndicatorsToStop: SimpleMovingAverageIndicatorParameter[];
  priceIndicatorsToStop: PriceIndicatorParameter[];
  mlSdcaLogisticRegressionsToStop?: MLSdcaLogisticRegressionParameter[];
  bollingerBandsIndicatorsToStop?: BollingerBandsIndicatorParameter[];
}
