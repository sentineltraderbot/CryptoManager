import { Component, OnInit, Renderer2 } from "@angular/core";

@Component({
  selector: "app-white-paper",
  templateUrl: "./white-paper.component.html",
  styleUrl: "./white-paper.component.scss",
})
export class WhitePaperComponent implements OnInit {
  constructor(private renderer: Renderer2) {}
  ngOnInit(): void {
    this.renderer.setStyle(document.body, "background-color", "black");
  }
}
