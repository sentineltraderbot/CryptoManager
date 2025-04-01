import { Component, OnInit, Renderer2 } from "@angular/core";
import { HealthService } from "../shared";
import { HealthStatusType } from "../shared/models/health-status-type.enum";
import { Health } from "../shared/models/health.model";

@Component({
  selector: "app-health",
  templateUrl: "./health.component.html",
  styleUrls: ["./health.component.scss"],
})
export class HealthComponent implements OnInit {
  cryptoManagerhealth: Health = null;
  roboTraderhealth: Health = null;

  constructor(
    private healthService: HealthService,
    private renderer: Renderer2
  ) {}

  ngOnInit() {
    this.renderer.setStyle(document.body, "background-color", "white");

    this.healthService.getAllForCryptoManager().subscribe({
      next: (data) => (this.cryptoManagerhealth = data),
      error: (error) => {
        this.cryptoManagerhealth = this.createErrorResponse(
          this.cryptoManagerhealth,
          error
        );
      },
    });
    this.healthService.getAllForRoboTrader().subscribe({
      next: (data) => (this.roboTraderhealth = data),
      error: (error) => {
        this.roboTraderhealth = this.createErrorResponse(
          this.roboTraderhealth,
          error
        );
      },
    });
  }

  createErrorResponse(health: Health, error: any): Health {
    if (error?.error?.generalStatus) {
      health = error.error;
    } else {
      health = {
        checks: [
          {
            name: "ApplicationDown",
            description: "Application is down",
            errors: [], //[error],
            status: HealthStatusType.Unhealthy,
          },
        ],
        generalStatus: HealthStatusType.Unhealthy,
      };
    }
    return health;
  }

  getColorByStatus(status: HealthStatusType) {
    switch (status) {
      case HealthStatusType.Degraded:
        return "table-warning";
      case HealthStatusType.Unhealthy:
        return "table-danger";
      case HealthStatusType.Healthy:
        return "table-success";
    }
  }
}
