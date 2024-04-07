import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { DeliveryService } from 'app/core/delivery/delivery.service';
import { DeliveryViewModel } from 'app/core/delivery/delivery.types';
import { ResponseResult } from 'app/core/respons.types';
import { ListDeliveryComponent } from 'app/modules/admin/delivery/components/list.delivery/list.delivery.component';
import { nextDeliveryComponent } from 'app/modules/admin/delivery/components/next.delivery/next.delivery.component';
import { upcomingDeliveryComponent } from 'app/modules/admin/delivery/components/upcoming.delivery/upcoming.delivery.component';
import { Observable } from 'rxjs';

@Component({
    selector: 'main',
    standalone: true,
    templateUrl: './main.component.html',
    encapsulation: ViewEncapsulation.None,
    imports: [nextDeliveryComponent, upcomingDeliveryComponent, ListDeliveryComponent]
})
export class MainComponent implements OnInit {
    constructor(private deliveryService: DeliveryService) {
    }

    upcomingDeliveries: DeliveryViewModel[];
    pastDeliveries: DeliveryViewModel[];
    ngOnInit(): void {
        this.getUpComing();
        this.getPast();
    }

    getUpComing() {
        this.deliveryService.getUpComing().subscribe(x => {
            this.upcomingDeliveries = x.data;
        });
    }

    getPast() {
        this.deliveryService.getPast().subscribe(x => {
            this.pastDeliveries = x.data;
        });
    }
}
