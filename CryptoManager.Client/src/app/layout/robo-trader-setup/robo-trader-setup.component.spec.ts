import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoboTraderSetupComponent } from './robo-trader-setup.component';

describe('RoboTraderSetupComponent', () => {
  let component: RoboTraderSetupComponent;
  let fixture: ComponentFixture<RoboTraderSetupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RoboTraderSetupComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RoboTraderSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
