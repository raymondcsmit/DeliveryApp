import { Observable, of } from "rxjs";
import { ResponseResult } from "../respons.types";
import { DeliveryViewModel } from "./delivery.types";

export const deliveries =  [
  {
    deliveryNumber: "DEL-154",
    deliveryDate: "2024-04-16",
    deliveryTimeRange: "11:15 AM - 13:15 AM",
    deliveryAddress: "21 Broadway",
    courierName: "Emily Brown",
    items: [
      { Name: "Headphones", Quantity: 5 },
      { Name: "Smartphone", Quantity: 4 },
    ],
    editableUntil: "2024-04-12",
  },
  {
    deliveryNumber: "DEL-048",
    deliveryDate: "2024-04-19",
    deliveryTimeRange: "1:00 PM - 3:00 PM",
    deliveryAddress: "86 Oak Lane",
    courierName: "Michael Lee",
    items: [{ Name: "Headphones", Quantity: 5 }],
    editableUntil: "2024-04-17",
  },
  {
    deliveryNumber: "DEL-907",
    deliveryDate: "2024-04-12",
    deliveryTimeRange: "8:30 AM - 10:30 AM",
    deliveryAddress: "34 Elm Street",
    courierName: "Michael Lee",
    items: [{ Name: "Backpack", Quantity: 3 }],
    editableUntil: "2024-04-07",
  },
  ]
 

export function getMockUpCommingDeliveres(): ResponseResult<DeliveryViewModel[]> {
  return {
    success: true,
    message: "Success",
    statusCode: 200,
    data: deliveries as any,
  }
}

export function getMockPastDeliveres(): ResponseResult<DeliveryViewModel[]> {
      
  return {
    success: true,
    message: "Success",
    statusCode: 200,
    data: deliveries as any,
  }
}