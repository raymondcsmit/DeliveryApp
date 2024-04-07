import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { User } from 'app/core/user/user.types';
import { map, Observable, ReplaySubject, tap } from 'rxjs';
import { AddressType, DeliveryStatus, Item, Period, UserType } from './lookup.types';
import { ResponseResult } from '../respons.types';

@Injectable({
    providedIn: 'root',
  })
  export class LookupService {
    private apiUrl = 'https://localhost:44331/api/Lookup';  // Adjust the URL based on your actual API URL
  
    constructor(private http: HttpClient) {}
  
    getItems(): Observable<ResponseResult<Item>> {
      return this.http.get<ResponseResult<Item>>(`${this.apiUrl}/GetItems`);
    }
  
    getAddressTypes(): Observable<ResponseResult<AddressType>> {
      return this.http.get<ResponseResult<AddressType>>(`${this.apiUrl}/GetAddressTypes`);
    }
  
    getPeriods(): Observable<ResponseResult<Period>> {
      return this.http.get<ResponseResult<Period>>(`${this.apiUrl}/GetPeriods`);
    }
  
    getDeliveryStatus(): Observable<ResponseResult<DeliveryStatus>> {
      return this.http.get<ResponseResult<DeliveryStatus>>(`${this.apiUrl}/GetDeliveryStatus`);
    }
  
    getUserTypes(): Observable<ResponseResult<UserType>> {
      return this.http.get<ResponseResult<UserType>>(`${this.apiUrl}/GetUserTypes`);
    }
  
    getCouriers(): Observable<ResponseResult<User>> {
      return this.http.get<ResponseResult<User>>(`${this.apiUrl}/GetCouriers`);
    }
  }