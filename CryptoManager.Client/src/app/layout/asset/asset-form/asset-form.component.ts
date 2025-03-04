import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { routerTransition } from "../../../router.animations";
import { TranslateService } from "@ngx-translate/core";
import {
  AlertHandlerService,
  AlertType,
  Asset,
  AssetService,
} from "../../../shared";

@Component({
  selector: "app-asset-form  ",
  templateUrl: "./asset-form.component.html",
  styleUrls: ["./asset-form.component.scss"],
  animations: [routerTransition()],
})
export class AssetFormComponent implements OnInit {
  title: string;
  asset: Asset = new Asset();
  formState: string;
  constructor(
    private translate: TranslateService,
    private assetService: AssetService,
    private router: Router,
    private route: ActivatedRoute,
    private alertHandlerService: AlertHandlerService
  ) {}

  ngOnInit() {
    var id = this.route.params.subscribe((params) => {
      var id = params["id"];

      this.formState = id ? "Edit" : "Add";
      this.title = this.translate.instant(this.formState);

      if (!id) return;

      this.assetService.get(id).subscribe({
        next: (asset) => (this.asset = asset),
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
    let result;
    let operationMessage = "";
    if (this.asset.id) {
      result = this.assetService.update(this.asset);
      operationMessage = "Asset Updated";
    } else {
      result = this.assetService.add(this.asset);
      operationMessage = "Asset Added";
    }

    result.subscribe({
      next: () => {
        this.alertHandlerService.createAlert(
          AlertType.Success,
          operationMessage
        );
        this.router.navigate(["/sentinel-trader/asset"]);
      },
      error: () =>
        this.alertHandlerService.createAlert(
          AlertType.Danger,
          this.translate.instant("CouldNotProcess")
        ),
    });
  }
}
