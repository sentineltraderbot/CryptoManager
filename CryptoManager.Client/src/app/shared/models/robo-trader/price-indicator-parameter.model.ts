import { CrossingType } from "./crossing-type.enum";
import { IntervalType } from "./interval-type.enum";

export class PriceIndicatorParameter {
  id: string;
  setupTraderStartId: string;
  setupTraderStopId: string;
  interval: IntervalType;
  crossing: CrossingType;
  percentageChange: number | null;
  amountChange: number | null;
  priceTarget: number | null;
  trailingStopPercentage: number | null;
  isHolding: boolean;
}
