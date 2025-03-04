import { Component, Input, Output, EventEmitter } from "@angular/core";
import {
  BollingerBandType,
  CrossingType,
  IndicatorFieldType,
  IndicatorType,
  IntervalType,
} from "../../../shared";

@Component({
  selector: "app-indicator",
  templateUrl: "./indicator.component.html",
})
export class IndicatorComponent {
  IndicatorFieldType = IndicatorFieldType;
  @Input() indicators: any[] = [];
  @Input() title: string;
  @Input() indicatorType: IndicatorType;
  @Output() remove = new EventEmitter<number>();

  intervalTypes: IntervalType[] = Object.values(IntervalType);
  crossingTypes: CrossingType[] = Object.values(CrossingType);
  BollingerBandTypes: BollingerBandType[] = Object.values(BollingerBandType);
  fieldsToShowByIndicatorType: {
    [key in IndicatorType]: IndicatorFieldType[];
  } = {
    [IndicatorType.SimpleMovingAverage]: [
      IndicatorFieldType.Interval,
      IndicatorFieldType.Crossing,
      IndicatorFieldType.IncludeCurrentPrice,
      IndicatorFieldType.Period1,
      IndicatorFieldType.Period2,
      IndicatorFieldType.HasJustCrossed,
      IndicatorFieldType.IsHolding,
    ],
    [IndicatorType.ExponentialMovingAverage]: [
      IndicatorFieldType.Interval,
      IndicatorFieldType.Crossing,
      IndicatorFieldType.IncludeCurrentPrice,
      IndicatorFieldType.Period1,
      IndicatorFieldType.Period2,
      IndicatorFieldType.HasJustCrossed,
      IndicatorFieldType.IsHolding,
    ],
    [IndicatorType.RelativeStrengthIndex]: [
      IndicatorFieldType.Interval,
      IndicatorFieldType.Crossing,
      IndicatorFieldType.Period,
      IndicatorFieldType.RSIThreshold,
      IndicatorFieldType.HasJustCrossed,
      IndicatorFieldType.IsHolding,
    ],
    [IndicatorType.Price]: [
      IndicatorFieldType.Crossing,
      IndicatorFieldType.PercentageChange,
      IndicatorFieldType.AmountChange,
      IndicatorFieldType.PriceTarget,
      IndicatorFieldType.TrailingStopPercentage,
      IndicatorFieldType.IsHolding,
    ],
    [IndicatorType.MLSdcaLogisticRegression]: [
      IndicatorFieldType.Interval,
      IndicatorFieldType.IsBuy,
      IndicatorFieldType.IsHolding,
    ],
    [IndicatorType.BollingerBands]: [
      IndicatorFieldType.Interval,
      IndicatorFieldType.Crossing,
      IndicatorFieldType.Length,
      IndicatorFieldType.Multiplier,
      IndicatorFieldType.CrossingBand,
      IndicatorFieldType.HasJustCrossed,
      IndicatorFieldType.IsHolding,
    ],
  };

  removeIndicator(index: number) {
    this.remove.emit(index);
  }

  isVisible(indicatorFieldType: IndicatorFieldType): boolean {
    return this.fieldsToShowByIndicatorType[this.indicatorType].includes(
      indicatorFieldType
    );
  }
}
