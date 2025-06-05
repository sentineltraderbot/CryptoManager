import { Component, OnInit } from "@angular/core";
import { Router, NavigationEnd } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { AccountService, ClipboardService, UserDetails } from "../../../shared";

@Component({
  selector: "app-header",
  templateUrl: "./header.component.html",
  styleUrls: ["./header.component.scss"],
})
export class HeaderComponent implements OnInit {
  pushRightClass: string = "push-right";
  userDetails: UserDetails;

  constructor(
    private translate: TranslateService,
    public router: Router,
    private accountService: AccountService,
    private clipboardService: ClipboardService
  ) {
    this.router.events.subscribe((val) => {
      if (
        val instanceof NavigationEnd &&
        window.innerWidth <= 992 &&
        this.isToggled()
      ) {
        this.toggleSidebar();
      }
    });
  }

  ngOnInit() {
    this.getUserDetails();
  }

  onReferralClick() {
    const referralLink = `https://sentineltraderbot.com/?referralCode=${this.userDetails.id}`;
    this.clipboardService.copyToClipboard(referralLink);
  }

  getUserDetails(shouldGet: boolean = true) {
    if (!shouldGet) return;
    this.accountService.getUserDetails().subscribe((userData) => {
      this.userDetails = userData;
    });
  }

  isToggled(): boolean {
    const dom: Element = document.querySelector("body");
    return dom.classList.contains(this.pushRightClass);
  }

  toggleSidebar() {
    const dom: any = document.querySelector("body");
    dom.classList.toggle(this.pushRightClass);
  }

  rltAndLtr() {
    const dom: any = document.querySelector("body");
    dom.classList.toggle("rtl");
  }

  onLoggedout() {
    this.accountService.purgeAuth();
  }

  changeLang(language: string) {
    this.translate.use(language);
  }
}
