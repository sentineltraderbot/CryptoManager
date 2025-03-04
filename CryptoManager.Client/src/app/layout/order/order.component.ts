import { Component, OnInit } from "@angular/core";
import { routerTransition } from "../../router.animations";
import { TranslateService } from "@ngx-translate/core";
import { AlertHandlerService, AlertType, OrderService } from "../../shared";
import { OrderDetail } from "../../shared/models/orderDetail.model";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ConfirmationModalComponent } from "../../shared/confirmation-modal";

@Component({
  selector: "app-order",
  templateUrl: "./order.component.html",
  styleUrls: ["./order.component.scss"],
  animations: [routerTransition()],
})
export class OrderComponent implements OnInit {
  orders: OrderDetail[] = [];
  constructor(
    private translate: TranslateService,
    private orderService: OrderService,
    private alertHandlerService: AlertHandlerService,
    private modalService: NgbModal
  ) {}

  ngOnInit() {
    this.orderService
      .getAllByLoggedUser({ isBackTest: false, isViaRoboTrader: false })
      .subscribe({
        next: (data) => (this.orders = data),
        error: () =>
          this.alertHandlerService.createAlert(
            AlertType.Danger,
            this.translate.instant("CouldNotProcess")
          ),
      });
  }

  getProfitColor(profit: number) {
    if (profit >= 0) {
      return "green";
    }
    return "red";
  }

  delete(order) {
    const modalRef = this.modalService.open(ConfirmationModalComponent);
    modalRef.componentInstance.confirmationBoxTitle =
      this.translate.instant("My Investments");
    modalRef.componentInstance.confirmationMessage =
      this.translate.instant("DeleteMessage");

    modalRef.result
      .then((userResponse) => {
        if (userResponse) {
          var index = this.orders.indexOf(order);
          this.orders.splice(index, 1);
          this.orderService.delete(order.id).subscribe({
            next: () =>
              this.alertHandlerService.createAlert(
                AlertType.Success,
                "Order Deleted"
              ),
            error: () => {
              this.alertHandlerService.createAlert(
                AlertType.Danger,
                this.translate.instant("CouldNotProcess")
              );
              // Revert the view back to its original state
              this.orders.splice(index, 0, order);
            },
          });
        }
      })
      .catch((error) => console.log(`User aborted: ${error}`));
  }
}
