import { Component, OnInit } from "@angular/core";
import { routerTransition } from "../../router.animations";
import { TranslateService } from "@ngx-translate/core";
import {
  AlertType,
  AlertHandlerService,
  SetupTraderService,
  SetupTrader,
} from "../../shared";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ConfirmationModalComponent } from "../../shared/confirmation-modal";

@Component({
  selector: "robo-trader-setup",
  templateUrl: "./robo-trader-setup.component.html",
  styleUrls: ["./robo-trader-setup.component.scss"],
  animations: [routerTransition()],
})
export class RoboTraderSetupComponent implements OnInit {
  setupTraders: SetupTrader[] = [];

  constructor(
    private translate: TranslateService,
    private setupTraderService: SetupTraderService,
    private alertHandlerService: AlertHandlerService,
    private modalService: NgbModal
  ) {}

  ngOnInit() {
    this.setupTraderService.getAllByLoggedUser().subscribe({
      next: (data: SetupTrader[]) => (this.setupTraders = data),
      error: () =>
        this.alertHandlerService.createAlert(
          AlertType.Danger,
          this.translate.instant("CouldNotProcess")
        ),
    });
  }

  delete(setupTrader: SetupTrader) {
    const modalRef = this.modalService.open(ConfirmationModalComponent);
    modalRef.componentInstance.confirmationBoxTitle =
      this.translate.instant("Setup Traders");
    modalRef.componentInstance.confirmationMessage =
      this.translate.instant("DeleteMessage");

    modalRef.result
      .then((userResponse: boolean) => {
        if (userResponse) {
          const index = this.setupTraders.indexOf(setupTrader);
          this.setupTraders.splice(index, 1);

          this.setupTraderService.delete(setupTrader.id).subscribe({
            next: () => {
              this.alertHandlerService.createAlert(
                AlertType.Success,
                "Setup Trader Deleted"
              );
            },
            error: () => {
              this.alertHandlerService.createAlert(
                AlertType.Danger,
                this.translate.instant("CouldNotProcess")
              );
              this.setupTraders.splice(index, 0, setupTrader); // Reverte a remoção em caso de erro
            },
          });
        }
      })
      .catch((error) => console.log(`User aborted: ${error}`));
  }
}
