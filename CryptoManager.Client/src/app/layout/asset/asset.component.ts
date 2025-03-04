import { Component, OnInit } from "@angular/core";
import { routerTransition } from "../../router.animations";
import { TranslateService } from "@ngx-translate/core";
import {
  Asset,
  AssetService,
  AccountService,
  UserToken,
  AlertType,
  AlertHandlerService,
} from "../../shared";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ConfirmationModalComponent } from "../../shared/confirmation-modal";
@Component({
  selector: "app-asset",
  templateUrl: "./asset.component.html",
  styleUrls: ["./asset.component.scss"],
  animations: [routerTransition()],
})
export class AssetComponent implements OnInit {
  assets: Asset[] = [];
  user: UserToken;

  constructor(
    private translate: TranslateService,
    private assetService: AssetService,
    private accountService: AccountService,
    private alertHandlerService: AlertHandlerService,
    private modalService: NgbModal
  ) {}

  ngOnInit() {
    this.user = this.accountService.getCurrentUser();
    this.assetService.getAll().subscribe({
      next: (data) => (this.assets = data),
      error: () =>
        this.alertHandlerService.createAlert(
          AlertType.Danger,
          this.translate.instant("CouldNotProcess")
        ),
    });
  }

  delete(asset: Asset) {
    const modalRef = this.modalService.open(ConfirmationModalComponent);
    modalRef.componentInstance.confirmationBoxTitle =
      this.translate.instant("Assets");
    modalRef.componentInstance.confirmationMessage =
      this.translate.instant("DeleteMessage");

    modalRef.result
      .then((userResponse) => {
        if (userResponse) {
          var index = this.assets.indexOf(asset);
          this.assets.splice(index, 1);
          this.assetService.delete(asset.id).subscribe({
            next: () => {
              this.alertHandlerService.createAlert(
                AlertType.Success,
                "Asset Deleted"
              );
            },
            error: () => {
              this.alertHandlerService.createAlert(
                AlertType.Danger,
                this.translate.instant("CouldNotProcess")
              );
              // Revert the view back to its original state
              this.assets.splice(index, 0, asset);
            },
          });
        }
      })
      .catch((error) => console.log(`User aborted: ${error}`));
  }
}
