import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CruiseSquad } from './cruise-squad';

describe('CruiseSquad', () => {
  let component: CruiseSquad;
  let fixture: ComponentFixture<CruiseSquad>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CruiseSquad]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CruiseSquad);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
