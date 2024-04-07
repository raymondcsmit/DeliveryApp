export interface Item {
    itemId: number; 
    name: string;
    description: string;
    price: number; 
  }
  

// ActivityType model
export interface ActivityType {
    activityTypeId: number;
    activityTypeName: string;
  }
  
  // AddressType model
  export interface AddressType {
    addressTypeId: number; 
    name: string;
  }
  
  // DeliveryStatus model
  export interface DeliveryStatus {
    deliveryStatusId: number; 
    name: string;
  }
  
  // UserType model
  export interface UserType {
    userTypeId: number; 
    name: string;
  }
  
  // Period model
  export interface Period {
    periodId: number; 
    name: string;
    description: string;
  }
  
  // ScheduleStatus model
  export interface ScheduleStatus {
    scheduleStatusId: number; 
    name: string;
  }
  