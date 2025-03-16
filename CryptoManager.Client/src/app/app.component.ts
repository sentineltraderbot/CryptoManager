import {
  AfterContentInit,
  ChangeDetectorRef,
  Component,
  OnInit,
} from "@angular/core";
import { Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { AccountService, HealthService, HTTPStatus } from "./shared";
import { SolanaWalletService } from "./shared/services/solana-wallet-service";
@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"],
})
export class AppComponent implements OnInit, AfterContentInit {
  showLoader: boolean = false;
  lastPingTime = 0;
  constructor(
    private accountService: AccountService,
    private healthService: HealthService,
    private httpStatus: HTTPStatus,
    private translate: TranslateService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private readonly solanaWalletService: SolanaWalletService
  ) {
    this.translate.addLangs([
      "en",
      "fr",
      "ur",
      "es",
      "it",
      "fa",
      "de",
      "zh-CHS",
      "pt",
    ]);
    this.translate.setDefaultLang("pt");
    const browserLang = this.translate.getBrowserLang();
    this.translate.use(
      browserLang.match(/en|fr|ur|es|it|fa|de|zh-CHS|pt/) ? browserLang : "pt"
    );
    this.solanaWalletService.setWalletAdapters();
  }

  ngAfterContentInit(): void {
    this.httpStatus.getHttpStatus().subscribe((status: boolean) => {
      this.showLoader = status;
      this.cdr.detectChanges();
    });
  }

  ngOnInit(): void {
    this.router.events.subscribe(() => {
      this.ping();
    });
  }

  ping() {
    const now = Date.now();
    if (now - this.lastPingTime > 1000) {
      this.lastPingTime = now;
      this.healthService.ping().subscribe({
        next: () => {
          this.accountService.populate();
        },
        error: () => {
          console.log("API is Offline");
          this.accountService.purgeAuth();
        },
      });
    }
  }
}
