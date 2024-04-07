/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { Delivery.containerComponent } from './delivery.container.component';

describe('Delivery.containerComponent', () => {
  let component: Delivery.containerComponent;
  let fixture: ComponentFixture<Delivery.containerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Delivery.containerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Delivery.containerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
