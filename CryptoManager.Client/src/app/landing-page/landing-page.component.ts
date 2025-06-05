import { Component, OnInit, Renderer2 } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { LocalStorageKeyType } from "../shared";

@Component({
  selector: "app-landing-page",
  templateUrl: "./landing-page.component.html",
  styleUrl: "./landing-page.component.scss",
})
export class LandingPageComponent implements OnInit {
  constructor(private renderer: Renderer2, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.renderer.setStyle(document.body, "background-color", "black");
    this.route.queryParams.subscribe((params) => {
      const code = params[LocalStorageKeyType.ReferralCode];
      if (code) {
        localStorage.setItem(LocalStorageKeyType.ReferralCode, code);
      }
    });
  }

  readonly year = new Date().getFullYear();
  toggleMenu() {
    const nav = document.querySelector(".header .nav");
    if (nav) nav.classList.toggle("active");
  }
}
