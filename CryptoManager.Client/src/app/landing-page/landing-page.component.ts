import { Component, OnInit, Renderer2 } from "@angular/core";

@Component({
  selector: "app-landing-page",
  templateUrl: "./landing-page.component.html",
  styleUrl: "./landing-page.component.scss",
})
export class LandingPageComponent implements OnInit {
  constructor(private renderer: Renderer2) {}
  ngOnInit(): void {
    this.renderer.setStyle(document.body, "background-color", "black");
  }
  readonly year = new Date().getFullYear();
  toggleMenu() {
    const nav = document.querySelector(".header .nav");
    if (nav) nav.classList.toggle("active");
  }
}
