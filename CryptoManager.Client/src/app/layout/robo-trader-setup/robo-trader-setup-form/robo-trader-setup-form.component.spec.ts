import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoboTraderSetupFormComponent } from './robo-trader-setup-form.component';

describe('RoboTraderSetupFormComponent', () => {
  let component: RoboTraderSetupFormComponent;
  let fixture: ComponentFixture<RoboTraderSetupFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RoboTraderSetupFormComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RoboTraderSetupFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
