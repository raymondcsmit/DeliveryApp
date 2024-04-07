
// DeliveryViewModel interface
export interface DeliveryViewModel {
  deliveryId: number; 
  deliveryNumber: string;
  deliveryDate: Date | string;
  deliveryTimeRange: string;
  deliveryAddress: string;
  courierName: string;
  items: ItemViewModel[];
  editableUntil: Date | string;
}

// ItemViewModel interface
export interface ItemViewModel {
  itemId: number;
  name: string;
  description: string;
  price: number;
  quantity: number;
}


export interface DeliveryAddress {
  deliveryAddressId: number;
  deliveryDetailsId: number;
  cityId: number;
  street: string;
  road: string;
  zip: string;
  address: string;
  addressTypeId?: number; // Optional as denoted by '?'
}
export interface DeliveryDetails {
  deliveryDetailsId: number;
  orderId: number;
  deliveryDate?: Date; // Optional and using Date type for DateTime
  deliveryStatus: number;
  confirmation: boolean;
  confirmationDate?: Date; // Optional and using Date type for DateTime
  confirmationNote?: string; // Optional as it might not always be present
}
export interface OrderItem {
  orderItemsId: number;
  orderId: number;
  itemId: number;
  quantity: number;
}
export interface Order {
  orderId: number;
  userId: number;
  // OrderDate is mentioned in comments as being the same as DeliveryDate, hence omitted here
  total: number;
  orderStatusId?: number; // Optional as denoted by '?'
}
