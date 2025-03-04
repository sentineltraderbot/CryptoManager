import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SolanaWalletConnectorModalComponent } from './solana-wallet-connector-modal.component';

describe('SolanaWalletConnectorModalComponent', () => {
  let component: SolanaWalletConnectorModalComponent;
  let fixture: ComponentFixture<SolanaWalletConnectorModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SolanaWalletConnectorModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SolanaWalletConnectorModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
