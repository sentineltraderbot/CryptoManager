// clipboard.service.ts
import { Injectable } from "@angular/core";
import { AlertHandlerService } from "./alert-handler.service";
import { AlertType } from "../models";

@Injectable({ providedIn: "root" })
export class ClipboardService {
  constructor(private alertService: AlertHandlerService) {}

  copyToClipboard(
    text: string,
    successMessage = "Copied to clipboard!"
  ): boolean {
    try {
      // Modern Clipboard API (most browsers)
      navigator.clipboard.writeText(text).then(() => {
        this.showNotification(successMessage);
      });
      return true;
    } catch (err) {
      // Fallback for older browsers
      const textarea = document.createElement("textarea");
      textarea.value = text;
      document.body.appendChild(textarea);
      textarea.select();
      const success = document.execCommand("copy");
      document.body.removeChild(textarea);

      if (success) {
        this.showNotification(successMessage);
      } else {
        this.showNotification("Failed to copy!", true);
      }
      return success;
    }
  }

  private showNotification(message: string, isError = false): void {
    this.alertService.createAlert(
      isError ? AlertType.Danger : AlertType.Info,
      message
    );
  }
}
