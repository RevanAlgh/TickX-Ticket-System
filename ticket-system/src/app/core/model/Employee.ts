import { Roles } from './enums/Roles';

export interface EmployeeModal {
  breakCategory: any;
  breakDuration: any;
  userId: number;
  fullName: string;
  userName: string;
  mobileNumber: string;
  email: string;
  dob: string;
  address: string;
  userImage: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
  role: Roles;
}
