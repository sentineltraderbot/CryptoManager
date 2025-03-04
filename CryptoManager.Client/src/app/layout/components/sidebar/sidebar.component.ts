import { Component, OnInit } from "@angular/core";
import { Router, NavigationEnd } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { AccountService, UserDetails, UserToken } from "../../../shared";

@Component({
  selector: "app-sidebar",
  templateUrl: "./sidebar.component.html",
  styleUrls: ["./sidebar.component.scss"],
})
export class SidebarComponent {
  isActive: boolean = false;
  showMenu: string = "";
  pushRightClass: string = "push-right";
  userDetails: UserDetails;
  userToken: UserToken;
  constructor(
    private translate: TranslateService,
    public router: Router,
    private accountService: AccountService
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
    this.userToken = this.accountService.getCurrentUser();
    this.accountService.getUserDetails().subscribe((userData) => {
      this.userDetails = userData;
    });
  }

  eventCalled() {
    this.isActive = !this.isActive;
  }

  addExpandClass(element: any) {
    if (element === this.showMenu) {
      this.showMenu = "0";
    } else {
      this.showMenu = element;
    }
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

  changeLang(language: string) {
    this.translate.use(language);
  }

  onLoggedout() {
    this.accountService.purgeAuth();
  }
}
