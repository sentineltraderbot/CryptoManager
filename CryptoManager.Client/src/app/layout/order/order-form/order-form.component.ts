import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { routerTransition } from "../../../router.animations";
import { TranslateService } from "@ngx-translate/core";
import {
  Exchange,
  Order,
  ExchangeService,
  AssetService,
  Asset,
  OrderItem,
  OrderService,
  AlertHandlerService,
  AlertType,
} from "../../../shared";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ConfirmationModalComponent } from "../../../shared/confirmation-modal";

@Component({
  selector: "app-order-form",
  templateUrl: "./order-form.component.html",
  styleUrls: ["./order-form.component.scss"],
  animations: [routerTransition()],
})
export class OrderFormComponent implements OnInit {
  order: Order = new Order();
  orderItem: OrderItem = new OrderItem();
  exchanges: Exchange[] = [];
  assets: Asset[] = [];
  orderItems: OrderItem[] = [];
  select: any;
  orderGroup: FormGroup;
  orderItemGroup: FormGroup;

  constructor(
    private translate: TranslateService,
    private exchangeService: ExchangeService,
    private assetService: AssetService,
    private orderService: OrderService,
    private router: Router,
    private frmBuilder: FormBuilder,
    private alertHandlerService: AlertHandlerService,
    private modalService: NgbModal
  ) {
    this.orderGroup = this.frmBuilder.group({
      date: ["", [Validators.required]],
      exchange: ["", [Validators.required]],
      baseAsset: ["", [Validators.required]],
      quoteAsset: ["", [Validators.required]],
    });

    this.orderItemGroup = this.frmBuilder.group({
      price: ["", [Validators.required]],
      quantity: ["", [Validators.required]],
      fee: ["", [Validators.required]],
      feeAsset: ["", [Validators.required]],
    });
  }

  ngOnInit() {
    this.exchangeService
      .getAll()
      .subscribe((items) => (this.exchanges = items));
    this.assetService.getAll().subscribe((items) => (this.assets = items));
  }

  get date() {
    return this.orderGroup.get("date");
  }
  get exchange() {
    return this.orderGroup.get("exchange");
  }
  get baseAsset() {
    return this.orderGroup.get("baseAsset");
  }
  get quoteAsset() {
    return this.orderGroup.get("quoteAsset");
  }

  get price() {
    return this.orderItemGroup.get("price");
  }
  get quantity() {
    return this.orderItemGroup.get("quantity");
  }
  get fee() {
    return this.orderItemGroup.get("fee");
  }
  get feeAsset() {
    return this.orderItemGroup.get("feeAsset");
  }

  addItem() {
    console.log(this.orderItem);
    this.orderItems.splice(
      this.orderItems.length,
      0,
      Object.assign({}, this.orderItem)
    );
    this.orderItem = new OrderItem();
    this.select = null;
  }

  deleteItem(orderItem) {
    const modalRef = this.modalService.open(ConfirmationModalComponent);
    modalRef.componentInstance.confirmationBoxTitle =
      this.translate.instant("Items");
    modalRef.componentInstance.confirmationMessage =
      this.translate.instant("DeleteMessage");

    modalRef.result
      .then((userResponse) => {
        if (userResponse) {
          var index = this.orderItems.indexOf(orderItem);
          this.orderItems.splice(index, 1);
        }
      })
      .catch((error) => console.log(`User aborted: ${error}`));
  }

  changeFeeAsset(select: Asset) {
    this.orderItem.feeAssetId = select.id;
    this.orderItem.feeAssetName = select.name;
  }

  save() {
    this.order.orderItems = this.orderItems;
    let result = this.orderService.add(this.order);

    result.subscribe({
      next: (_) => {
        this.alertHandlerService.createAlert(
          AlertType.Success,
          "Order Created"
        );
        this.router.navigate(["/sentinel-trader/order"]);
      },
      error: () =>
        this.alertHandlerService.createAlert(
          AlertType.Danger,
          this.translate.instant("CouldNotProcess")
        ),
    });
  }
}
