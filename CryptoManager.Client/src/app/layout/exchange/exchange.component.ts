import { Component, OnInit } from "@angular/core";
import { routerTransition } from "../../router.animations";
import { TranslateService } from "@ngx-translate/core";
import {
  Exchange,
  ExchangeService,
  ExchangeType,
  AccountService,
  UserToken,
  AlertHandlerService,
  AlertType,
} from "../../shared";
import { ConfirmationModalComponent } from "../../shared/confirmation-modal";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
@Component({
  selector: "app-exchange",
  templateUrl: "./exchange.component.html",
  styleUrls: ["./exchange.component.scss"],
  animations: [routerTransition()],
})
export class ExchangeComponent implements OnInit {
  exchanges: Exchange[] = [];
  user: UserToken;

  constructor(
    private translate: TranslateService,
    private exchangeService: ExchangeService,
    private accountService: AccountService,
    private alertHandlerService: AlertHandlerService,
    private modalService: NgbModal
  ) {}

  ngOnInit() {
    this.user = this.accountService.getCurrentUser();
    this.exchangeService.getAll().subscribe({
      next: (data) => (this.exchanges = data),
      error: () =>
        this.alertHandlerService.createAlert(
          AlertType.Danger,
          this.translate.instant("CouldNotProcess")
        ),
    });
  }

  delete(exchange) {
    const modalRef = this.modalService.open(ConfirmationModalComponent);
    modalRef.componentInstance.confirmationBoxTitle =
      this.translate.instant("Exchanges");
    modalRef.componentInstance.confirmationMessage =
      this.translate.instant("DeleteMessage");

    modalRef.result
      .then((userResponse) => {
        if (userResponse) {
          var index = this.exchanges.indexOf(exchange);
          this.exchanges.splice(index, 1);
          this.exchangeService.delete(exchange.id).subscribe({
            next: () =>
              this.alertHandlerService.createAlert(
                AlertType.Success,
                "Exchange Deleted"
              ),
            error: () => {
              this.alertHandlerService.createAlert(
                AlertType.Danger,
                this.translate.instant("CouldNotProcess")
              );
              // Revert the view back to its original state
              this.exchanges.splice(index, 0, exchange);
            },
          });
        }
      })
      .catch((error) => console.log(`User aborted: ${error}`));
  }
  getExchangeType(exchangeType: ExchangeType) {
    return ExchangeType[exchangeType];
  }
}
