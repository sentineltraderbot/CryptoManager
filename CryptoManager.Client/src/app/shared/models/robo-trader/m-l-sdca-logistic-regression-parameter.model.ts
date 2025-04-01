import { CrossingType } from "./crossing-type.enum";
import { IntervalType } from "./interval-type.enum";

export class MLSdcaLogisticRegressionParameter {
  id: string;
  setupTraderStartId: string;
  setupTraderStopId: string;
  interval: IntervalType;
  isBuy: boolean;
  isHolding: boolean;
}
