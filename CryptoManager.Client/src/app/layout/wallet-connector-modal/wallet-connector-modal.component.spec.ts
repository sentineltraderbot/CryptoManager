import { ComponentFixture, TestBed } from "@angular/core/testing";

import { WalletConnectorModalComponent } from "./wallet-connector-modal.component";

describe("WalletConnectorModalComponent", () => {
  let component: WalletConnectorModalComponent;
  let fixture: ComponentFixture<WalletConnectorModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WalletConnectorModalComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(WalletConnectorModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
