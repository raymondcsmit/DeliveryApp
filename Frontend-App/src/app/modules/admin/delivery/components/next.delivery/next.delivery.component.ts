import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector     : 'next-delivery',
  standalone   : true,
  imports: [MatButtonModule,CommonModule],
  templateUrl: './next.delivery.component.html',
  styleUrls: ['./next.delivery.component.scss'],
  encapsulation: ViewEncapsulation.None,
})

export class nextDeliveryComponent implements OnInit {

  delivery = {
    deliveryNumber: '000004',
    deliveryDateTime: new Date('2023-12-12T12:00:00Z'), // Replace with actual data source
    deliveryAddress: 'Tower, Abu Dhabi Exhibition Centre (ADNEC)',
    courierName: 'Amir Al-Farooq',
    items: [
      { name: 'Beef with Beetroot', quantity: 1 },
      { name: 'Chicken with Banana', quantity: 1 },
      { name: 'Lamb with Apple', quantity: 1 }
    ],
    editableUntil: new Date('2023-12-10') // Replace with actual data source
  };

  constructor() { }

  ngOnInit(): void {
  }

}