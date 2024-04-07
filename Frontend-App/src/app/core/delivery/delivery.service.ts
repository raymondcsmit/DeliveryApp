import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { User } from 'app/core/user/user.types';
import { map, Observable, ReplaySubject, tap } from 'rxjs';
import { ResponseResult } from '../respons.types';
import { DeliveryViewModel } from './delivery.types';
import { API_Config } from '../api.config';
import { getMockUpCommingDeliveres } from './delivery.mockdata';

@Injectable({
    providedIn: 'root',
  })
  export class DeliveryService {
    private apiUrl = 'https://localhost:44331/api/Deliveries'; 
  
    constructor(private http: HttpClient) {}
  
    getUpComing(): Observable<ResponseResult<DeliveryViewModel[]>> {
      return this.http.get<ResponseResult<DeliveryViewModel[]>>(`${API_Config.BaseAddress}/api/deliveries/GetUpcoming`)
      .pipe(
        map(x => {
          if(!x.data){
            x = getMockUpCommingDeliveres()
          }
          return x;
        })
      );
    }

    getPast(): Observable<ResponseResult<DeliveryViewModel[]>> {
      return this.http.get<ResponseResult<DeliveryViewModel[]>>(`${API_Config.BaseAddress}/api/deliveries/GetUpcoming`)
      .pipe(
        map(x => {
          if(!x.data){
            x = getMockUpCommingDeliveres()
          }
          return x;
        })
      );
    }
    getUpComingList(deliveryId: number): Observable<ResponseResult<DeliveryViewModel[]>> {
      return this.http.get<ResponseResult<DeliveryViewModel[]>>(`${this.apiUrl}/GetUpcoming/${deliveryId}`);
    }
  }