import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { routerTransition } from "../../../router.animations";
import { TranslateService } from "@ngx-translate/core";
import {
  AlertHandlerService,
  AlertType,
  Exchange,
  ExchangeService,
} from "../../../shared";

@Component({
  selector: "app-exchange-form  ",
  templateUrl: "./exchange-form.component.html",
  styleUrls: ["./exchange-form.component.scss"],
  animations: [routerTransition()],
})
export class ExchangeFormComponent implements OnInit {
  title: string;
  exchange: Exchange = new Exchange();
  exchangesType = [
    {
      id: 0,
      name: "Binance",
    },
    {
      id: 1,
      name: "HitBTC",
    },
    {
      id: 2,
      name: "Coinbase",
    },
    {
      id: 3,
      name: "BitcoinTrade",
    },
    {
      id: 4,
      name: "KuCoin",
    },
  ];
  formState: string;
  constructor(
    private translate: TranslateService,
    private exchangeService: ExchangeService,
    private router: Router,
    private route: ActivatedRoute,
    private alertHandlerService: AlertHandlerService
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params) => {
      var id = params["id"];

      this.formState = id ? "Edit" : "Add";
      this.title = this.translate.instant(this.formState);
      if (!id) return;

      this.exchangeService.get(id).subscribe({
        next: (exchange) => (this.exchange = exchange),
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
    var result;
    let operationMessage = "";
    if (this.exchange.id) {
      result = this.exchangeService.update(this.exchange);
      operationMessage = "Exchange Updated";
    } else {
      result = this.exchangeService.add(this.exchange);
      operationMessage = "Exchange Added";
    }

    result.subscribe({
      next: () => {
        this.alertHandlerService.createAlert(
          AlertType.Success,
          operationMessage
        );
        this.router.navigate(["/sentinel-trader/exchange"]);
      },
      error: () =>
        this.alertHandlerService.createAlert(
          AlertType.Danger,
          this.translate.instant("CouldNotProcess")
        ),
    });
  }
}
