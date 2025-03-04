import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-cookie-consent",
  templateUrl: "./cookie-consent.component.html",
  styleUrl: "./cookie-consent.component.scss",
})
export class CookieConsentComponent implements OnInit {
  isConsentGiven: boolean = false;

  ngOnInit(): void {
    this.isConsentGiven = localStorage.getItem("cookieConsent") === "true";
  }

  acceptCookies(): void {
    localStorage.setItem("cookieConsent", "true");
    this.isConsentGiven = true;
  }
}
