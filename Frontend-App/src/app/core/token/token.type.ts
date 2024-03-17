import { User } from "../user/user.types";

export interface SecurityToken {
  user: User;
  token: string;
  expireon: Date;
}