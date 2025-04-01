import { CrossingType } from "./crossing-type.enum";
import { IntervalType } from "./interval-type.enum";

export class RelativeStrengthIndexIndicatorParameter {
  id: string;
  setupTraderStartId: string;
  setupTraderStopId: string;
  interval: IntervalType;
  crossing: CrossingType;
  period: number;
  rsiThreshold: number;
  hasJustCrossed: boolean;
  isHolding: boolean;
}
