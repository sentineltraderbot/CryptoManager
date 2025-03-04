import { ExchangeType } from "./exchange-type.enum";

export class Exchange {
  id: string;
  name: string;
  website: string;
  apiUrl: string;
  isEnabled: boolean;
  registryDate: Date;
  exchangeType: ExchangeType;
}
