import { User } from './User';

export interface LoginAPIResponce {
  success: string;
  token: string;
  user: User;
}
