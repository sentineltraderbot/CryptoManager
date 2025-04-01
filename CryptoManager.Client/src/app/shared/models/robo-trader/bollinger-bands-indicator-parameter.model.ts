import { BollingerBandType } from "./bollinger-band-type.enum";
import { CrossingType } from "./crossing-type.enum";
import { IntervalType } from "./interval-type.enum";

export class BollingerBandsIndicatorParameter {
  id: string;
  setupTraderStartId: string;
  setupTraderStopId: string;
  interval: IntervalType;
  crossing: CrossingType;
  length: number;
  multiplier: number;
  crossingBand: BollingerBandType;
  hasJustCrossed: boolean;
  isHolding: boolean;
}
