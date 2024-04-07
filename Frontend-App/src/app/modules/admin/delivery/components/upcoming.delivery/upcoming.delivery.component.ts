import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { DeliveryDetails, DeliveryViewModel } from 'app/core/delivery/delivery.types';
import {MatTableModule} from '@angular/material/table';
 
@Component({
  selector     : 'upcoming-delivery',
  standalone   : true,
  imports: [MatButtonModule,CommonModule, MatTableModule],
  templateUrl: './upcoming.delivery.component.html',
  styleUrls: ['./upcoming.delivery.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class upcomingDeliveryComponent  implements OnInit {
  @Input() deliveries: DeliveryViewModel[] = []; 
  displayedColumns: string[] = ['deliveryNumber', 'deliveryDate', 'deliveryAddress', 'items']; 
  constructor() { }

  ngOnInit(): void { 
  } 
}
 