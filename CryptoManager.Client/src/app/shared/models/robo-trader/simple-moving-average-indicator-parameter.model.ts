import { CrossingType } from "./crossing-type.enum";
import { IntervalType } from "./interval-type.enum";

export class SimpleMovingAverageIndicatorParameter {
  id: string;
  setupTraderStartId: string;
  setupTraderStopId: string;
  interval: IntervalType;
  crossing: CrossingType;
  includeCurrentPrice: boolean;
  period1: number;
  period2: number;
  hasJustCrossed: boolean;
  isHolding: boolean;
}
